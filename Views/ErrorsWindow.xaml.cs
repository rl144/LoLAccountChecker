#region

using System.Linq;
using LoLAccountChecker.Data;

#endregion

namespace LoLAccountChecker.Views
{
    public partial class ErrorsWindow
    {
        public ErrorsWindow()
        {
            InitializeComponent();

            foreach (var account in Checker.AccountsChecked.Where(account => account.Result == Client.Result.Error))
            {
                _errorsDataGrid.Items.Add(account);
            }
        }

        public void AddAccount(AccountData account)
        {
            _errorsDataGrid.Items.Add(account);
        }
    }
}