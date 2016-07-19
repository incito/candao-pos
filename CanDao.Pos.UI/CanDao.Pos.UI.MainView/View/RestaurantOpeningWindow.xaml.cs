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
    }
}
