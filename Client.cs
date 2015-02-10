#region

using System;
using System.Collections.Generic;
using System.Linq;
using PVPNetConnect;
using PVPNetConnect.RiotObjects.Platform.Catalog.Champion;

#endregion

namespace LoL_Account_Checker
{
    public class Client
    {
        public enum Result
        {
            Success,
            Error
        }

        public PVPNetConnection Connection;
        public AccountData Data;

        public Client(Region region, string username, string password)
        {
            Data = new AccountData { Username = username, Password = password };
            Completed = false;

            Connection = new PVPNetConnection();
            Connection.OnLogin += OnLogin;
            Connection.OnError += OnError;

            Connection.Connect(username, password, region, "5.2.15");

            Console.WriteLine(@"[{0:HH:mm}] <{1}> Connecting to PvP.net", DateTime.Now, Data.Username);
        }

        public bool Completed { get; private set; }

        public void Disconnect()
        {
            if (!Connection.IsConnected())
            {
                return;
            }

            Connection.Disconnect();
            Console.WriteLine(@"[{0:HH:mm}] <{1}> Disconnecting", DateTime.Now, Data.Username);
        }

        private void OnLogin(object sender, string username, string ipAddress)
        {
            Console.WriteLine(@"[{0:HH:mm}] <{1}> Logged in!", DateTime.Now, Data.Username);
            GetData();
        }

        private void OnError(object sender, Error error)
        {
            if (Completed)
            {
                return;
            }

            switch (error.Type)
            {
                case ErrorType.AuthKey:
                    Data.ErrorMessage = "Unable to authenticate";
                    break;
                case ErrorType.Connect:
                    Data.ErrorMessage = "Unable to connect to PvP.Net";
                    break;
                case ErrorType.Receive:
                    if (error.ErrorCode == "LOGIN-0018")
                    {
                        Data.ErrorMessage = "This account requires a password change in order to login.";
                    }
                    break;

                default:
                    Data.ErrorMessage = string.Format(
                        "Unregisted error - Type: {0} - Code: {1}", error.Type, error.ErrorCode);
#if DEBUG
                    ErrorMessage += string.Format(" - Message: {0}", error.Message)
#endif
                    break;
            }

            Console.WriteLine(@"[{0:HH:mm}] <{1}> Error: {2}", DateTime.Now, Data.Username, Data.ErrorMessage);

            Report(Result.Error);
        }

        public async void GetData()
        {
            var loginPacket = await Connection.GetLoginDataPacketForUser();
            if (loginPacket.AllSummonerData == null)
            {
                Data.ErrorMessage = "Summoner not created.";
                Console.WriteLine(@"[{0:HH:mm}] <{1}> Error: {2}", DateTime.Now, Data.Username, Data.ErrorMessage);
                Report(Result.Error);
                return;
            }

            var champions = await Connection.GetAvailableChampions();
            var skins = new List<ChampionSkinDTO>();

            foreach (var champion in champions)
            {
                skins.AddRange(champion.ChampionSkins.Where(s => s.Owned));
            }

            Data.Level = (int) loginPacket.AllSummonerData.SummonerLevel.Level;
            Data.RpBalance = (int) loginPacket.RpBalance;
            Data.Ipbalance = (int) loginPacket.IpBalance;
            Data.Champions = champions.Count(c => c.Owned);
            Data.Skins = skins.Count;
            Data.RunePages = loginPacket.AllSummonerData.SpellBook.BookPages.Count;
            Data.SummonerName = loginPacket.AllSummonerData.Summoner.Name;
            Data.EmailStatus = loginPacket.EmailStatus;

            // Leagues
            if (Data.Level == 30)
            {
                var myLeagues = await Connection.GetMyLeaguePositions();
                var soloqLeague = myLeagues.SummonerLeagues.FirstOrDefault(l => l.QueueType == "RANKED_SOLO_5x5");
                Data.SoloQRank = soloqLeague != null
                    ? string.Format("{0} {1}", soloqLeague.Tier, soloqLeague.Rank)
                    : "UNRANKED";
            }
            else
            {
                Data.SoloQRank = "UNRANKED";
            }

            // Last Play
            var recentGames = await Connection.GetRecentGames(loginPacket.AllSummonerData.Summoner.AcctId);
            var lastGame = recentGames.GameStatistics.FirstOrDefault();

            if (lastGame != null)
            {
                Data.LastPlay = lastGame.CreateDate;
            }

            Console.WriteLine(@"[{0:HH:mm}] <{1}> Data received!", DateTime.Now, Data.Username);
            Report(Result.Success);
        }

        protected virtual void Report(Result result)
        {
            Data.Result = result;
            Completed = true;
        }
    }
}