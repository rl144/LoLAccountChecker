#region

using System.IO;
using System.Linq;
using System.Windows;
using LoLAccountChecker.Data;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

#endregion

namespace LoLAccountChecker.Views
{
    public partial class AccountsWindow
    {
        public AccountsWindow()
        {
            InitializeComponent();
            WindowManager.Accounts = this;

            _accountsGrid.ItemsSource = Checker.Accounts;
            _showPasswords.IsChecked = Settings.Config.ShowPasswords;
        }

        private async void BtnAddAccountClick(object sender, RoutedEventArgs e)
        {
            var settings = new LoginDialogSettings
            {
                AffirmativeButtonText = "Add",
                NegativeButtonVisibility = Visibility.Visible,
                NegativeButtonText = "Cancel"
            };

            var input = await this.ShowLoginAsync("New account", "Insert your new Account", settings);

            if (input == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(input.Username) || string.IsNullOrEmpty(input.Password))
            {
                return;
            }

            if (Checker.Accounts.Exists(a => a.Username == input.Username))
            {
                await this.ShowMessageAsync("New account", "Error: Account already exists.");
                return;
            }

            var account = new Account
            {
                Username = input.Username,
                Password = input.Password,
                State = Account.Result.Unchecked
            };

            Checker.Accounts.Add(account);

            WindowManager.Main.UpdateControls();
            RefreshAccounts();
        }

        private void BtnImportClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";

            var result = ofd.ShowDialog();

            if (result == true)
            {
                if (!File.Exists(ofd.FileName))
                {
                    return;
                }

                var accounts = Utils.GetLogins(ofd.FileName);
                var num = 0;

                foreach (var account in accounts)
                {
                    if (!Checker.Accounts.Exists(a => a.Username == account.Username))
                    {
                        Checker.Accounts.Add(account);
                        num++;
                    }
                }

                if (num > 0)
                {
                    this.ShowMessageAsync("Import", string.Format("Imported {0} accounts.", num));
                }
                else
                {
                    this.ShowMessageAsync("Import", "No new accounts found.");
                }

                RefreshAccounts();
                WindowManager.Main.UpdateControls();
            }
        }

        private void ShowPasswordsClick(object sender, RoutedEventArgs e)
        {
            Settings.Config.ShowPasswords = _showPasswords.IsChecked == true;
            RefreshAccounts();
        }

        public void RefreshAccounts()
        {
            _accountsGrid.Items.Refresh();
        }

        private async void BtnExportClick(object sender, RoutedEventArgs e)
        {
            var accounts = Checker.Accounts.Where(a => a.State != Account.Result.Unchecked).ToList();

            if (!accounts.Any())
            {
                return;
            }

            var sfd = new SaveFileDialog();
            sfd.FileName = "output";
            sfd.Filter = "Text file (*.txt)|*.txt";

            if (sfd.ShowDialog() == false)
            {
                return;
            }

            var settings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                FirstAuxiliaryButtonText = "Cancel"
            };

            var dialog =
                await
                    this.ShowMessageAsync(
                        "Export", "Export errors?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                        settings);

            if (dialog == MessageDialogResult.FirstAuxiliary)
            {
                return;
            }

            var exportErrors = dialog == MessageDialogResult.Affirmative;


            Utils.ExportLogins(sfd.FileName, accounts, exportErrors);

            await this.ShowMessageAsync("Export", "All the accounts have been exported!");
        }

        private void CmCopyComboClick(object sender, RoutedEventArgs e)
        {
            var account = _accountsGrid.SelectedItem as Account;

            if (account == null)
            {
                return;
            }

            Clipboard.SetText(string.Format("{0}:{1}", account.Username, account.Password));

            this.ShowMessageAsync("Copy combo", "Combo copied to clipboard!");
        }

        private async void CmMakeUncheckedClick(object sender, RoutedEventArgs e)
        {
            var account = _accountsGrid.SelectedItem as Account;

            if (account == null)
            {
                return;
            }

            if (account.State == Account.Result.Unchecked)
            {
                await this.ShowMessageAsync("Make Unchecked", "This account has not been checked yet.");
                return;
            }

            if (account.State == Account.Result.Success)
            {
                var confirm =
                    await
                        this.ShowMessageAsync(
                            "Make Unchecked",
                            "This account was successfully checked, are you sure that you wanna make it unchecked?",
                            MessageDialogStyle.AffirmativeAndNegative);

                if (confirm == MessageDialogResult.Negative)
                {
                    return;
                }

                WindowManager.Main.RemoveAccount(account);
            }

            account.State = Account.Result.Unchecked;
            RefreshAccounts();
            await this.ShowMessageAsync("Make Unchecked", "Account state has been changed to Unchecked.");
        }
    }
}