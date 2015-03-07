#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;

#endregion

namespace LoLAccountChecker.Data
{
    internal static class LeagueData
    {
        private static string _repoUrl;
        private static List<string> _files;

        static LeagueData()
        {
            _repoUrl = "https://raw.githubusercontent.com/madk/LoLAccountChecker/master/";
            _files = new List<string> { "League/Champions.json", "League/Runes.json" };
        }

        public static List<Champion> Champions { get; private set; }
        public static List<Rune> Runes { get; private set; }

        public static async void Load()
        {
            var toDownload = _files.Where(f => !File.Exists(f));

            if (toDownload.Any())
            {
                var fileCount = 0;

                var wc = new WebClient();
                var dialog = await WindowManager.Main.ShowProgressAsync("Updating", "Downloading required files...");
                wc.DownloadProgressChanged += (o, p) => dialog.SetProgress(p.ProgressPercentage / 100d);
                wc.DownloadFileCompleted += (o, a) =>
                {
                    if (fileCount >= toDownload.Count())
                    {
                        dialog.CloseAsync();
                    }
                };

                if (!Directory.Exists("League"))
                {
                    Directory.CreateDirectory("League");
                }

                foreach (var file in toDownload)
                {
                    fileCount++;
                    await wc.DownloadFileTaskAsync(new Uri(_repoUrl + file), file);
                }
            }

            using (var sr = new StreamReader("League/Champions.json"))
            {
                Champions = JsonConvert.DeserializeObject<List<Champion>>(sr.ReadToEnd());
            }

            using (var sr = new StreamReader("League/Runes.json"))
            {
                Runes = JsonConvert.DeserializeObject<List<Rune>>(sr.ReadToEnd());
            }
        }
    }
}