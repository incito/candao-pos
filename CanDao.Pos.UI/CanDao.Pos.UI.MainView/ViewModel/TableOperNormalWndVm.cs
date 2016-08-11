using System;
using System.Windows.Threading;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.UI.MainView.Operates;
using CanDao.Pos.UI.MainView.View;
using CanDao.Pos.UI.Utility;

namespace CanDao.Pos.UI.MainView.ViewModel
{
    public class TableOperNormalWndVm : TableOperWndVm
    {
      

        public TableOperNormalWndVm(TableInfo tableInfo)
            : base(tableInfo)
        {
            HasTip = true;
        }

        protected override void OnWindowLoaded(object param)
        {
            if (IsInDesignMode)
                return;
            _dishesTimer = new DishesTimer();

            if (_tableInfo.TableStatus == EnumTableStatus.Idle)
            {
                var openTableWnd = new OpenTableWindow(_tableInfo);
                if (!WindowHelper.ShowDialog(openTableWnd, OwnerWindow))
                {
                    InfoLog.Instance.I("取消开台，关闭窗口。");
                    CloseWindow(true);
                    return;
                }

                _tableInfo.TableStatus = EnumTableStatus.Dinner;//开台成功以后需要修改餐桌状态。

                var tableFullInfo = new TableFullInfo();
                tableFullInfo.CloneDataFromTableInfo(_tableInfo);
                tableFullInfo.CustomerNumber = openTableWnd.CustomerNumber;

                // 开台成功以后，弹出点菜窗口。
                WindowHelper.ShowDialog(new OrderDishWindow(tableFullInfo), OwnerWindow);
            }
            GetTableDishInfoAsync();
            //GenerateGetSavedCouponAsync();//暂时屏蔽，有问题。

            //定时检查菜品信息是否一致
            if (OwnerWindow.DialogResult == null)
            {
                _dishesTimer.TableName = _tableInfo.TableName;
                _dishesTimer.DataChangeAction = new Action(DataChangeHandel);
                _dishesTimer.Start(Data.TotalAmount);
            }
        }

        /// <summary>
        /// 同步订单信息
        /// </summary>
        private void DataChangeHandel()
        {
            this.OwnerWindow.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(() =>
                {
                    GetTableDishInfoAsync();
                }));

        }

        protected override void BackAllDishSuccessProcess()
        {
            CommonHelper.BroadcastMessageAsync(EnumBroadcastMsgType.SyncOrder, Data.OrderId);
            GetTableDishInfoAsync();//整单退菜完成后重新获取餐桌明细。
        }

        protected override void OnWindowClosed(object param)
        {
            if (_dishesTimer != null)
            {
                _dishesTimer.stop();
                _dishesTimer = null;
            }
        }
    }
}