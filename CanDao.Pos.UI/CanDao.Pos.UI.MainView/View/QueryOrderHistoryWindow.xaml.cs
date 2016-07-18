using CanDao.Pos.UI.MainView.ViewModel;

namespace CanDao.Pos.UI.MainView.View
{
    /// <summary>
    /// 查询订单历史窗口。
    /// </summary>
    public partial class QueryOrderHistoryWindow
    {
        public QueryOrderHistoryWindow()
        {
            InitializeComponent();
            DataContext = new QueryOrderHistoryWndVm { OwnerWindow = this };
        }
    }
}
