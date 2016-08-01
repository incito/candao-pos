using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.ReportPrint;
using CanDao.Pos.UI.MainView.View;
using CanDao.Pos.UI.Utility;

namespace CanDao.Pos.UI.MainView.ViewModel
{
    /// <summary>
    /// 账单查询窗口的Vm。
    /// </summary>
    public class QueryOrderHistoryWndVm : NormalWindowViewModel
    {
        #region Fields

        private List<QueryOrderInfo> _source;

        #endregion

        #region Constructor

        public QueryOrderHistoryWndVm()
        {
            Orders = new ObservableCollection<QueryOrderInfo>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 查询的订单状态。
        /// </summary>
        private EnumQueryOrderStatus _queryOrderStatus;

        /// <summary>
        /// 查询的订单状态。
        /// </summary>
        public EnumQueryOrderStatus QueryOrderStatus
        {
            get { return _queryOrderStatus; }
            set
            {
                _queryOrderStatus = value;
                RaisePropertiesChanged("QueryOrderStatus");

                FilterOrders();
            }
        }

        /// <summary>
        /// 查询的账单集合。
        /// </summary>
        public ObservableCollection<QueryOrderInfo> Orders { get; private set; }

        /// <summary>
        /// 选中的账单。
        /// </summary>
        public QueryOrderInfo SelectedOrder { get; set; }

        /// <summary>
        /// 过滤账单号
        /// </summary>
        private string _filterOrderId;
        /// <summary>
        /// 过滤账单号
        /// </summary>
        public string FilterOrderId
        {
            get { return _filterOrderId; }
            set
            {
                _filterOrderId = value;
                RaisePropertiesChanged("FilterOrderId");

                FilterOrders();
            }
        }

        /// <summary>
        /// 过滤桌号。
        /// </summary>
        private string _filterTableNo;
        /// <summary>
        /// 过滤桌号。
        /// </summary>
        public string FilterTableNo
        {
            get { return _filterTableNo; }
            set
            {
                _filterTableNo = value;
                RaisePropertiesChanged("FilterTableNo");

                FilterOrders();
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// 查询的账单状态选择命令。
        /// </summary>
        public ICommand OrderStatusCheckCmd { get; private set; }

        /// <summary>
        /// 操作命令集合。
        /// </summary>
        public ICommand OperCmd { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 账单状态选择时执行。
        /// </summary>
        /// <param name="arg"></param>
        private void OrderStatusCheck(object arg)
        {
            switch (arg as string)
            {
                case "All":
                    QueryOrderStatus = EnumQueryOrderStatus.All;
                    break;
                case "Paied":
                    QueryOrderStatus = EnumQueryOrderStatus.Paied;
                    break;
                case "Unpay":
                    QueryOrderStatus = EnumQueryOrderStatus.Unpay;
                    break;
            }
        }

        /// <summary>
        /// 操作命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void Oper(object arg)
        {
            switch (arg as string)
            {
                case "Load":
                case "Refresh":
                    LoadOrderHistoryAsync();
                    break;
                case "ReprintPayBill":
                    ReportPrintHelper.PrintSettlementReport(SelectedOrder.OrderId, Globals.UserInfo.UserName);
                    break;
                case "ReprintClearn":
                    ReportPrintHelper.PrintClearPosReport(Globals.UserInfo.UserName);
                    break;
                case "PreGroup":
                    ((QueryOrderHistoryWindow)OwnerWindow).GsOrderList.PreviousGroup();
                    break;
                case "NextGroup":
                    ((QueryOrderHistoryWindow)OwnerWindow).GsOrderList.NextGroup();
                    break;
                case "PayBill":
                    PayBill();
                    break;
                case "AntiPayBill":
                    AntiPayBill();
                    break;
            }
        }

        /// <summary>
        /// 操作命令是否可用的判断方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanOper(object arg)
        {
            var enable = true;
            switch (arg as string)
            {
                case "ReprintPayBill":
                case "AntiPayBill":
                    enable = SelectedOrder != null && SelectedOrder.HasBeenPaied;
                    break;
                case "PayBill":
                    enable = SelectedOrder != null && !SelectedOrder.HasBeenPaied;
                    break;
                case "PreGroup":
                    enable = ((QueryOrderHistoryWindow)OwnerWindow).GsOrderList.CanPreviousGroup;
                    break;
                case "NextGroup":
                    enable = ((QueryOrderHistoryWindow)OwnerWindow).GsOrderList.CanNextGruop;
                    break;
            }
            return enable;
        }

        #endregion

        #region Protected  Methods

        protected override void InitCommand()
        {
            base.InitCommand();
            OrderStatusCheckCmd = CreateDelegateCommand(OrderStatusCheck);
            OperCmd = CreateDelegateCommand(Oper, CanOper);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 根据账单信息组合餐台信息，进行结算或反结算。
        /// </summary>
        /// <param name="orderInfo">账单信息。</param>
        /// <returns></returns>
        private TableInfo GenerateTableInfo(QueryOrderInfo orderInfo)
        {
            var item = new TableInfo
            {
                TableName = orderInfo.TableName,
                TableId = orderInfo.TableId,
                TableStatus = orderInfo.HasBeenPaied ? EnumTableStatus.Idle : EnumTableStatus.Dinner,
                OrderId = orderInfo.OrderId
            };
            return item;
        }

        /// <summary>
        /// 结算账单。
        /// </summary>
        private void PayBill()
        {
            if (!Globals.UserRight.AllowCash)
            {
                MessageDialog.Warning("您没有收银权限！");
                return;
            }

            if (!MessageDialog.Quest(string.Format("订单号：\"{0}\"确定结算吗？", SelectedOrder.OrderId), OwnerWindow))
                return;

            var item = GenerateTableInfo(SelectedOrder);
            if (WindowHelper.ShowDialog(new TableOperWindow(item), OwnerWindow))
                LoadOrderHistoryAsync();
        }

        /// <summary>
        /// 反结算账单。
        /// </summary>
        private void AntiPayBill()
        {
            if (!Globals.UserRight.AllowAntiSettlement)
            {
                MessageDialog.Warning("您没有反结算权限。");
                return;
            }

            if (!MessageDialog.Quest(string.Format("订单号： \"{0}\" 确定反结算吗？", SelectedOrder.OrderId), OwnerWindow))
                return;

            var helper = new AntiSettlementHelper();
            var afterAntiSettlementWf = new WorkFlowInfo(null, AfterAntiSettlement);
            helper.AntiSettlementAsync(SelectedOrder.OrderId, SelectedOrder.MemberNo, OwnerWindow, afterAntiSettlementWf);
        }

        /// <summary>
        /// 异步加载历史账单。
        /// </summary>
        private void LoadOrderHistoryAsync()
        {
            TaskService.Start(null, LoadOrderInfosProcess, LoadOrderInfoComplete, "获取历史账单中...");
        }

        /// <summary>
        /// 加载账单信息的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object LoadOrderInfosProcess(object arg)
        {
            InfoLog.Instance.I("开始获取账单数据...");
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, List<QueryOrderInfo>>("创建IRestaurantService服务失败。", null);

            return service.QueryOrderInfos(Globals.UserInfo.UserName);
        }

        /// <summary>
        /// 加载账单执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private void LoadOrderInfoComplete(object arg)
        {
            var result = (Tuple<string, List<QueryOrderInfo>>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E(result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            InfoLog.Instance.I("账单查询完成。");
            _source = result.Item2;
            FilterOrders();
        }

        private Tuple<bool, object> AfterAntiSettlement(object param)
        {
            NotifyDialog.Notify(string.Format("订单号：\"{0}\"反结算成功。", SelectedOrder.OrderId), OwnerWindow);
            var item = GenerateTableInfo(SelectedOrder);
            item.TableStatus = EnumTableStatus.Dinner;//反结算成功以后将餐台状态设置成就餐，避免进入结账页面弹出开台窗口。
            InfoLog.Instance.I("弹出结账窗口...");
            if (WindowHelper.ShowDialog(new TableOperWindow(item), OwnerWindow))
                LoadOrderHistoryAsync();
            return null;
        }

        /// <summary>
        /// 过滤订单。
        /// </summary>
        private void FilterOrders()
        {
            if (_source == null)
                return;

            Orders.Clear();
            IEnumerable<QueryOrderInfo> temp = _source;
            if (QueryOrderStatus == EnumQueryOrderStatus.Paied)
                temp = _source.Where(t => t.HasBeenPaied);
            else if (QueryOrderStatus == EnumQueryOrderStatus.Unpay)
                temp = _source.Where(t => !t.HasBeenPaied);

            if (!string.IsNullOrEmpty(FilterOrderId))
                temp = temp.Where(t => t.OrderId.Contains(FilterOrderId));

            if (!string.IsNullOrEmpty(FilterTableNo))
                temp = temp.Where(t => t.TableName.Contains(FilterTableNo));

            temp.ToList().ForEach(Orders.Add);
        }

        #endregion
    }
}