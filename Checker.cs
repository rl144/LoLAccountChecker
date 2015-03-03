#region

using System.Collections.Generic;
using System.Linq;
using LoLAccountChecker.Data;

#endregion

namespace LoLAccountChecker
{
    internal delegate void NewAccount(Account accout);

    internal static class Checker
    {
        public static NewAccount OnNewAccount;

        static Checker()
        {
            Accounts = new List<Account>();
            IsChecking = false;
        }

        public static List<Account> Accounts { get; set; }
        public static bool IsChecking { get; private set; }

        public static async void Start()
        {
            if (IsChecking)
            {
                return;
            }

            IsChecking = true;

            var region = Settings.Config.SelectedRegion;

            while (Accounts.Any(a => a.State == Account.Result.Unchecked))
            {
                if (!IsChecking)
                {
                    break;
                }

                var account = Accounts.FirstOrDefault(a => a.State == Account.Result.Unchecked);

                if (account == null)
                {
                    continue;
                }

                var client = new Client(region, account.Username, account.Password);
                await client.IsCompleted.Task;
                var data = client.Data;
                var i = Accounts.FindIndex(a => a.Username == account.Username);
                Accounts[i] = data;
                ReportNewAccount(data);
            }

            IsChecking = false;

            WindowManager.Main.UpdateControls();
        }

        public static void Stop()
        {
            if (!IsChecking)
            {
                return;
            }

            IsChecking = false;
        }


        private static void ReportNewAccount(Account data)
        {
            if (OnNewAccount != null)
            {
                OnNewAccount(data);
            }
        }
    }
}