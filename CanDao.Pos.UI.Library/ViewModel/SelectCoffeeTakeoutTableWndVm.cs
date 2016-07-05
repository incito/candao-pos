using System.Collections.Generic;
using System.Collections.ObjectModel;
using CanDao.Pos.Common;
using Models;

namespace CanDao.Pos.UI.Library.ViewModel
{
    /// <summary>
    /// 选择咖啡模式外卖台窗口的VM。
    /// </summary>
    public class SelectCoffeeTakeoutTableWndVm : NormalWindowViewModel
    {
        public SelectCoffeeTakeoutTableWndVm(List<TableInfo> tableInfos)
        {
            CoffeeTakeoutTables = new ObservableCollection<TableInfo>();
            if(tableInfos != null)
                tableInfos.ForEach(CoffeeTakeoutTables.Add);
        }

        /// <summary>
        /// 咖啡模式外卖台集合。
        /// </summary>
        public ObservableCollection<TableInfo> CoffeeTakeoutTables { get; private set; }

        /// <summary>
        /// 选择的咖啡模式外卖台。
        /// </summary>
        public TableInfo SelectedTable { get; set; }

    }
}