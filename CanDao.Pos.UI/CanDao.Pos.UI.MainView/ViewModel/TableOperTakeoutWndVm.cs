using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.UI.MainView.View;

namespace CanDao.Pos.UI.MainView.ViewModel
{
    public class TableOperTakeoutWndVm : TableOperWndVm
    {
        /// <summary>
        /// 用语外卖台窗口退出时同步。
        /// </summary>
        private readonly EventWaitHandle _eventWait = new AutoResetEvent(false);

        /// <summary>
        /// 关闭窗口时处理的超时时间（秒），默认30秒。
        /// </summary>
        private const int ClosingWaitTimeoutSecond = 15;

        /// <summary>
        /// 窗口关闭时取消事件参数。
        /// </summary>
        private CancelEventArgs _cancelArgs;

        public TableOperTakeoutWndVm(string tableName)
            : base(GenerateTableInfo(tableName))
        {
            HasTip = false;
        }

        protected override void GetTableDishInfo(object param)
        {
            if (IsInDesignMode)
                return;

            if (Data == null || string.IsNullOrEmpty(Data.OrderId))
                TaskService.Start(null, OpenTableProcess, OpenTableComplete, "外卖台开台中...");

            if (OwnerWindow != null)
            {
                OwnerWindow.Closing -= OwnerWindowOnClosing;//解决多次触发GetTableDishInfo方法导致事件重复添加。
                OwnerWindow.Closing += OwnerWindowOnClosing;
            }
        }

        private void OwnerWindowOnClosing(object sender, CancelEventArgs e)
        {
            if (OwnerWindow.DialogResult == true)//为true表示正常关闭，不做后续处理。
                return;

            e.Cancel = true;
            _cancelArgs = e;
            InfoLog.Instance.I("外卖台结账页面退出，进行退菜和取消账单处理...");
            var curWf = new WorkFlowInfo(CancelOrderProcess, CancelOrderComplete, "取消账单中...");
            if (Data.DishInfos.Any())
            {
                if (!MessageDialog.Quest("退出将清空当前已选菜品，确定放弃结算？"))
                    return;

                InfoLog.Instance.I("外卖台有已点菜品，进行整桌退菜...");
                var backDishWf = GenerateBackAllDishWf();
                backDishWf.NextWorkFlowInfo = curWf;
                curWf = backDishWf;
            }

            WorkFlowService.Start(null, curWf);
            _eventWait.WaitOne(ClosingWaitTimeoutSecond * 1000);
        }

        private object OpenTableProcess(object param)
        {
            var request = new OpenTableRequest
            {
                tableNo = _tableInfo.TableName,
                womanNum = 0,
                manNum = 0,
                username = Globals.UserInfo.UserName,
                ageperiod = "",
            };
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, string>("创建IRestaurantService服务失败。", null);

            return service.OpenTable(request);
        }

        private void OpenTableComplete(object param)
        {
            var result = (Tuple<string, string>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("外卖开台失败：{0}", result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                CloseWindow(false);
                return;
            }

            InfoLog.Instance.I("外卖开台完成，订单号：{0}", result.Item2);
            _tableInfo.OrderId = result.Item2;
            if (Data == null)
                Data = new TableFullInfo();
            Data.TableName = _tableInfo.TableName;
            Data.OrderId = _tableInfo.OrderId;

            WindowHelper.ShowDialog(new OrderDishWindow(_tableInfo), OwnerWindow);
            GetTableDishInfoAsync();
        }

        protected override Tuple<bool, object> CancelOrderComplete(object param)
        {
            var result = (string)param;
            if (!string.IsNullOrEmpty(result))
            {
                var msg = string.Format("取消外卖台账单失败：{0}", result);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, OwnerWindow);
                _eventWait.Set();
                return null;
            }

            InfoLog.Instance.I("外卖台取消账单成功。");
            if (_cancelArgs != null)
                _cancelArgs.Cancel = false;//取消阻止关闭，即关闭窗口。

            _eventWait.Set();
            return null;
        }

        protected override void DosomethingAfterSettlement()
        {
            Data.OrderId = null;//设定订单id为空以后，调用GetTableDishInfo命令执行方法时就会自动开台。
            Data.DishInfos.Clear();//清空左侧外卖菜品列表。
            GetTableDishInfoCmd.Execute(null);
        }

        /// <summary>
        /// 根据餐桌名生成餐桌信息。
        /// </summary>
        /// <param name="tableName">餐桌名。</param>
        /// <returns></returns>
        private static TableInfo GenerateTableInfo(string tableName)
        {
            return new TableInfo()
            {
                TableName = tableName,
                TableNo = tableName,
                TableStatus = EnumTableStatus.Idle,
            };
        }
    }
}