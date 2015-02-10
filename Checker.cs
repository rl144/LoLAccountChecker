using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PVPNetConnect;

namespace LoL_Account_Checker
{
    public delegate void NewAccountChecked(AccountData account);

    public delegate void FinishedChecking(EventArgs args);
    
    static class Checker
    {
        public static List<AccountData> Accounts;

        public static int AccountsRemaining
        {
            get { return (_logins != null && _threads != null) ? _logins.Count + _threads.Count : 0; }
        }

        public static bool IsChecking { get; private set; }

        public static int Percentage;

        private static List<Thread> _threads;
        private static List<LoginData> _logins;

        public static event NewAccountChecked OnNewAccount;
        public static event FinishedChecking OnFinishedChecking;

        public static void Start(List<LoginData> logins, Region region, int threads, string output, bool html)
        {
            Accounts = new List<AccountData>();
            _threads = new List<Thread>();
            _logins = logins;
            IsChecking = true;
            var numLogins = logins.Count;

            while (_logins.Count > 0)
            {
                if (_threads.Count >= threads)
                {
                    Thread.Sleep(500);
                    continue;
                }

                var account = _logins.FirstOrDefault();

                if (account == null)
                    continue;

                _logins.Remove(account);

                var thread = new Thread(
                    () =>
                    {
                        var client = new Client(region, account.Username, account.Password);

                        while (!client.Completed)
                        {
                            Thread.Sleep(1000);
                        }
                        
                        Accounts.Add(client.Data);
                        _threads.Remove(Thread.CurrentThread);

                        Percentage = (Accounts.Count * 100) / numLogins;
                        NewAccount(client.Data);
                    });

                thread.Start();

                _threads.Add(thread);
            }

            while (_threads.Count > 0)
            {
                Thread.Sleep(500);
            }

            IsChecking = false;
            Percentage = 100;
            FinishedChecking(new EventArgs());
        }

        private static void NewAccount(AccountData account)
        {
            if (OnNewAccount != null)
            {
                OnNewAccount(account);
            }
        }

        private static void FinishedChecking(EventArgs args)
        {
            if (OnFinishedChecking != null)
            {
                OnFinishedChecking(args);
            }
        }
    }
}
