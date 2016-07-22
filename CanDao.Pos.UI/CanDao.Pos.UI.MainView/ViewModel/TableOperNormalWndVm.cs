using System;
using System.Threading;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.UI.MainView.View;

namespace CanDao.Pos.UI.MainView.ViewModel
{
    public class TableOperNormalWndVm : TableOperWndVm
    {
        public TableOperNormalWndVm(TableInfo tableInfo)
            : base(tableInfo)
        {
            HasTip = true;
        }

        protected override void GetTableDishInfo(object param)
        {
            if (IsInDesignMode)
                return;

            Data = new TableFullInfo { TableName = _tableInfo.TableName };//为了在左上角显示出桌台名称。

            if (_tableInfo.TableStatus == EnumTableStatus.Idle)
            {
                if (WindowHelper.ShowDialog(new OpenTableWindow(_tableInfo), OwnerWindow))
                {
                    _tableInfo.TableStatus = EnumTableStatus.Dinner;//开台成功以后需要修改餐桌状态。
                    // 开台成功以后，弹出点菜窗口。
                    WindowHelper.ShowDialog(new OrderDishWindow(_tableInfo), OwnerWindow);
                }
            }
            GetTableDishInfoAsync();
        }

        protected override void BackAllDishSuccessProcess()
        {
            GetTableDishInfoAsync();//整单退菜完成后重新获取餐桌明细。
        }

        protected override void DosomethingAfterSettlement()
        {
            CloseWindow(true);//结算完成后关闭窗口。
        }
    }
}