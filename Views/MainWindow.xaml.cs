﻿#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using LoLAccountChecker.Data;
using Microsoft.Win32;
using Newtonsoft.Json;
using PVPNetConnect;

#endregion

namespace LoLAccountChecker.Views
{
    public partial class MainWindow
    {
        private ErrorsWindow _errorsWindow;

        public MainWindow()
        {
            InitializeComponent();

            // Init Checker
            Checker.AccountsChecked = new List<AccountData>();
            Checker.AccountsToCheck = new List<LoginData>();
            Checker.OnNewAccount += OnNewAccount;

            // Init Regions
            _regionsComboBox.ItemsSource = Enum.GetValues(typeof(Region)).Cast<Region>();
            _regionsComboBox.SelectedIndex = 0;

            // Init Champion Data
            if (File.Exists("Champions.json"))
            {
                LeagueData.UpdateData("Champions.json");
            }
        }

        private void OnNewAccount(AccountData account)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new NewAccount(OnNewAccount), account);
                return;
            }

            _progressBar.Value = (Checker.AccountsChecked.Count * 100f) / Checker.AccountsToCheck.Count;
            _checkedLabel.Content = string.Format(
                "Checked: {0}/{1}", Checker.AccountsChecked.Count, Checker.AccountsToCheck.Count);

            if (account.Result == Client.Result.Success)
            {
                _accountsDataGrid.Items.Add(account);

                if (!_exportButton.IsEnabled)
                {
                    _exportButton.IsEnabled = true;
                }
            }
            else
            {
                if (!_errorsButton.IsEnabled)
                {
                    _errorsButton.IsEnabled = true;
                }

                if (_errorsWindow != null)
                {
                    _errorsWindow.AddAccount(account);
                }
            }

            if (Checker.AccountsChecked.Count >= Checker.AccountsToCheck.Count)
            {
                _startButton.IsEnabled = true;
                _loadButton.IsEnabled = true;
                _statusLabel.Content = string.Format("Status: Finished!");
            }
        }

        private void BtnLoadFileClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();

            ofd.Filter = "Text Files (*.txt)|*.*|JavaScript Object Notation (*.json)|*.*";

            var result = ofd.ShowDialog();

            if (result == true)
            {
                var file = ofd.FileName;

                if (File.Exists(file))
                {
                    if (file.EndsWith(".json"))
                    {
                        List<AccountData> accounts;
                        using (var sr = new StreamReader(file))
                        {
                            accounts = JsonConvert.DeserializeObject<List<AccountData>>(sr.ReadToEnd());
                        }

                        foreach (var account in accounts)
                        {
                            if (
                                !Checker.AccountsChecked.Exists(
                                    a => a.Username == account.Username && a.Result == Client.Result.Success))
                            {
                                Checker.AccountsChecked.Add(account);
                                _accountsDataGrid.Items.Add(account);
                            }
                        }
                    }
                    else
                    {
                        var logins = Utils.GetLogins(file);
                        Checker.AddLogins(logins);

                        if (logins.Any())
                        {
                            _startButton.IsEnabled = true;
                        }
                    }

                    _checkedLabel.Content = string.Format(
                        "Checked: {0}/{1}", Checker.AccountsChecked.Count, Checker.AccountsToCheck.Count);
                }
            }
        }

        private void BtnExportToFileClick(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.FileName = "output";
            sfd.Filter = "Text Files (*.txt)|*.*|Html Files (*.html)|*.*|JavaScript Object Notation (*.json)|*.*";

            if (sfd.ShowDialog() == true)
            {
                var file = sfd.FileName;

                if (file.EndsWith(".html"))
                {
                    Utils.ExportAsHtml(file, Checker.AccountsChecked, false);
                }
                else if (file.EndsWith(".json"))
                {
                    Utils.ExportAsJson(file, Checker.AccountsChecked, false);
                }
                else
                {
                    Utils.ExportAsText(file, Checker.AccountsChecked, false);
                }
            }
        }

        private void BtnErrorsClick(object sender, RoutedEventArgs e)
        {
            if (_errorsWindow == null)
            {
                _errorsWindow = new ErrorsWindow();
                _errorsWindow.Show();
                _errorsWindow.Closed += (o, args) => { _errorsWindow = null; };
            }
            else if (_errorsWindow != null && !_errorsWindow.IsActive)
            {
                _errorsWindow.Activate();
            }
        }

        private void BtnStartCheckingClick(object sender, RoutedEventArgs e)
        {
            if (Checker.AccountsToCheck.Count <= Checker.AccountsChecked.Count)
            {
                return;
            }

            _loadButton.IsEnabled = false;
            _startButton.IsEnabled = false;
            _statusLabel.Content = "Status: Checking...";

            var thread = new Thread(Checker.Start);
            thread.IsBackground = true;
            thread.Start();
        }

        private void CmCopyComboClick(object sender, RoutedEventArgs e)
        {
            var account = _accountsDataGrid.SelectedItem as AccountData;

            if (account == null)
            {
                return;
            }

            var combo = string.Format("{0}:{1}", account.Username, account.Password);
            Clipboard.SetText(combo);
        }

        private void CmViewChampionsClick(object sender, RoutedEventArgs e)
        {
            var account = _accountsDataGrid.SelectedItem as AccountData;

            if (account == null)
            {
                return;
            }

            var window = new ChampionsWindow(account);
            window.Show();
        }

        private void CmViewSkinsClick(object sender, RoutedEventArgs e)
        {
            var account = _accountsDataGrid.SelectedItem as AccountData;

            if (account == null)
            {
                return;
            }

            var window = new SkinsWindow(account);
            window.Show();
        }

        private void CbRegionOnChangeSelection(object sender, SelectionChangedEventArgs e)
        {
            Checker.SelectedRegion = (Region) _regionsComboBox.SelectedIndex;
        }
    }
}