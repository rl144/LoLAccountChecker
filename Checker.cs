#region

using System.Collections.Generic;
using System.Linq;
using LoLAccountChecker.Data;
using PVPNetConnect;

#endregion

namespace LoLAccountChecker
{
    internal delegate void NewAccount(AccountData accout);

    internal static class Checker
    {
        public static NewAccount OnNewAccount;
        private static Region _selectedRegion;

        static Checker()
        {
            IsChecking = false;
        }

        public static List<LoginData> AccountsToCheck { get; set; }
        public static List<AccountData> AccountsChecked { get; set; }
        public static bool IsChecking { get; private set; }

        public static Region SelectedRegion
        {
            get { return _selectedRegion; }
            set
            {
                if (IsChecking)
                {
                    return;
                }

                _selectedRegion = value;
            }
        }

        public static async void Start()
        {
            if (IsChecking)
            {
                return;
            }

            IsChecking = true;

            foreach (var account in AccountsToCheck.Where(a => AccountsChecked.All(c => c.Username != a.Username)))
            {
                var client = new Client(SelectedRegion, account.Username, account.Password);
                await client.IsCompleted.Task;
                var data = client.Data;
                AccountsChecked.Add(data);

                ReportNewAccount(data);
            }

            IsChecking = false;
        }

        public static void AddLogin(LoginData login)
        {
            if (AccountsToCheck.Any(account => account.Username == login.Username))
            {
                return; // Account already exists
            }

            if (AccountsChecked.Any(account => account.Username == login.Username))
            {
                return; // Account already checked
            }

            AccountsToCheck.Add(login);
        }

        public static void AddLogins(List<LoginData> loginList)
        {
            loginList.ForEach(AddLogin);
        }


        private static void ReportNewAccount(AccountData data)
        {
            if (OnNewAccount != null)
            {
                OnNewAccount(data);
            }
        }
    }
}