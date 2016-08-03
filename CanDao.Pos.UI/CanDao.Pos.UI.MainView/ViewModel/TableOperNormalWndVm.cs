using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
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
                var openTableWnd = new OpenTableWindow(_tableInfo);
                if (WindowHelper.ShowDialog(openTableWnd, OwnerWindow))
                {
                    _tableInfo.TableStatus = EnumTableStatus.Dinner;//开台成功以后需要修改餐桌状态。

                    var tableFullInfo = new TableFullInfo();
                    tableFullInfo.CloneDataFromTableInfo(_tableInfo);
                    tableFullInfo.CustomerNumber = openTableWnd.CustomerNumber;

                    // 开台成功以后，弹出点菜窗口。
                    WindowHelper.ShowDialog(new OrderDishWindow(tableFullInfo), OwnerWindow);
                }
            }
            GetTableDishInfoAsync();
            //GenerateGetSavedCouponAsync();//暂时屏蔽，有问题。

            //定时检查菜品信息是否一致
            if (OwnerWindow.DialogResult == null)
            {
                _dishesTimer = new DishesTimer();
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

        /// <summary>
        /// 异步获取保存的优惠券信息工作流。
        /// </summary>
        /// <returns></returns>
        protected void GenerateGetSavedCouponAsync()
        {
            TaskService.Start(null, GetSavedCouponProcess, GetSavedCouponComplete, null);
        }

        /// <summary>
        /// 获取保存的优惠券信息执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object GetSavedCouponProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return new Tuple<string, List<UsedCouponInfo>>("创建IOrderService服务失败。", null);

            return service.GetSavedUsedCoupon(Data.OrderId, Globals.UserInfo.UserName);
        }

        /// <summary>
        /// 获取保存的优惠券信息执行完成。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private void GetSavedCouponComplete(object param)
        {
            var result = (Tuple<string, List<UsedCouponInfo>>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取保存的优惠券信息失败：{0}", result.Item1);
                return;
            }

            if (result.Item2 != null && result.Item2.Any())
            {
                Data.UsedCouponInfos.Clear();
                result.Item2.ForEach(Data.UsedCouponInfos.Add);
            }
        }

    }
}