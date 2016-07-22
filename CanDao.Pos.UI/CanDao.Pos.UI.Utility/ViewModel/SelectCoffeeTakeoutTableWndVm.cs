using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
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
        }

        /// <summary>
        /// 咖啡模式外卖台集合。
        /// </summary>
        public ObservableCollection<TableInfo> CoffeeTakeoutTables { get; private set; }

        /// <summary>
        /// 选择的咖啡模式外卖台。
        /// </summary>
        public TableInfo SelectedTable { get; set; }

        /// <summary>
        /// 分组命令。
        /// </summary>
        public ICommand GroupCmd { get; private set; }

        /// <summary>
        /// 分组命令的执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void GroupTable(object param)
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

        /// <summary>
        /// 分组命令是否可用的判断方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool CanGroupTable(object param)
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

        protected override void InitCommand()
        {
            base.InitCommand();
            GroupCmd = CreateDelegateCommand(GroupTable, CanGroupTable);
        }

        protected override bool CanConfirm(object param)
        {
            return SelectedTable != null;
        }
    }
}