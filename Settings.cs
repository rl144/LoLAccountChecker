#region

using System.IO;
using Newtonsoft.Json;
using PVPNetConnect;

#endregion

namespace LoLAccountChecker
{
    internal class Settings
    {
        private static string _file;

        public static Settings Config;

        static Settings()
        {
            _file = "settings.json";

            if (!File.Exists(_file))
            {
                Config = new Settings
                {
                    ShowPasswords = true,
                    SelectedRegion = Region.NA
                };
                Save();
                return;
            }

            Load();
        }

        public bool ShowPasswords { get; set; }
        public Region SelectedRegion { get; set; }
        public string ClientVersion { get; set; }

        public static void Save()
        {
            using (var sw = new StreamWriter(_file))
            {
                sw.Write(JsonConvert.SerializeObject(Config, Formatting.Indented));
            }
        }

        public static void Load()
        {
            using (var sr = new StreamReader(_file))
            {
                Config = JsonConvert.DeserializeObject<Settings>(sr.ReadToEnd());
            }
        }
    }
}