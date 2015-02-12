#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PVPNetConnect;

#endregion

namespace LoL_Account_Checker
{
    public delegate void NewAccountChecked(AccountData account);

    public delegate void FinishedChecking(EventArgs args);

    internal static class Checker
    {
        public static List<AccountData> Accounts;

        public static int Percentage;

        private static List<Thread> _threads;
        private static List<LoginData> _logins;

        public static bool IsChecking { get; private set; }

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
                {
                    continue;
                }

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

                        Percentage = (Accounts.Count * 100) / numLogins;
                        NewAccount(client.Data);

                        if (_threads.Contains(Thread.CurrentThread))
                        {
                            _threads.Remove(Thread.CurrentThread);
                        }
                        Thread.CurrentThread.Abort();
                    });

                thread.Start();

                _threads.Add(thread);
            }

            while (Accounts.Count < numLogins)
            {
                Thread.Sleep(500);
            }

            _threads.ForEach(t => t.Abort());

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