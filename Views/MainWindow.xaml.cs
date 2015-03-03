#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using LoLAccountChecker.Data;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Newtonsoft.Json;
using PVPNetConnect;

#endregion

namespace LoLAccountChecker.Views
{
    internal delegate void UpdateControls();

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            WindowManager.Main = this;

            // Init Checker
            Checker.OnNewAccount += OnNewAccount;

            // Init Regions
            _regionsComboBox.ItemsSource = Enum.GetValues(typeof(Region)).Cast<Region>();
            _regionsComboBox.SelectedItem = Settings.Config.SelectedRegion;

            // Init Champion Data
            if (File.Exists("Champions.json"))
            {
                LeagueData.UpdateData("Champions.json");
            }
            else
            {
                Loaded += (sender, args) => LeagueData.DownloadFile();
            }

            Closed += ClosedWindow;
        }

        private void ClosedWindow(object sender, EventArgs e)
        {
            Settings.Save();
            Application.Current.Shutdown();
        }

        private void OnNewAccount(Account account)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new NewAccount(OnNewAccount), account);
                return;
            }

            _progressBar.Value = (Checker.Accounts.Count(a => a.State != Account.Result.Unchecked) * 100f) /
                                 Checker.Accounts.Count();

            if (account.State == Account.Result.Success)
            {
                _accountsDataGrid.Items.Add(account);

                if (!_exportButton.IsEnabled)
                {
                    _exportButton.IsEnabled = true;
                }
            }

            UpdateControls();

            if (Checker.Accounts.All(a => a.State != Account.Result.Unchecked))
            {
                this.ShowMessageAsync("Done", "All the accounts have been checked!");
            }
        }

        public void UpdateControls()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new UpdateControls(UpdateControls));
                return;
            }

            var numCheckedAcccounts = Checker.Accounts.Count(a => a.State != Account.Result.Unchecked);

            _checkedLabel.Content = string.Format("Checked: {0}/{1}", numCheckedAcccounts, Checker.Accounts.Count);

            if (numCheckedAcccounts < Checker.Accounts.Count)
            {
                _startButton.IsEnabled = true;
            }

            if (Checker.IsChecking)
            {
                _statusLabel.Content = "Status: Checking...";
            }
            else if (numCheckedAcccounts > 0 && Checker.Accounts.All(a => a.State != Account.Result.Unchecked))
            {
                _statusLabel.Content = "Status: Finished!";
            }

            _startButton.Content = Checker.IsChecking ? "Stop" : "Start";
            _startButton.IsEnabled = numCheckedAcccounts < Checker.Accounts.Count;
            _exportButton.IsEnabled = numCheckedAcccounts > 0;
        }

        private void BtnDonateClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=CHEV6LWPMHUMW");
        }

        #region Import Button

        private void BtnImportClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();

            ofd.Filter = "JavaScript Object Notation (*.json)|*.json";

            var result = ofd.ShowDialog();

            if (result == true)
            {
                var file = ofd.FileName;
                if (!File.Exists(ofd.FileName))
                {
                    return;
                }

                List<Account> accounts;
                var num = 0;

                using (var sr = new StreamReader(file))
                {
                    accounts = JsonConvert.DeserializeObject<List<Account>>(sr.ReadToEnd());
                }

                foreach (var account in accounts)
                {
                    if (!Checker.Accounts.Exists(a => a.Username == account.Username))
                    {
                        Checker.Accounts.Add(account);

                        if (account.State == Account.Result.Success)
                        {
                            _accountsDataGrid.Items.Add(account);
                        }

                        num++;
                    }
                }

                UpdateControls();

                if (num > 0)
                {
                    this.ShowMessageAsync("Import", string.Format("Imported {0} accounts.", num));
                }
                else
                {
                    this.ShowMessageAsync("Import", "No new accounts found.");
                }
            }
        }

        #endregion

        #region Export Button

        private void BtnExportToFileClick(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.FileName = "output";
            sfd.Filter = "JavaScript Object Notation (*.json)|*.json";

            if (sfd.ShowDialog() == true)
            {
                var file = sfd.FileName;

                Utils.ExportAsJson(file, Checker.Accounts, true);
                this.ShowMessageAsync("Export", string.Format("Exported {0} accounts.", Checker.Accounts.Count));
            }
        }

        #endregion

        #region Accounts Button

        private void BtnAccountsClick(object sender, RoutedEventArgs e)
        {
            if (WindowManager.Accounts == null)
            {
                WindowManager.Accounts = new AccountsWindow();
                WindowManager.Accounts.Show();
                WindowManager.Accounts.Closed += (o, a) => { WindowManager.Accounts = null; };
            }
            else if (WindowManager.Accounts != null && !WindowManager.Accounts.IsActive)
            {
                WindowManager.Accounts.Activate();
            }
        }

        #endregion

        #region Start Button

        private void BtnStartCheckingClick(object sender, RoutedEventArgs e)
        {
            if (Checker.IsChecking)
            {
                Checker.Stop();
                _startButton.Content = "Start";
                _statusLabel.Content = "Status: Stopped!";
                return;
            }

            if (Checker.Accounts.All(a => a.State != Account.Result.Unchecked))
            {
                this.ShowMessageAsync("Error", "All accounts have been checked.");
                return;
            }

            _startButton.Content = "Stop";
            _statusLabel.Content = "Status: Checking...";

            var thread = new Thread(Checker.Start);
            thread.IsBackground = true;
            thread.Start();
        }

        #endregion

        #region Context Menu

        private void CmCopyComboClick(object sender, RoutedEventArgs e)
        {
            var account = _accountsDataGrid.SelectedItem as Account;

            if (account == null)
            {
                return;
            }

            var combo = string.Format("{0}:{1}", account.Username, account.Password);
            Clipboard.SetText(combo);
        }

        private void CmViewChampionsClick(object sender, RoutedEventArgs e)
        {
            var account = _accountsDataGrid.SelectedItem as Account;

            if (account == null)
            {
                return;
            }

            var window = new ChampionsWindow(account);
            window.Show();
        }

        private void CmViewSkinsClick(object sender, RoutedEventArgs e)
        {
            var account = _accountsDataGrid.SelectedItem as Account;

            if (account == null)
            {
                return;
            }

            var window = new SkinsWindow(account);
            window.Show();
        }

        #endregion

        #region Regions Combo Box

        private void CbRegionOnChangeSelection(object sender, SelectionChangedEventArgs e)
        {
            Settings.Config.SelectedRegion = (Region) _regionsComboBox.SelectedIndex;
        }

        #endregion
    }
}