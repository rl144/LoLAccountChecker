#region

using LoLAccountChecker.Data;

#endregion

namespace LoLAccountChecker.Views
{
    public partial class ChampionsWindow
    {
        public ChampionsWindow(AccountData account)
        {
            InitializeComponent();

            Title = string.Format("{0} - Champions", account.Username);

            if (account.ChampionList != null)
            {
                account.ChampionList.ForEach(champion => _championsDataGrid.Items.Add(champion));
            }
        }
    }
}