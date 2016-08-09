using CanDao.Pos.Common;

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
            DataContext = new NormalWindowViewModel { OwnerWindow = this };
        }
    }
}
