#region

using System.IO;
using Newtonsoft.Json;

#endregion

namespace LoLAccountChecker.Data
{
    internal class LeagueData
    {
        public static ChampionFull Data;

        public static void UpdateData(string file)
        {
            var sr = new StreamReader(file);
            var data = sr.ReadToEnd();
            sr.Close();

            Data = JsonConvert.DeserializeObject<ChampionFull>(data);
        }
    }
}