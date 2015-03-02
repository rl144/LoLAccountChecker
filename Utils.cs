#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LoLAccountChecker.Data;
using Newtonsoft.Json;

#endregion

namespace LoLAccountChecker
{
    internal class Utils
    {
        public static List<Account> GetLogins(string file)
        {
            var logins = new List<Account>();

            var sr = new StreamReader(file);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var accountData = line.Split(new[] { ':' });

                if (accountData.Count() < 2)
                {
                    continue;
                }

                var loginData = new Account
                {
                    Username = accountData[0],
                    Password = accountData[1],
                    State = Account.Result.Unchecked
                };

                logins.Add(loginData);
            }

            return logins;
        }

        public static void ExportAsJson(string file, List<Account> accounts, bool exportErrors)
        {
            using (var sw = new StreamWriter(file))
            {
                if (!exportErrors)
                {
                    accounts = accounts.Where(a => a.State == Account.Result.Success).ToList();
                }

                sw.Write(JsonConvert.SerializeObject(accounts));
            }
        }

        public static void ExportException(Exception e)
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var file = string.Format("crash_{0:dd-MM-yyyy_HH-mm-ss}.txt", DateTime.Now);

            using (var sw = new StreamWriter(Path.Combine(dir, file)))
            {
                sw.WriteLine(e.ToString());
            }
        }
    }
}