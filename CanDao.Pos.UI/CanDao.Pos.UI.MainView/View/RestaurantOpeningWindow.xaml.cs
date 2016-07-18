using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.UI.MainView.ViewModel;

namespace CanDao.Pos.UI.MainView.View
{
    /// <summary>
    /// 开业窗口。
    /// </summary>
    public partial class RestaurantOpeningWindow
    {
        public RestaurantOpeningWindow()
        {
            InitializeComponent();
            DataContext = new RestaurantOpeningWndVm { OwnerWindow = this };
        }

        private void RestaurantOpeningWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    MessageDialog.Warning("开业授权");
                    break;
            }

        }
    }
}
