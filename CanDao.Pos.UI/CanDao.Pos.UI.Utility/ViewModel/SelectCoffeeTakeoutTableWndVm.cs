using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 咖啡外卖桌台选择窗口Vm。
    /// </summary>
    public class SelectCoffeeTakeoutTableWndVm : NormalWindowViewModel
    {

        public SelectCoffeeTakeoutTableWndVm(List<TableInfo> tableInfos)
        {
            CoffeeTakeoutTables = new ObservableCollection<TableInfo>();
            if (tableInfos != null)
                tableInfos.ForEach(CoffeeTakeoutTables.Add);
            SelectedTable = CoffeeTakeoutTables.FirstOrDefault();
        }

        /// <summary>
        /// 咖啡模式外卖台集合。
        /// </summary>
        public ObservableCollection<TableInfo> CoffeeTakeoutTables { get; private set; }

        /// <summary>
        /// 选择的咖啡模式外卖台。
        /// </summary>
        public TableInfo SelectedTable { get; set; }

        protected override bool CanConfirm(object param)
        {
            return SelectedTable != null;
        }

        protected override void GroupMethod(object param)
        {
            switch ((string)param)
            {
                case "PreGroup":
                    ((SelectCoffeeTakeoutTableWindow)OwnerWindow).GsCfTakeout.PreviousGroup();
                    break;
                case "NextGroup":
                    ((SelectCoffeeTakeoutTableWindow)OwnerWindow).GsCfTakeout.NextGroup();
                    break;
            }
        }

        protected override bool CanGroupMethod(object param)
        {
            switch ((string)param)
            {
                case "PreGroup":
                    return ((SelectCoffeeTakeoutTableWindow)OwnerWindow).GsCfTakeout.CanPreviousGroup;
                case "NextGroup":
                    return ((SelectCoffeeTakeoutTableWindow)OwnerWindow).GsCfTakeout.CanNextGruop;
                default:
                    return true;
            }
        }
    }
}