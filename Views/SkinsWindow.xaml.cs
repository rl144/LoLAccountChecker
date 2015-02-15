#region

using LoLAccountChecker.Data;

#endregion

namespace LoLAccountChecker.Views
{
    public partial class SkinsWindow
    {
        public SkinsWindow(AccountData account)
        {
            InitializeComponent();

            Title = string.Format("{0} - Skins", account.Username);

            if (account.SkinList != null)
            {
                account.SkinList.ForEach(skin => _skinsDataGrid.Items.Add(skin));
            }
        }
    }
}