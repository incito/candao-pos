using CanDao.Pos.Model;
using CanDao.Pos.UI.MainView.ViewModel;

namespace CanDao.Pos.UI.MainView.View
{
    /// <summary>
    /// 点菜窗口。
    /// </summary>
    public partial class OrderDishWindow
    {
        public OrderDishWindow(TableFullInfo info)
        {
            InitializeComponent();
            DataContext = new OrderDishWndVm(info) { OwnerWindow = this };
        }

        public OrderDishWindow(TableInfo info)
        {
            InitializeComponent();
            var tableFullInfo = new TableFullInfo();
            tableFullInfo.CloneDataFromTableInfo(info);
            DataContext = new OrderDishWndVm(tableFullInfo) { OwnerWindow = this };
        }
    }
}
