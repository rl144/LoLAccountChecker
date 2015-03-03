#region

using System;
using System.IO;
using System.Net;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;

#endregion

namespace LoLAccountChecker.Data
{
    internal class LeagueData
    {
        public static ChampionFull Champions;

        public static void UpdateData(string file)
        {
            var sr = new StreamReader(file);
            var data = sr.ReadToEnd();
            sr.Close();

            Champions = JsonConvert.DeserializeObject<ChampionFull>(data);
        }

        public static async void DownloadFile()
        {
            var wc = new WebClient();
            var dialog = await WindowManager.Main.ShowProgressAsync("Updating", "Updating Champions.json");
            wc.DownloadProgressChanged += (o, p) => dialog.SetProgress(p.ProgressPercentage/100d);
            wc.DownloadFileCompleted += (o, a) => dialog.CloseAsync();
            wc.DownloadFileAsync(new Uri("https://raw.githubusercontent.com/madk/LoLAccountChecker/master/Champions.json"), "Champions.json");
        }
    }
}