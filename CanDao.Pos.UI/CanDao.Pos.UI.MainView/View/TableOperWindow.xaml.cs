using CanDao.Pos.Model;
using CanDao.Pos.UI.MainView.ViewModel;

namespace CanDao.Pos.UI.MainView.View
{
    /// <summary>
    /// 餐台操作窗口。
    /// </summary>
    public partial class TableOperWindow
    {
        public TableOperWindow(TableInfo tableInfo)
        {
            InitializeComponent();
            if (tableInfo.IsTakeoutTable)
                DataContext = new TableOperTakeoutWndVm(tableInfo) { OwnerWindow = this };
            else
                DataContext = new TableOperNormalWndVm(tableInfo) { OwnerWindow = this };
        }

        public TableOperWindow(string tableName)
        {
            InitializeComponent();
            DataContext = new TableOperTakeoutWndVm(tableName) { OwnerWindow = this };
        }
    }
}
