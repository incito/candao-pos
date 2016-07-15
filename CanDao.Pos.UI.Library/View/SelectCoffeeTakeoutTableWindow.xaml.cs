using System.Collections.Generic;
using CanDao.Pos.UI.Library.ViewModel;
using Models;

namespace CanDao.Pos.UI.Library.View
{
    /// <summary>
    /// 选择咖啡外卖台窗口。
    /// </summary>
    public partial class SelectCoffeeTakeoutTableWindow
    {
        public SelectCoffeeTakeoutTableWindow(List<TableInfo> tableInfos)
        {
            InitializeComponent();
            DataContext = new SelectCoffeeTakeoutTableWndVm(tableInfos) { OwnerWindow = this };
        }

        /// <summary>
        /// 选择的咖啡外卖台。
        /// </summary>
        public TableInfo SelectedTable
        {
            get { return ((SelectCoffeeTakeoutTableWndVm)DataContext).SelectedTable; }
        }
    }
}
