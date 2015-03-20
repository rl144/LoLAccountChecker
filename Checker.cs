#region

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoLAccountChecker.Data;
using LoLAccountChecker.Views;

#endregion

namespace LoLAccountChecker
{
    internal delegate void NewAccount(Account accout);

    internal static class Checker
    {
        static Checker()
        {
            Accounts = new List<Account>();
            IsChecking = false;
        }

        public static List<Account> Accounts { get; set; }
        public static bool IsChecking { get; private set; }

        public static void Start()
        {
            if (IsChecking)
            {
                return;
            }

            IsChecking = true;

            var thread = new Thread(Handler)
            {
                IsBackground = true
            };

            thread.Start();
        }

        public static void Stop()
        {
            if (!IsChecking)
            {
                return;
            }

            IsChecking = false;
        }

        public static void Refresh(bool e = false)
        {
            if (IsChecking)
            {
                return;
            }

            IsChecking = true;

            foreach (var account in Accounts.Where(a => a.State == Account.Result.Success || e))
            {
                account.State = Account.Result.Unchecked;
            }

            Start();
        }

        private static async void Handler()
        {
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

                var i = Accounts.FindIndex(a => a.Username == account.Username);
                Accounts[i] = await CheckAccount(account);

                MainWindow.Instance.UpdateControls();

                if (AccountsWindow.Instance != null)
                {
                    AccountsWindow.Instance.RefreshAccounts();
                }
            }
        }

        public static async Task<Account> CheckAccount(Account account)
        {
            var client = new Client(account.Region, account.Username, account.Password);

            await client.IsCompleted.Task;

            return client.Data;
        }
    }
}