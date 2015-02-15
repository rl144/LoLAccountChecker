#region

using System;
using System.Collections.Generic;

#endregion

namespace LoLAccountChecker.Data
{
    public class AccountData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Summoner { get; set; }
        public int Level { get; set; }
        public string EmailStatus { get; set; }
        public int RpBalance { get; set; }
        public int IpBalance { get; set; }
        public int Champions { get; set; }
        public int Skins { get; set; }
        public int RunePages { get; set; }
        public string SoloQRank { get; set; }
        public DateTime LastPlay { get; set; }
        public List<ChampionData> ChampionList { get; set; }
        public List<SkinData> SkinList { get; set; }
        public string ErrorMessage { get; set; }
        public Client.Result Result { get; set; }
    }
}