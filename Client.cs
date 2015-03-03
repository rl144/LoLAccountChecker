#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoLAccountChecker.Data;
using PVPNetConnect;

#endregion

namespace LoLAccountChecker
{
    public class Client
    {
        public PVPNetConnection Connection;
        public Account Data;

        public TaskCompletionSource<bool> IsCompleted;

        public Client(Region region, string username, string password)
        {
            Data = new Account { Username = username, Password = password };
            IsCompleted = new TaskCompletionSource<bool>();
            Completed = false;

            Connection = new PVPNetConnection();
            Connection.OnLogin += OnLogin;
            Connection.OnError += OnError;

            Connection.Connect(username, password, region, "5.4.15_02_19_22_27");
        }

        public bool Completed { get; private set; }

        public void Disconnect()
        {
            if (!Connection.IsConnected())
            {
                return;
            }

            Connection.Disconnect();
        }

        private void OnLogin(object sender, string username, string ipAddress)
        {
            GetData();
        }

        private void OnError(object sender, Error error)
        {
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
                    break;
            }

#if DEBUG
            Data.ErrorMessage += string.Format(" - Message: {0}", error.Message);
#endif
            Data.State = Account.Result.Error;

            IsCompleted.TrySetResult(true);
        }

        public async void GetData()
        {
            try
            {
                var loginPacket = await Connection.GetLoginDataPacketForUser();
                if (loginPacket.AllSummonerData == null)
                {
                    Data.ErrorMessage = "Summoner not created.";
                    Data.State = Account.Result.Error;

                    IsCompleted.TrySetResult(true);
                    return;
                }

                var champions = await Connection.GetAvailableChampions();

                Data.ChampionList = new List<ChampionData>();
                Data.SkinList = new List<SkinData>();

                foreach (var champion in champions)
                {
                    var cdata = LeagueData.Champions.data.FirstOrDefault(c => Int32.Parse(c.key) == champion.ChampionId);

                    if (cdata == null)
                    {
                        continue; // champion not implemented in json file ?
                    }

                    if (champion.Owned)
                    {
                        Data.ChampionList.Add(
                            new ChampionData
                            {
                                Name = cdata.name,
                                PurchaseDate =
                                    new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(
                                        Math.Round(champion.PurchaseDate / 1000d))
                            });
                    }

                    foreach (var skin in champion.ChampionSkins.Where(skin => skin.Owned))
                    {
                        var sdata = cdata.skins.FirstOrDefault(s => Int32.Parse(s.id) == skin.SkinId);

                        if (sdata == null)
                        {
                            continue; // skin not implemented in json file ?
                        }

                        Data.SkinList.Add(new SkinData { Name = sdata.name, StillObtainable = skin.StillObtainable });
                    }
                }

                Data.Level = (int) loginPacket.AllSummonerData.SummonerLevel.Level;
                Data.RpBalance = (int) loginPacket.RpBalance;
                Data.IpBalance = (int) loginPacket.IpBalance;
                Data.Champions = Data.ChampionList.Count;
                Data.Skins = Data.SkinList.Count;
                Data.RunePages = loginPacket.AllSummonerData.SpellBook.BookPages.Count;
                Data.Summoner = loginPacket.AllSummonerData.Summoner.Name;

                if (loginPacket.EmailStatus != null)
                {
                    var emailStatus = loginPacket.EmailStatus.Replace('_', ' ');
                    Data.EmailStatus = Char.ToUpper(emailStatus[0]) + emailStatus.Substring(1);
                }
                else
                {
                    Data.EmailStatus = "Unknown";
                }

                // Leagues
                if (Data.Level == 30)
                {
                    var myLeagues = await Connection.GetMyLeaguePositions();
                    var soloqLeague = myLeagues.SummonerLeagues.FirstOrDefault(l => l.QueueType == "RANKED_SOLO_5x5");
                    Data.SoloQRank = soloqLeague != null
                        ? string.Format(
                            "{0}{1} {2}", char.ToUpper(soloqLeague.Tier[0]), soloqLeague.Tier.Substring(1).ToLower(),
                            soloqLeague.Rank)
                        : "Unranked";
                }
                else
                {
                    Data.SoloQRank = "Unranked";
                }

                // Last Play
                var recentGames = await Connection.GetRecentGames(loginPacket.AllSummonerData.Summoner.AcctId);
                var lastGame = recentGames.GameStatistics.FirstOrDefault();

                if (lastGame != null)
                {
                    Data.LastPlay = lastGame.CreateDate;
                }

                Data.State = Account.Result.Success;
            }
            catch (Exception e)
            {
                Utils.ExportException(e);
                Data.ErrorMessage = string.Format("Exception found: {0}", e.Message);
                Data.State = Account.Result.Error;
            }

            IsCompleted.TrySetResult(true);
        }
    }
}