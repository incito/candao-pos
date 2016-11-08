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

        #endregion

        #region Protected  Methods

        protected override void InitCommand()
        {
            base.InitCommand();
            OrderStatusCheckCmd = CreateDelegateCommand(OrderStatusCheck);
        }

        protected override void OperMethod(object param)
        {
            var print = new ReportPrintHelper2(OwnerWindow);
            switch (param as string)
            {
                case "Load":
                case "Refresh":
                    LoadOrderHistoryAsync();
                    break;
                case "ReprintPayBill":
                    print.PrintSettlementReport(SelectedOrder.OrderId, Globals.UserInfo.UserName);
                    break;
                case "ReprintTransactionSlip":
                    print.PrintMemberPayBillReport(SelectedOrder.OrderId, Globals.UserInfo.UserName);
                    break;
                case "ReprintClearn":
                    print.PrintClearPosReport(Globals.UserInfo.UserName);
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

        protected override bool CanOperMethod(object param)
        {
            switch (param as string)
            {
                case "ReprintPayBill":
                case "AntiPayBill":
                    return SelectedOrder != null && SelectedOrder.OrderStatus == EnumOrderStatus.InternalSettle;
                case "ReprintTransactionSlip":
                    return SelectedOrder != null && SelectedOrder.OrderStatus == EnumOrderStatus.InternalSettle && !string.IsNullOrEmpty(SelectedOrder.MemberNo);
                case "PayBill":
                    return SelectedOrder != null && SelectedOrder.OrderStatus == EnumOrderStatus.Ordered;
                case "PreGroup":
                    return ((QueryOrderHistoryWindow)OwnerWindow).GsOrderList.CanPreviousGroup;
                case "NextGroup":
                    return ((QueryOrderHistoryWindow)OwnerWindow).GsOrderList.CanNextGruop;
                default:
                    return true;
            }
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
                TableType = orderInfo.TableType,
                TableStatus = orderInfo.OrderStatus == EnumOrderStatus.InternalSettle ? EnumTableStatus.Idle : EnumTableStatus.Dinner,
                OrderId = orderInfo.OrderId,
                TableNo = orderInfo.TableName,
            };
            item.IsHangOrder = item.IsTakeoutTable;
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

            if (!MessageDialog.Quest(string.Format("订单号： \"{0}\"确定结算吗？", SelectedOrder.OrderId), OwnerWindow))
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
            if (!MessageDialog.Quest(string.Format("订单号： \"{0}\" 确定反结算吗？", SelectedOrder.OrderId), OwnerWindow))
                return;

            var helper = new AntiSettlementHelper();
            var afterAntiSettlementWf = new WorkFlowInfo(null, AfterAntiSettlement);
            helper.AntiSettlementAsync(SelectedOrder.OrderId, SelectedOrder.MemberNo, afterAntiSettlementWf);
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
            WindowHelper.ShowDialog(new TableOperWindow(item), OwnerWindow);
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
                temp = _source.Where(t => t.OrderStatus == EnumOrderStatus.InternalSettle);
            else if (QueryOrderStatus == EnumQueryOrderStatus.Unpay)
                temp = _source.Where(t => t.OrderStatus == EnumOrderStatus.Ordered);

            if (!string.IsNullOrEmpty(FilterOrderId))
                temp = temp.Where(t => t.OrderId.ToUpper().Contains(FilterOrderId.ToUpper()));

            if (!string.IsNullOrEmpty(FilterTableNo))
                temp = temp.Where(t => t.TableName.ToUpper().Contains(FilterTableNo.ToUpper()));

            temp.ToList().ForEach(Orders.Add);
        }

        #endregion
    }
}