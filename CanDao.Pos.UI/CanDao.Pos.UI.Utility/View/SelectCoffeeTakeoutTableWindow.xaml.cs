using System.Collections.Generic;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 咖啡外卖桌台选择窗口。
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

        /// <summary>
        /// 是否选择普通外卖。
        /// </summary>
        public bool IsSelectNormalTakeout
        {
            get { return ((SelectCoffeeTakeoutTableWndVm)DataContext).IsSelectNormalTakeout; }
        }
    }
}
