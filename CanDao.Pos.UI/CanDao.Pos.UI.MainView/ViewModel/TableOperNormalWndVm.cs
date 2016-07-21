using System;
using System.Threading;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
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

        protected override Tuple<bool, object> CancelOrderComplete(object param)
        {
            var result = (string) param;
            if (!string.IsNullOrEmpty(result))
            {
                var msg = string.Format("账单取消失败“{0}", result);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, OwnerWindow);
                return null;
            }

            InfoLog.Instance.I("取消账单完成。");
            ThreadPool.QueueUserWorkItem(t =>
            {
                InfoLog.Instance.I("广播清台消息给PAD...");
                var errMsg = CommonHelper.BroadcastMessage(EnumBroadcastMsgType.ClearTable, Data.OrderId);
                if (!string.IsNullOrEmpty(errMsg))
                    ErrLog.Instance.E("广播清台指令失败：{0}", (int)EnumBroadcastMsgType.ClearTable);
            });
            NotifyDialog.Notify("取消账单完成。", OwnerWindow.Owner);
            CloseWindow(true);
            return null;
        }

        protected override void DosomethingAfterSettlement()
        {
            CloseWindow(true);//结算完成后关闭窗口。
        }
    }
}