using System;
using System.Threading;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.UI.MainView.Operates;
using CanDao.Pos.UI.MainView.View;

namespace CanDao.Pos.UI.MainView.ViewModel
{
    public class TableOperNormalWndVm : TableOperWndVm
    {
        private DishesTimer _dishesTimer;

        public TableOperNormalWndVm(TableInfo tableInfo)
            : base(tableInfo)
        {
            HasTip = true;
        }

        protected override void GetTableDishInfo(object param)
        {
         
            if (IsInDesignMode)
                return;

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

            //定时检查菜品信息是否一致
            if (OwnerWindow.DialogResult == null)
            {
                _dishesTimer = new DishesTimer();
                _dishesTimer.TableName = _tableInfo.TableName;
                _dishesTimer.DataChangeAction = new Action(GetTableDishInfoAsync);
                _dishesTimer.Start(Data.TotalAmount);
            }
          
        }

        protected override void BackAllDishSuccessProcess()
        {
            GetTableDishInfoAsync();//整单退菜完成后重新获取餐桌明细。
        }

        protected override void OnWindowClosed()
        {
            if (_dishesTimer != null)
            {
                _dishesTimer.stop();
                _dishesTimer = null;
            }
        }

        protected override void DosomethingAfterSettlement()
        {
            CloseWindow(true);//结算完成后关闭窗口。
        }
    }
}