#region

using System.Diagnostics;
using System.Linq;
using System.Windows;
using LoLAccountChecker.Data;

#endregion

namespace LoLAccountChecker.Views
{
    public partial class SkinsWindow
    {
        public SkinsWindow(Account account)
        {
            InitializeComponent();

            Title = string.Format("{0} - Skins", account.Username);

            if (account.SkinList != null)
            {
                account.SkinList.ForEach(skin => _skinsDataGrid.Items.Add(skin));
            }
        }

        private void CmViewModel(object sender, RoutedEventArgs e)
        {
            // old versions didn't save skin id and champion id in the exported files 
            var selectedSkin = _skinsDataGrid.SelectedItem as SkinData;

            if (selectedSkin == null)
            {
                return;
            }

            if (selectedSkin.Skin == null)
            {
                var champion = LeagueData.Champions.FirstOrDefault(c => c.Skins.Any(s => s.Name == selectedSkin.Name));

                if (champion == null)
                {
                    return;
                }

                var skin = champion.Skins.FirstOrDefault(s => s.Name == selectedSkin.Name);

                if (skin == null)
                {
                    return;
                }

                selectedSkin.Skin = skin;
                selectedSkin.Champion = champion;

            }

            Process.Start(
                string.Format(
                    "http://www.lolking.net/models/?champion={0}&skin={1}", selectedSkin.Champion.Id,
                    selectedSkin.Skin.Number - 1));
        }
    }
}