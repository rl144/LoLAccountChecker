#region

using LoLAccountChecker.Data;

#endregion

namespace LoLAccountChecker.Views
{
    public partial class RunesWindow
    {
        public RunesWindow(Account account)
        {
            InitializeComponent();

            Title = string.Format("{0} - Runes", account.Username);

            if (account.Runes != null)
            {
                _runesDataGrid.ItemsSource = account.Runes;
            }
        }
    }
}