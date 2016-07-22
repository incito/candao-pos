﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.ReportPrint;
using CanDao.Pos.UI.MainView.View;
using CanDao.Pos.UI.Utility;
using CanDao.Pos.UI.Utility.View;
using CanDao.Pos.UI.Utility.ViewModel;
using Timer = System.Timers.Timer;

namespace CanDao.Pos.UI.MainView.ViewModel
{
    /// <summary>
    /// 餐台操作窗口Vm。
    /// </summary>
    public class TableOperWndVm : NormalWindowViewModel
    {
        #region Field

        /// <summary>
        /// 当前餐桌信息。
        /// </summary>
        protected readonly TableInfo _tableInfo;

        /// <summary>
        /// 反结原因。
        /// </summary>
        private string _antiSettlementReason;

        /// <summary>
        /// 当前零头处理方式。
        /// </summary>
        private EnumOddModel _curOddModel;

        /// <summary>
        /// 当前选中的优惠券。
        /// </summary>
        private CouponInfo _curSelectedCouponInfo;

        /// <summary>
        /// 是否是用户输入现金金额。
        /// </summary>
        private bool _isUserInputCash;

        /// <summary>
        /// 优惠券长按定时器。
        /// </summary>
        private Timer _couponLongPressTimer;

        /// <summary>
        /// 长按定时器的触发间隔（1秒）
        /// </summary>
        private const int LongPressTimerSecond = 1;

        /// <summary>
        /// 是否是长按优惠券处理模式，当是该模式时，优惠券弹起事件里不处理。
        /// </summary>
        private bool _isLongPressModel;

        #endregion

        #region Constructor

        public TableOperWndVm(TableInfo tableInfo)
        {
            _tableInfo = tableInfo;
            InitCouponCategories();
            InitCouponLongPressTimer();
            SelectedBankInfo = Globals.BankInfos != null ? Globals.BankInfos.FirstOrDefault(t => t.Id == 0) : null;
            _curOddModel = Globals.OddModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 餐台信息全。
        /// </summary>
        private TableFullInfo _data;
        /// <summary>
        /// 餐台信息全。
        /// </summary>
        public TableFullInfo Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertiesChanged("Data");
            }
        }

        /// <summary>
        /// 当前分类集合。
        /// </summary>
        public ObservableCollection<CouponCategory> CouponCategories { get; private set; }

        /// <summary>
        /// 当前优惠券分类下的优惠券集合。
        /// </summary>
        public ObservableCollection<CouponInfo> CouponInfos { get; private set; }

        /// <summary>
        /// 选择的优惠分类。
        /// </summary>
        private CouponCategory _selectedCouponCategory;
        /// <summary>
        /// 选择的优惠分类。
        /// </summary>
        public CouponCategory SelectedCouponCategory
        {
            get { return _selectedCouponCategory; }
            set
            {
                _selectedCouponCategory = value;
                RaisePropertiesChanged("SelectedCouponCategory");

                CouponInfos.Clear();
                InfoLog.Instance.I("开始获取\"{0}\"优惠券...", value.CategoryName);
                TaskService.Start(value.CategoryType, GetCouponCategoriesProcess, GetCouponCategoriesComplete);

                //不缓存优惠券，每次都重新获取。
                //if (value.CouponInfos == null || !value.CouponInfos.Any())
                //{
                //    InfoLog.Instance.I("开始获取\"{0}\"优惠券...", value.CategoryName);
                //    TaskService.Start(value.CategoryType, GetCouponCategoriesProcess, GetCouponCategoriesComplete);
                //}
                //else
                //{
                //    value.CouponInfos.ForEach(CouponInfos.Add);
                //}
            }
        }

        /// <summary>
        /// 当前选择的订单菜品。
        /// </summary>
        private OrderDishInfo _selectedOrderDish;
        /// <summary>
        /// 当前选择的订单菜品。
        /// </summary>
        public OrderDishInfo SelectedOrderDish
        {
            get { return _selectedOrderDish; }
            set
            {
                _selectedOrderDish = value;
                RaisePropertiesChanged("SelectedOrderDish");
            }
        }

        /// <summary>
        /// 选中的银行。
        /// </summary>
        private BankInfo _selectedBankInfo;
        /// <summary>
        /// 选中的银行。
        /// </summary>
        public BankInfo SelectedBankInfo
        {
            get { return _selectedBankInfo; }
            set
            {
                _selectedBankInfo = value;
                RaisePropertiesChanged("SelectedBankInfo");
            }
        }

        /// <summary>
        /// 选择的挂账单位。
        /// </summary>
        private OnCompanyAccountInfo _selectedOnCmpAccInfo;
        /// <summary>
        /// 获取或设置选择的挂账单位。
        /// </summary>
        public OnCompanyAccountInfo SelectedOnCmpAccInfo
        {
            get { return _selectedOnCmpAccInfo; }
            set
            {
                _selectedOnCmpAccInfo = value;
                RaisePropertiesChanged("SelectedOnCmpAccInfo");
            }
        }

        /// <summary>
        /// 当前选中的使用优惠券。
        /// </summary>
        private UsedCouponInfo _selectedUsedCouponInfo;
        /// <summary>
        /// 当前选中的使用优惠券。
        /// </summary>
        public UsedCouponInfo SelectedUsedCouponInfo
        {
            get { return _selectedUsedCouponInfo; }
            set
            {
                _selectedUsedCouponInfo = value;
                RaisePropertiesChanged("SelectedUsedCouponInfo");
            }
        }

        /// <summary>
        /// 结账明细信息。
        /// </summary>
        private string _settlementInfo;

        /// <summary>
        /// 结账明细信息。
        /// </summary>
        public string SettlementInfo
        {
            get { return _settlementInfo; }
            set
            {
                _settlementInfo = value;
                RaisePropertiesChanged("SettlementInfo");
            }
        }

        /// <summary>
        /// 现金支付。
        /// </summary>
        private decimal _cashAmount;
        /// <summary>
        /// 现金支付。
        /// </summary>
        public decimal CashAmount
        {
            get { return _cashAmount; }
            set
            {
                if (_cashAmount == value)
                    return;

                _cashAmount = value;
                RaisePropertiesChanged("CashAmount");

                GenerateSettlementInfo();
            }
        }

        /// <summary>
        /// 找零金额。
        /// </summary>
        private decimal _chargeAmount;
        /// <summary>
        /// 找零金额。
        /// </summary>
        public decimal ChargeAmount
        {
            get { return _chargeAmount; }
            set
            {
                _chargeAmount = value;
                RaisePropertiesChanged("ChargeAmount");
            }
        }

        /// <summary>
        /// 银行刷卡金额。
        /// </summary>
        private decimal _bankAmount;
        /// <summary>
        /// 银行刷卡金额。
        /// </summary>
        public decimal BankAmount
        {
            get { return _bankAmount; }
            set
            {
                _bankAmount = value;
                RaisePropertiesChanged("BankAmount");

                GenerateSettlementInfo();
            }
        }

        /// <summary>
        /// 会员刷卡金额。
        /// </summary>
        private decimal _memberAmount;
        /// <summary>
        /// 会员刷卡金额。
        /// </summary>
        public decimal MemberAmount
        {
            get { return _memberAmount; }
            set
            {
                _memberAmount = value;
                RaisePropertiesChanged("MemberAmount");

                GenerateSettlementInfo();
            }
        }

        /// <summary>
        /// 会员积分消费。
        /// </summary>
        private decimal _integralAmount;

        /// <summary>
        /// 会员积分消费。
        /// </summary>
        public decimal IntegralAmount
        {
            get { return _integralAmount; }
            set
            {
                _integralAmount = value;
                RaisePropertiesChanged("IntegralAmount");

                GenerateSettlementInfo();
            }
        }

        /// <summary>
        /// 挂账金额。
        /// </summary>
        private decimal _debitAmount;

        /// <summary>
        /// 挂账金额。
        /// </summary>
        public decimal DebitAmount
        {
            get { return _debitAmount; }
            set
            {
                _debitAmount = value;
                RaisePropertiesChanged("DebitAmount");

                GenerateSettlementInfo();
            }
        }

        /// <summary>
        /// 支付宝金额。
        /// </summary>
        private decimal _alipayAmount;

        /// <summary>
        /// 支付宝金额。
        /// </summary>
        public decimal AlipayAmount
        {
            get { return _alipayAmount; }
            set
            {
                _alipayAmount = value;
                RaisePropertiesChanged("AlipayAmount");

                GenerateSettlementInfo();
            }
        }

        /// <summary>
        /// 微信支付金额。
        /// </summary>
        private decimal _wechatAmount;

        /// <summary>
        /// 微信支付金额。
        /// </summary>
        public decimal WechatAmount
        {
            get { return _wechatAmount; }
            set
            {
                _wechatAmount = value;
                RaisePropertiesChanged("WechatAmount");

                GenerateSettlementInfo();
            }
        }

        /// <summary>
        /// 四舍五入金额。
        /// </summary>
        public decimal RoundingAmount { get; set; }

        /// <summary>
        /// 抹零金额。
        /// </summary>
        public decimal WipeOddAmount { get; set; }

        /// <summary>
        /// 银行卡号码。
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        /// 会员卡号码。
        /// </summary>
        private string _memberCardNo;
        /// <summary>
        /// 会员卡号码。
        /// </summary>
        public string MemberCardNo
        {
            get { return _memberCardNo; }
            set
            {
                _memberCardNo = value;
                RaisePropertyChanged("MemberCardNo");
            }
        }

        /// <summary>
        /// 会员密码。
        /// </summary>
        private string _memberPassword;
        /// <summary>
        /// 会员密码。
        /// </summary>
        public string MemberPassword
        {
            get { return _memberPassword; }
            set
            {
                _memberPassword = value;
                RaisePropertiesChanged("MemberPassword");
            }
        }

        /// <summary>
        /// 支付宝账号。
        /// </summary>
        public string AlipayNo { get; set; }

        /// <summary>
        /// 微信账号。
        /// </summary>
        public string WechatNo { get; set; }

        /// <summary>
        /// 是否更多打印打开着。
        /// </summary>
        private bool _isPrintMoreOpened;
        /// <summary>
        /// 是否更多打印打开着。
        /// </summary>
        public bool IsPrintMoreOpened
        {
            get { return _isPrintMoreOpened; }
            set
            {
                _isPrintMoreOpened = value;
                RaisePropertiesChanged("IsPrintMoreOpened");
            }
        }

        /// <summary>
        /// 是否会员已登录。
        /// </summary>
        private bool _isMemberLogin;
        /// <summary>
        /// 是否会员已登录。
        /// </summary>
        public bool IsMemberLogin
        {
            get { return _isMemberLogin; }
            set
            {
                _isMemberLogin = value;
                RaisePropertiesChanged("IsMemberLogin");
            }
        }

        /// <summary>
        /// 是否有小费。（外卖不支持小费）
        /// </summary>
        protected bool HasTip { get; set; }

        /// <summary>
        /// 经过计算后收到的小费金额。
        /// </summary>
        protected decimal TipPaymentAmount { get; set; }

        #region 优惠券分组

        /// <summary>
        /// 每一组优惠券个数。
        /// </summary>
        private int _eachGroupCouponCount;
        /// <summary>
        /// 每一组优惠券个数。
        /// </summary>
        public int EachGroupCouponCount
        {
            get { return _eachGroupCouponCount; }
            set
            {
                if (_eachGroupCouponCount == value)
                    return;

                _eachGroupCouponCount = value;
                PacketCoupon();
            }
        }

        #endregion

        #endregion

        #region Command

        /// <summary>
        /// 获取餐桌菜单明细命令。
        /// </summary>
        public ICommand GetTableDishInfoCmd { get; private set; }

        /// <summary>
        /// 菜单列表操作命令。
        /// </summary>
        public ICommand DataGridPageOperCmd { get; private set; }

        /// <summary>
        /// 打印命令。
        /// </summary>
        public ICommand PrintCmd { get; private set; }

        /// <summary>
        /// 其他操作命令。
        /// </summary>
        public ICommand OperCmd { get; private set; }

        /// <summary>
        /// 优惠券按下时命令。
        /// </summary>
        public ICommand CouponMouseDownCmd { get; private set; }

        /// <summary>
        /// 优惠券弹起时命令。
        /// </summary>
        public ICommand CouponMouseUpCmd { get; private set; }

        /// <summary>
        /// 现金输入控件获取或失去焦点命令。
        /// </summary>
        public ICommand CashControlFocusCmd { get; private set; }

        /// <summary>
        /// 回车支付命令。
        /// </summary>
        public ICommand EnterPayCmd { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 获取餐桌菜单明细命令执行方法。
        /// </summary>
        /// <param name="param"></param>
        protected virtual void GetTableDishInfo(object param)
        {
        }

        /// <summary>
        /// 菜单列表操作命令执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void DataGridPageOper(object param)
        {
            TableOperWindow wnd = (TableOperWindow)OwnerWindow;
            switch ((string)param)
            {
                case "DishPreGroup":
                    wnd.DishGroupSelector.PreviousGroup();
                    break;
                case "DishNextGroup":
                    wnd.DishGroupSelector.NextGroup();
                    break;
                case "CouponPreGroup":
                    wnd.CouponGroupSelector.PreviousGroup();
                    break;
                case "CouponNextGroup":
                    wnd.CouponGroupSelector.NextGroup();
                    break;
                case "CouponListPreGroup":
                    wnd.GsCouponList.PreviousGroup();
                    break;
                case "CouponListNextGroup":
                    wnd.GsCouponList.NextGroup();
                    break;
            }
        }

        private bool CanDataGridPageOper(object param)
        {
            TableOperWindow wnd = (TableOperWindow)OwnerWindow;
            switch ((string)param)
            {
                case "DishPreGroup":
                    return wnd.DishGroupSelector.CanPreviousGroup;
                case "DishNextGroup":
                    return wnd.DishGroupSelector.CanNextGruop;
                case "CouponPreGroup":
                    return wnd.CouponGroupSelector.CanPreviousGroup;
                case "CouponNextGroup":
                    return wnd.CouponGroupSelector.CanNextGruop;
                case "CouponListPreGroup":
                    return wnd.GsCouponList.CanPreviousGroup;
                case "CouponListNextGroup":
                    return wnd.GsCouponList.CanNextGruop;
                default:
                    return true;
            }
        }

        /// <summary>
        /// 打印命令执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void Print(object param)
        {
            switch (param as string)
            {
                case "PreSettlement":
                    IsPrintMoreOpened = false;
                    GetTableDishInfoAsync();
                    ReportPrintHelper.PrintPresettlementReport(Data, Globals.UserInfo.UserName);
                    break;
                case "ReprintBill":
                    IsPrintMoreOpened = false;
                    if (MessageDialog.Quest(string.Format("确定要重印餐台\"{0}\"的结账单吗？", Data.TableName)))
                    {
                        InfoLog.Instance.I("开始重印餐台\"{0}\"的结账单...", Data.TableName);
                        ReportPrintHelper.PrintSettlementReport(Data.OrderId, Globals.UserInfo.UserName);
                        InfoLog.Instance.I("结束重印餐台\"{0}\"的结账单。", Data.TableName);
                    }
                    break;
                case "ReprintCustomUseBill":
                    IsPrintMoreOpened = false;
                    if (MessageDialog.Quest(string.Format("确定要重印餐台\"{0}\"的客用单吗？", Data.TableName)))
                    {
                        InfoLog.Instance.I("开始重印餐台\"{0}\"的客用单...", Data.TableName);
                        ReportPrintHelper.PrintCustomUseBillReport(Data.OrderId, Globals.UserInfo.UserName);
                        InfoLog.Instance.I("结束重印餐台\"{0}\"的客用单。", Data.TableName);
                    }
                    break;
                case "PrintTransactionSlip":
                    IsPrintMoreOpened = false;
                    ReportPrintHelper.PrintMemberPayBillReport(Data.OrderId, Globals.UserInfo.UserName);
                    break;
            }
        }

        /// <summary>
        /// 打印命令是否可用的判断方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool CanPrint(object param)
        {
            bool enable = true;
            switch (param as string)
            {
                case "PreSettlement":
                    enable = Data != null && !Data.HasBeenPaied && Data.DishInfos.Any();
                    break;
                case "ReprintBill":
                case "PrintTransactionSlip":
                    enable = Data != null && Data.HasBeenPaied;
                    break;
                case "ReprintUserBill":
                    break;
            }
            return enable;
        }

        /// <summary>
        /// 其他操作命令执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void Oper(object param)
        {
            switch (param as string)
            {
                case "More":
                    IsPrintMoreOpened = !IsPrintMoreOpened;
                    break;
                case "CancelOrder":
                    CancelOrderAsync();
                    break;
                case "Resettlement":
                    ResettlementSync();
                    break;
                case "Order":
                    if (WindowHelper.ShowDialog(new OrderDishWindow(Data), OwnerWindow))
                        GetTableDishInfoAsync();
                    break;
                case "OpenCashBox":
                    OpenCashBoxAsync();
                    break;
                case "KeepOdd":
                    _curOddModel = EnumOddModel.None;
                    CalculatePaymentAmount();
                    break;
                case "DishCountIncrease":
                    AddDish();
                    break;
                case "DishCountReduce":
                    BackDish();
                    break;
                case "SelectBank":
                    var wnd = new SelectBankWindow(SelectedBankInfo);
                    if (WindowHelper.ShowDialog(wnd, OwnerWindow))
                        SelectedBankInfo = ((SelectBankWndVm)wnd.DataContext).SelectedBank;
                    break;
                case "SelectOnAccountCompany":
                    InfoLog.Instance.I("开始获取挂账单位...");
                    var companyWnd = new OnAccountCompanySelectWindow();
                    if (WindowHelper.ShowDialog(companyWnd, OwnerWindow))
                        SelectedOnCmpAccInfo = companyWnd.SelectedCompany;
                    break;
                case "MemberLogin":
                    InfoLog.Instance.I("会员登入按钮点击...");
                    var loginWf = GenerateMemberLoginWf();
                    if (loginWf != null)
                        WorkFlowService.Start(MemberCardNo, loginWf);
                    break;
                case "MemberLogout":
                    InfoLog.Instance.I("会员登出按钮点击...");
                    var logoutWf = GenerateMemberLogoutWf();
                    if (logoutWf != null)
                        WorkFlowService.Start(MemberCardNo, logoutWf);
                    break;
                case "CouponRemove":
                    InfoLog.Instance.I("移除优惠券：{0}。", SelectedUsedCouponInfo.Name);
                    RemoveUsedCouponInfo(SelectedUsedCouponInfo);
                    break;
                case "CouponClear":
                    InfoLog.Instance.I("清除优惠券。");
                    ClearUsedCouponInfo();
                    break;
                case "ClearTable":
                    ClearCoffeeTable();
                    break;
                case "BackAllDish":
                    var wf = GenerateBackAllDishWf();
                    WorkFlowService.Start(null, wf);
                    break;
            }
        }

        /// <summary>
        /// 是否操作命令可用的执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool CanOper(object param)
        {
            switch (param as string)
            {
                case "CancelOrder":
                    return Data != null && !Data.HasBeenPaied;
                case "Resettlement":
                    return Data != null && Data.HasBeenPaied;
                case "Order":
                case "KeepOdd":
                case "DishCountIncrease":
                case "DishCountReduce":
                case "PayBill":
                    return Data != null && !Data.HasBeenPaied;
                case "MemberLogin":
                    return !string.IsNullOrEmpty(MemberCardNo) && MemberCardNo.Length >= 11;
                case "CouponRemove":
                    return SelectedUsedCouponInfo != null;
                case "CouponClear":
                    return Data != null && Data.UsedCouponInfos.Any();
                case "BackAllDish":
                    return Data != null && Data.OrderStatus == EnumOrderStatus.Ordered && Data.DishInfos.Any();
                default:
                    return true;
            }
        }

        /// <summary>
        /// 现金控件获取或失去焦点命令时执行。
        /// </summary>
        /// <param name="param"></param>
        private void CashControlFocus(object param)
        {
            _isUserInputCash = Convert.ToBoolean(param);
        }

        private void CouponMouseDown(object arg)
        {
            var coupon = (CouponInfo)((CouponInfo)arg).Clone();
            _curSelectedCouponInfo = coupon;

            _isLongPressModel = false;
            _couponLongPressTimer.Start();
        }

        private void CouponMouseUp(object arg)
        {
            _couponLongPressTimer.Stop();
            if (_isLongPressModel)
                return;

            if (Data.TotalAmount <= 0)
            {
                MessageDialog.Warning("还未下单，不能使用优惠。", OwnerWindow);
                return;
            }

            if (_curSelectedCouponInfo == null)
                return;

            if (_curSelectedCouponInfo.CouponType == EnumCouponType.HandFree)//手工优惠类特殊处理。
            {
                switch (_curSelectedCouponInfo.HandCouponType)
                {
                    case EnumHandCouponType.FreeDish:
                        var giftDishWnd = new SelectGiftDishWindow(Data);
                        if (WindowHelper.ShowDialog(giftDishWnd, OwnerWindow))
                        {
                            foreach (var giftDishInfo in giftDishWnd.SelectedGiftDishInfos)
                            {
                                _curSelectedCouponInfo.Name = string.Format("赠菜：{0}", giftDishInfo.DishName);
                                _curSelectedCouponInfo.FreeAmount = giftDishInfo.DishPrice;
                                AddCouponInfoAsUsed(_curSelectedCouponInfo, giftDishInfo.SelectGiftNum, false);
                            }
                        }
                        break;
                    case EnumHandCouponType.Discount:
                        InfoLog.Instance.I("选择手工优惠折扣类优惠券，弹出折扣输入窗口...");
                        var diacountSelectWnd = new OfferTypeSelectWindow(EnumOfferType.Discount);
                        if (WindowHelper.ShowDialog(diacountSelectWnd, OwnerWindow))
                        {
                            InfoLog.Instance.I("选择折扣率：{0}，开始调用接口计算折扣金额...", diacountSelectWnd.Discount);
                            TaskService.Start(diacountSelectWnd.Discount, CalcDiscountAmountProcess, CalcDiscountAmountComplete, "计算折扣金额中...");//优惠券折扣时，自定义折扣为0。
                        }
                        break;
                    case EnumHandCouponType.Amount:
                        InfoLog.Instance.I("选择手工优惠优免类优惠券，弹出优免金额输入窗口...");
                        var amountSelectWnd = new OfferTypeSelectWindow(EnumOfferType.Amount);
                        if (WindowHelper.ShowDialog(amountSelectWnd, OwnerWindow))
                        {
                            AddCouponInfoAsUsed(_curSelectedCouponInfo, 1, false, amountSelectWnd.Amount);
                        }
                        break;
                    case null:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return;
            }

            if (_curSelectedCouponInfo.IsDiscount) //折扣类处理流程。
            {
                if (!MessageDialog.Quest(string.Format("确定使用{0}？", _curSelectedCouponInfo.Name)))
                    return;

                InfoLog.Instance.I("开始折扣类优惠券：{0}计算接口。", _curSelectedCouponInfo.Name);
                TaskService.Start(0m, CalcDiscountAmountProcess, CalcDiscountAmountComplete, "计算折扣金额中...");//优惠券折扣时，自定义折扣为0。
            }
            else
            {
                //增加选择数量的窗口
                var numWnd = new NumberSelectorWindow("请输入优惠券使用数量", 1, 0, false);
                if (!WindowHelper.ShowDialog(numWnd, OwnerWindow))
                    return;

                if (_curSelectedCouponInfo.FreeAmount == 999999 || _curSelectedCouponInfo.DebitAmount == -1) //特殊券。
                {

                }
                else
                {
                    if (_curSelectedCouponInfo.FreeAmount > 0 || _curSelectedCouponInfo.DebitAmount > 0)
                    {
                        if (_curSelectedCouponInfo.CouponType == EnumCouponType.Member)
                        {

                        }
                        else
                        {
                            AddCouponInfoAsUsed(_curSelectedCouponInfo, Convert.ToInt32(numWnd.InputNum), false);
                            //while (num-- > 0)
                            //{
                            //    AddCouponInfoAsUsed(coupon, 1, false);
                            //}
                        }
                    }
                    else if (_curSelectedCouponInfo.FreeAmount <= 0 && _curSelectedCouponInfo.DebitAmount <= 0)
                    {

                    }
                }
            }

            InfoLog.Instance.I("开始保存优惠券到使用列表...");
            var param = new Tuple<string, List<UsedCouponInfo>>(Data.OrderId, Data.UsedCouponInfos.ToList());
            TaskService.Start(param, SaveCouponInfoProcess, SaveCouponInfoComplete);
        }

        /// <summary>
        /// 回车支付。
        /// </summary>
        /// <param name="param"></param>
        private void EnterPay(object param)
        {
            if (!(param is ExCommandParameter))
                return;

            var args = ((ExCommandParameter)param).EventArgs as KeyEventArgs;
            if (args == null)
                return;

            if (args.Key == Key.Enter)
                PayTheBill();
        }

        #endregion

        #region Protected Method

        protected override void InitCommand()
        {
            base.InitCommand();
            GetTableDishInfoCmd = CreateDelegateCommand(GetTableDishInfo);
            DataGridPageOperCmd = CreateDelegateCommand(DataGridPageOper, CanDataGridPageOper);
            PrintCmd = CreateDelegateCommand(Print, CanPrint);
            OperCmd = CreateDelegateCommand(Oper, CanOper);
            CashControlFocusCmd = CreateDelegateCommand(CashControlFocus);
            EnterPayCmd = CreateDelegateCommand(EnterPay);
            CouponMouseDownCmd = CreateDelegateCommand(CouponMouseDown);
            CouponMouseUpCmd = CreateDelegateCommand(CouponMouseUp);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 初始化优惠券集合。
        /// </summary>
        private void InitCouponCategories()
        {
            CouponInfos = new ObservableCollection<CouponInfo>();
            CouponCategories = new ObservableCollection<CouponCategory>
            {
                new CouponCategory {CategoryName = "团购", CategoryType = "05"},
                new CouponCategory {CategoryName = "特价", CategoryType = "01"},
                new CouponCategory {CategoryName = "折扣", CategoryType = "02"},
                new CouponCategory {CategoryName = "代金券", CategoryType = "03"},
                new CouponCategory {CategoryName = "礼品券", CategoryType = "04"},
                new CouponCategory {CategoryName = "会员", CategoryType = "88"},
                new CouponCategory {CategoryName = "其他优惠", CategoryType = "00"},
                new CouponCategory {CategoryName = "合作单位", CategoryType = "08"},
                new CouponCategory {CategoryName = "不常用", CategoryType = "-1"}
            };
            SelectedCouponCategory = CouponCategories.FirstOrDefault();
        }

        /// <summary>
        /// 初始化优惠券长按定时器。
        /// </summary>
        private void InitCouponLongPressTimer()
        {
            _couponLongPressTimer = new Timer(LongPressTimerSecond * 1000);
            _couponLongPressTimer.Elapsed += CouponLongPressTimerOnElapsed;
        }

        /// <summary>
        /// 长按定时器触发时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void CouponLongPressTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _couponLongPressTimer.Stop();
            _isLongPressModel = true;
            OwnerWindow.Dispatcher.BeginInvoke((Action)delegate
            {
                var msg = "";
                if (_curSelectedCouponInfo.IsUncommonlyUsed)
                    msg = string.Format("恢复\"{0}\"为常用优惠{1}（恢复后可在对应分类查看、使用）", _curSelectedCouponInfo.Name, Environment.NewLine);
                else
                    msg = string.Format("设置\"{0}\"为不常用优惠{1}（设置后可在不常用优惠分类查看、使用）", _curSelectedCouponInfo.Name,
                        Environment.NewLine);

                if (!MessageDialog.Quest(msg, OwnerWindow))
                    return;

                var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
                if (service == null)
                {
                    ErrLog.Instance.E("创建IRestaurantService服务失败。");
                    MessageDialog.Warning("创建IRestaurantService服务失败。");
                    return;
                }

                InfoLog.Instance.I("开始设置优惠券的偏好。");
                var result = service.SetCouponFavor(_curSelectedCouponInfo.CouponId, _curSelectedCouponInfo.IsUncommonlyUsed);
                if (!string.IsNullOrEmpty(result))
                {
                    ErrLog.Instance.E(result);
                    MessageDialog.Warning(result);
                    return;
                }

                InfoLog.Instance.I("设置优惠券偏好成功。");
                NotifyDialog.Notify("设置优惠券偏好成功。", OwnerWindow);
                SelectedCouponCategory = SelectedCouponCategory;//触发优惠券的重新获取。
            });
        }

        /// <summary>
        /// 分组优惠券。
        /// </summary>
        private void PacketCoupon()
        {

        }

        /// <summary>
        /// 结账操作执行。
        /// </summary>
        private void PayTheBill()
        {
            InfoLog.Instance.I("开始账单结算...首先检测账单是否允许结账...");
            var msg = CheckTheBillAllowPay();
            if (!string.IsNullOrEmpty(msg))
            {
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, OwnerWindow);
                return;
            }

            if (Data.TipAmount > 0)//有小费的时候，进行小费特有逻辑判断。
            {
                if (TipPaymentAmount < Data.TipAmount)
                {
                    if (Data.TotalAlreadyPayment - Data.PaymentAmount + Data.TipAmount > TipPaymentAmount)//总共支付金额计算出的小费超过约定规则计算后的小费金额，说明小费金额部分不是由现金支付的，不允许结账。
                    {
                        MessageDialog.Warning(string.Format("小费{0}元，必须使用现金结算。", Data.TipAmount));
                        return;
                    }

                    if (!MessageDialog.Quest(string.Format("还有{0}元小费未结算，点击确定继续结算，点击取消取消结算。", Data.TipAmount - TipPaymentAmount)))
                        return;
                }
                MessageDialog.Warning("还有");
            }

            if (!MessageDialog.Quest(string.Format("台号：{0} 确定现在结算吗？", Data.TableName), OwnerWindow))
                return;

            var param = new Tuple<string, string, List<BillPayInfo>>(Data.OrderId, Globals.UserInfo.UserName, GenerateBillPayInfos());
            InfoLog.Instance.I("开始账单结算...");

            var payBillWorkFlow = new WorkFlowInfo(PayTheBillProcess, PayTheBillComplete, "账单结算中...");
            var curStepWf = payBillWorkFlow;
            if (!string.IsNullOrEmpty(Data.MemberNo))
            {
                if (Data.MemberInfo == null)
                {
                    var queryMemberWf = GenerateMemberQueryWf();
                    curStepWf.NextWorkFlowInfo = queryMemberWf;
                    curStepWf = queryMemberWf;
                }

                var saleMemberWf = GenerateMemberSaleWf();
                curStepWf.NextWorkFlowInfo = saleMemberWf;
                curStepWf = saleMemberWf;

                if (HasTip && Data.TipAmount > 0)
                {
                    var tipSettlementWf = new WorkFlowInfo(TipSettlementProcess, TipSettlementComplete, "小费结算中...");
                    curStepWf.NextWorkFlowInfo = tipSettlementWf;
                    curStepWf = tipSettlementWf;
                }

                var antiSettlementWf = new WorkFlowInfo(AutoAntiSettlementProcess, AutoAntiSettlementComplete, "自动反结算中...");
                curStepWf.ErrorWorkFlowInfo = antiSettlementWf;//会员消费结算错误时执行自动反结算工作流。
            }

            var jdeDebitAmountWf = new WorkFlowInfo(JdeDebitAmountProcess, JdeDebitAmountComplete, "计算实收执行中...");
            curStepWf.NextWorkFlowInfo = jdeDebitAmountWf;//结算的最后一个执行步骤为调用JDE计算实收。
            jdeDebitAmountWf.NextWorkFlowInfo = new WorkFlowInfo(null, PrintSettlementReportAndInvoice);//打印和开发票

            WorkFlowService.Start(param, payBillWorkFlow);
        }

        /// <summary>
        /// 退菜执行方法。
        /// </summary>
        private void BackDish()
        {
            if (SelectedOrderDish == null)
                return;

            if (SelectedOrderDish.DishStatus == EnumDishStatus.ToBeWeighed)
            {
                DishWeight();
                return;
            }

            if (SelectedOrderDish.IsComboDish)
            {
                MessageDialog.Warning("请选择套餐主体退整个套餐。");
                return;
            }

            if (SelectedOrderDish.IsFishPotDish && SelectedOrderDish.IsPot) //选中了鱼锅的锅。
            {
                MessageDialog.Warning("请选择鱼锅主体退整个鱼锅。");
                return;
            }

            var backDishReasonWnd = new BackDishReasonSelectWindow();
            if (!WindowHelper.ShowDialog(backDishReasonWnd, OwnerWindow))
                return;

            var numWnd = new NumberSelectorWindow("请输入退菜数量", SelectedOrderDish.DishNum, SelectedOrderDish.DishNum);
            if (!WindowHelper.ShowDialog(numWnd, OwnerWindow))
                return;

            var authorizeWnd = new AuthorizationWindow(EnumRightType.BackDish);
            if (WindowHelper.ShowDialog(authorizeWnd, OwnerWindow))
                TaskService.Start(numWnd.InputNum, GetBackDishInfoProcess, GetBackDishInfoComplete, "获取退菜信息...");
        }

        /// <summary>
        /// 点击加菜时执行。
        /// </summary>
        private void AddDish()
        {
            if (SelectedOrderDish == null)
                return;

            if (SelectedOrderDish.DishStatus == EnumDishStatus.ToBeWeighed)
            {
                DishWeight();
                return;
            }

            if (WindowHelper.ShowDialog(new OrderDishWindow(Data), OwnerWindow))
                GetTableDishInfoAsync();
        }

        /// <summary>
        /// 菜品称重。
        /// </summary>
        /// <returns></returns>
        private void DishWeight()
        {
            InfoLog.Instance.I("选中的菜时称重菜品，弹出称重窗体...");
            var dishWeightWnd = new DishWeightWindow();
            if (WindowHelper.ShowDialog(dishWeightWnd, OwnerWindow))
            {
                InfoLog.Instance.I("菜品\"{0}\"称重数量：{1}", SelectedOrderDish.DishName, dishWeightWnd.DishWeightNum);
                var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
                if (service == null)
                {
                    ErrLog.Instance.E("创建IOrderService服务失败。");
                    MessageDialog.Warning("创建IOrderService服务失败。");
                    return;
                }

                InfoLog.Instance.I("开始调用菜品称重接口...");
                var result = service.UpdateDishWeight(Data.TableNo, SelectedOrderDish.DishId,
                    SelectedOrderDish.PrimaryKey, dishWeightWnd.DishWeightNum);
                if (!string.IsNullOrEmpty(result))
                {
                    ErrLog.Instance.E("菜品\"{0}\"称重失败：{1}", SelectedOrderDish.DishName, result);
                    MessageDialog.Warning(result);
                    return;
                }

                var msg = string.Format("菜品\"{0}\"称重成功。", SelectedOrderDish.DishName);
                InfoLog.Instance.I(msg);
                NotifyDialog.Notify(msg, OwnerWindow);
                GetTableDishInfoAsync();
            }
        }

        /// <summary>
        /// 清台功能。
        /// </summary>
        private void ClearCoffeeTable()
        {
            var questMsg = "确定要清台吗？";
            if (!Data.HasBeenPaied && Data.DishInfos.Any())
                questMsg += "清台后已点菜品将全部清空。";

            if (!MessageDialog.Quest(questMsg))
                return;

            TaskService.Start(null, ClearCoffeeTableProcess, ClearCoffeeTableComplete, "清台执行中...");
        }

        /// <summary>
        /// 执行反结算。
        /// </summary>
        private void ResettlementSync()
        {
            if (!MessageDialog.Quest(string.Format("台号：{0} 确定要反结算吗？", Data.TableName), OwnerWindow))
                return;

            InfoLog.Instance.I("开始检测账单是否允许反结算...");
            var checkWf = new WorkFlowInfo(CheckOrderCanResettlementProcess, CheckOrderCanResettlementComplete);//检测订单是否允许反结工作流。
            var currentWf = checkWf;//当前工作流。
            if (!string.IsNullOrEmpty(Data.MemberNo))
            {
                if (Globals.IsCanDaoMember)
                {
                    var canDaoMemberResettlementWf = new WorkFlowInfo(CanDaoMemberResettlementProcess, CanDaoMemberResettlementComplete);//餐道会员反结算工作流。
                    checkWf.NextWorkFlowInfo = canDaoMemberResettlementWf;
                    currentWf = canDaoMemberResettlementWf;
                }
                else if (Globals.IsYazuoMember)
                {
                    var yaZuoMemberResettlementWf = new WorkFlowInfo(YaZuoMemberResettlementProcess, YaZuoMemberResettlementComplete);//雅座会员反结算工作流。
                    checkWf.NextWorkFlowInfo = yaZuoMemberResettlementWf;
                    currentWf = yaZuoMemberResettlementWf;
                }
            }

            var antiSettlementWf = new WorkFlowInfo(AutoAntiSettlementProcess, AutoAntiSettlementComplete, "账单反结算中...");
            currentWf.NextWorkFlowInfo = antiSettlementWf;
            var param = new Tuple<string, string>(Data.OrderId, Globals.UserInfo.UserName);
            WorkFlowService.Start(param, checkWf);
        }

        /// <summary>
        /// 取消账单。
        /// </summary>
        private void CancelOrderAsync()
        {
            if (Data.DishInfos.Any())
            {
                MessageDialog.Warning("只能取消空账单。", OwnerWindow);
                return;
            }

            if (!MessageDialog.Quest(string.Format("确定要取消桌号：{0}的帐单吗?", _tableInfo.TableName)))
                return;

            WorkFlowService.Start(null, new WorkFlowInfo(ClearTableProcess, ClearTableComplete, "取消账单中..."));
        }

        /// <summary>
        /// 打开钱箱。（异步）
        /// </summary>
        private void OpenCashBoxAsync()
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                InfoLog.Instance.I("开始打开钱箱...");
                var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
                if (service == null)
                {
                    ErrLog.Instance.E("创建IRestaurantService服务失败。");
                    return;
                }

                var result = service.OpenCash(SystemConfigCache.OpenCashIp);
                if (!string.IsNullOrEmpty(result))
                    ErrLog.Instance.E("打开钱箱失败：{0}", result);
                else
                    InfoLog.Instance.I("打开钱箱成功。");
            });
        }

        /// <summary>
        /// 异步获取餐台详情。
        /// </summary>
        protected void GetTableDishInfoAsync()
        {
            TaskService.Start(null, GetTableDishInfoProcess, GetTableDishInfoComplete, "加载餐台详情...");
        }

        /// <summary>
        /// 生成会员查询工作流。
        /// </summary>
        /// <returns></returns>
        private WorkFlowInfo GenerateMemberQueryWf()
        {
            WorkFlowInfo wf = null;
            if (Globals.IsCanDaoMember)
                wf = new WorkFlowInfo(QueryMemberCanDaoProcess, QueryMemberCanDaoComplete, "会员查询中...");
            else if (Globals.IsYazuoMember)
                wf = new WorkFlowInfo(QueryMemberYazuoProcess, QueryMemberYazuoComplete, "会员查询中...");
            return wf;
        }

        /// <summary>
        /// 生成会员消费工作流。
        /// </summary>
        /// <returns></returns>
        private WorkFlowInfo GenerateMemberSaleWf()
        {
            WorkFlowInfo wf = null;
            if (Globals.IsCanDaoMember)
                wf = new WorkFlowInfo(SaleMemberCanDaoProcess, SaleMemberCanDaoComplete, "会员消费结算中...");
            else if (Globals.IsYazuoMember)
                wf = new WorkFlowInfo(SaleMemberYazuoProcess, SaleMemberYazuoComplete, "会员消费结算中...");

            return wf;
        }

        /// <summary>
        /// 生成会员登录工作流。
        /// </summary>
        /// <returns></returns>
        private WorkFlowInfo GenerateMemberLoginWf()
        {
            WorkFlowInfo wf = null;
            if (Globals.MemberSystem == EnumMemberSystem.Candao)
                wf = new WorkFlowInfo(MemberCanDaoLoginProcess, MemberCanDaoLoginComplete, "会员登录中...");
            else if (Globals.MemberSystem == EnumMemberSystem.Yazuo)
            {

            }
            return wf;
        }

        /// <summary>
        /// 生成会员登出工作流。
        /// </summary>
        /// <returns></returns>
        private WorkFlowInfo GenerateMemberLogoutWf()
        {
            WorkFlowInfo wf = null;
            if (Globals.MemberSystem == EnumMemberSystem.Candao)
                wf = new WorkFlowInfo(MemberCanDaoLogoutProcess, MemberCanDaoLogoutComplete, "会员登出中...");
            else if (Globals.MemberSystem == EnumMemberSystem.Yazuo)
            {

            }
            return wf;
        }

        /// <summary>
        /// 生成整单退菜工作流。
        /// </summary>
        /// <returns></returns>
        protected WorkFlowInfo GenerateBackAllDishWf()
        {
            return new WorkFlowInfo(BackAllDishProcess, BackAllDishComplete, "整桌退菜中...");
        }

        /// <summary>
        /// 执行结账的方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object PayTheBillProcess(object param)
        {
            InfoLog.Instance.I("开始账单结算...");
            var args = (Tuple<string, string, List<BillPayInfo>>)param;

            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            if (Data.TableType == EnumTableType.CFTable || Data.TableType == EnumTableType.CFTakeout || Data.TableType == EnumTableType.Takeout)//咖啡台和外卖都走咖啡结账模式，后台处理是结账后打单。
                return service.PayTheBillCf(args.Item1, args.Item2, args.Item3);

            return service.PayTheBill(args.Item1, args.Item2, args.Item3);
        }

        /// <summary>
        /// 结账执行完成。
        /// </summary>
        /// <param name="param"></param>
        private Tuple<bool, object> PayTheBillComplete(object param)
        {
            var result = param as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E("账单结算失败：{0}", result);
                MessageDialog.Warning(result, OwnerWindow);
                return null;
            }

            InfoLog.Instance.I("账单结算成功。");
            return new Tuple<bool, object>(true, null);
        }

        /// <summary>
        /// 餐道会员查询执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object QueryMemberCanDaoProcess(object arg)
        {
            InfoLog.Instance.I("开始执行餐道会员查询...");
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, MemberInfo>("创建IMemberService服务失败。", null);

            var request = new CanDaoMemberQueryRequest
            {
                branch_id = Globals.BranchInfo.BranchId,
                cardno = MemberCardNo,
            };
            return service.QueryCanndao(request);
        }

        /// <summary>
        /// 餐道会员查询执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> QueryMemberCanDaoComplete(object arg)
        {
            var result = (Tuple<string, MemberInfo>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("餐道会员查询失败：{0}", result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return new Tuple<bool, object>(false, null);//不能return null，因为要执行错误工作流。
            }

            InfoLog.Instance.I("餐道会员查询成功。");
            Data.MemberInfo = result.Item2;
            return new Tuple<bool, object>(true, Data.MemberInfo);
        }

        /// <summary>
        /// 雅座会员查询执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object QueryMemberYazuoProcess(object arg)
        {
            return null;
        }

        /// <summary>
        /// 雅座会员查询执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> QueryMemberYazuoComplete(object arg)
        {
            IsMemberLogin = true;
            return null;
        }

        /// <summary>
        /// 会员登录时的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object MemberCanDaoLoginProcess(object arg)
        {
            InfoLog.Instance.I("开始执行餐道会员查询...");
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            var request = new CanDaoMemberQueryRequest
            {
                branch_id = Globals.BranchInfo.BranchId,
                cardno = MemberCardNo,
            };
            var queryResult = service.QueryCanndao(request);
            InfoLog.Instance.I("结束餐道会员查询。");
            if (!string.IsNullOrEmpty(queryResult.Item1))
                return string.Format("餐道会员查询失败：{0}", queryResult.Item1);

            Data.MemberInfo = queryResult.Item2;

            InfoLog.Instance.I("开始执行餐道会员登入...");
            var loginResult = service.MemberLogin(Data.OrderId, MemberCardNo);
            if (!string.IsNullOrEmpty(loginResult))
                return string.Format("餐道会员登入失败：{0}。", loginResult);

            InfoLog.Instance.I("餐道会员登入成功。");
            InfoLog.Instance.I("开始设置会员价...");
            var orderService = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (orderService == null)
                return "创建IOrderService服务失败。";

            var setMemberPriceResult = orderService.SetMemberPrice(Data.OrderId, MemberCardNo);
            if (!string.IsNullOrEmpty(setMemberPriceResult))
                return string.Format("设置会员价失败：{0}", setMemberPriceResult);

            InfoLog.Instance.I("设置会员价完成。");
            InfoLog.Instance.I("从新获取餐台所有信息...");

            var result = new Tuple<string, TableFullInfo>("未赋值", null);
            if (Data.TableType == EnumTableType.CFTakeout || Data.TableType == EnumTableType.Takeout)
                result = orderService.GetTableDishInfoByOrderId(_tableInfo.OrderId, Globals.UserInfo.UserName);
            else
                result = orderService.GetTableDishInfoes(_tableInfo.TableName, Globals.UserInfo.UserName);

            if (!string.IsNullOrEmpty(result.Item1))
                return string.Format("获取餐台明细失败：{0}", result.Item1);

            InfoLog.Instance.I("获取餐台所有信息完成。");
            if (result.Item2 == null)
                return "没有获取到该餐台的账单信息。";

            result.Item2.MemberInfo = Data.MemberInfo;//会员信息保留，不用再次查询了。
            Data = result.Item2;
            return null;
        }

        /// <summary>
        /// 会员登录执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> MemberCanDaoLoginComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result, OwnerWindow);
                return new Tuple<bool, object>(false, null);//不能return null，因为要执行错误工作流。
            }

            InfoLog.Instance.I("设置会员价成功，完成整个会员登录流程。");
            IsMemberLogin = true;
            GetTableDishInfoAsync();
            return new Tuple<bool, object>(true, null);
        }

        /// <summary>
        /// 会员登出时执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object MemberCanDaoLogoutProcess(object arg)
        {
            InfoLog.Instance.I("开始执行餐道会员登出...");
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            var logoutResult = service.MemberLogout(Data.OrderId, MemberCardNo);
            if (!string.IsNullOrEmpty(logoutResult))
                return string.Format("餐道会员登出失败：{0}。", logoutResult);

            InfoLog.Instance.I("餐道会员登出成功。");
            InfoLog.Instance.I("开始设置成正常价...");
            var orderService = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (orderService == null)
                return "创建IOrderService服务失败。";

            var setNormalPriceResult = orderService.SetNormalPrice(Data.OrderId);
            return !string.IsNullOrEmpty(setNormalPriceResult) ? string.Format("设置成正常价失败：{0}", setNormalPriceResult) : null;
        }

        /// <summary>
        /// 会员登出执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> MemberCanDaoLogoutComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result, OwnerWindow);
                return null;
            }

            InfoLog.Instance.I("设置成正常价成功，完成整个会员登出流程。");
            IsMemberLogin = false;
            Data.MemberInfo = null;
            MemberCardNo = null;
            GetTableDishInfoAsync();
            return null;
        }

        /// <summary>
        /// 餐道会员消费执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object SaleMemberCanDaoProcess(object arg)
        {
            InfoLog.Instance.I("开始会员消费结算...");
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            var request = new CanDaoMemberSaleRequest
            {
                branch_id = Globals.BranchInfo.BranchId,
                cardno = Data.MemberInfo.CardNo,
                password = MemberPassword,
                FCash = CashAmount,
                FStore = MemberAmount,
                FIntegral = IntegralAmount,
                Serial = Data.OrderId,
            };
            var result = service.Sale(request);
            if (!string.IsNullOrEmpty(result.Item1))
                return string.Format("餐道会员消费结算失败：{0}", result.Item1);

            //添加会员消费记录。
            var saleInfoRequest = new AddOrderMemberSaleInfoRequest
            {
                cardno = Data.MemberInfo.CardNo,
                orderid = Data.OrderId,
                userid = Globals.UserInfo.UserName,
                business = Globals.BranchInfo.BranchId,
                terminal = SystemConfigCache.PosId,
                serial = result.Item2.TraceCode,
                businessname = Globals.BranchInfo.BranchName,
                score = result.Item2.AddIntegral - result.Item2.DecIntegral,
                scorebalance = result.Item2.IntegralOverall,
                storedbalance = result.Item2.StoreCardBalance,
                netvalue = MemberAmount,
                stored = MemberAmount,
            };

            var saleInfoResult = service.AddMemberSaleInfo(saleInfoRequest);
            return !string.IsNullOrEmpty(saleInfoResult) ? string.Format("添加会员消费信息失败：{0}", saleInfoResult) : null;
        }

        /// <summary>
        /// 餐道会员消费执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> SaleMemberCanDaoComplete(object arg)
        {
            var result = (string)arg;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result, OwnerWindow);
                return new Tuple<bool, object>(false, null); //会员结算失败，走错误流程。
            }

            return new Tuple<bool, object>(true, null);
        }

        /// <summary>
        /// 雅座会员消费执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object SaleMemberYazuoProcess(object arg)
        {
            return null;
        }

        /// <summary>
        /// 雅座会员消费执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> SaleMemberYazuoComplete(object arg)
        {
            return new Tuple<bool, object>(true, null);
        }

        /// <summary>
        /// 自动反结算执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object AutoAntiSettlementProcess(object arg)
        {
            InfoLog.Instance.I("开始反结算账单：{0}...", Data.OrderId);
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            var reason = _antiSettlementReason;
            if (string.IsNullOrEmpty(reason))
                reason = "会员结算失败，系统自动反结";
            return service.AntiSettlementOrder(Globals.UserInfo.UserName, Data.OrderId, reason);
        }

        /// <summary>
        /// 自动反结算执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> AutoAntiSettlementComplete(object arg)
        {
            _antiSettlementReason = null;
            var result = (string)arg;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result, OwnerWindow);
                return null;
            }

            InfoLog.Instance.I("反结算账单：{0}完成。", Data.OrderId);
            _tableInfo.TableStatus = EnumTableStatus.Dinner;//将餐桌状态设置成就餐，调用GetTableDishInfo获取餐桌信息时就不会弹出开台窗口了。
            GetTableDishInfoAsync();
            return null;
        }

        /// <summary>
        /// JDE计算实收执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object JdeDebitAmountProcess(object arg)
        {
            InfoLog.Instance.I("开始调用JDE计算实收接口...");
            return null;
        }

        /// <summary>
        /// JDE计算实收执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> JdeDebitAmountComplete(object arg)
        {
            InfoLog.Instance.I("调用JDE计算时候接口结束，也结束了账单结算。");
            return new Tuple<bool, object>(true, null);
        }

        /// <summary>
        /// 打印结账单和发票。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> PrintSettlementReportAndInvoice(object arg)
        {
            if (Data.TableType == EnumTableType.Outside || Data.TableType == EnumTableType.Room)
                BroadcastSettlementMsgAsync();
            else if (Data.TableType == EnumTableType.CFTable)
                BroadcastCoffeeSettlementMsgAsyc();

            InfoLog.Instance.I("开始打印结账单...");
            ReportPrintHelper.PrintSettlementReport(Data.OrderId, Globals.UserInfo.UserName);
            InfoLog.Instance.I("结束打印结账单。");

            if (!string.IsNullOrEmpty(MemberCardNo))
            {
                InfoLog.Instance.I("开始打印交易凭条...");
                ReportPrintHelper.PrintMemberPayBillReport(Data.OrderId, Globals.UserInfo.UserName);
                InfoLog.Instance.I("结束打印交易凭条。");
            }

            if (!string.IsNullOrEmpty(Data.OrderInvoiceTitle))
            {
                InfoLog.Instance.I("有发票信息，显示发票金额设置窗口...");
                WindowHelper.ShowDialog(new SetInvoiceAmountWindow(Data), OwnerWindow);
            }

            NotifyDialog.Notify(string.Format("桌号\"{0}\"结账成功。", Data.TableName), OwnerWindow.Owner);
            InfoLog.Instance.I("结束整个结账流程，关闭窗口。");
            DosomethingAfterSettlement();
            return null;
        }

        /// <summary>
        /// 异步发送结算广播消息。
        /// </summary>
        private void BroadcastSettlementMsgAsync()
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                InfoLog.Instance.I("广播结算消息给PAD...");
                var errMsg = CommonHelper.BroadcastMessage(EnumBroadcastMsgType.Settlement2Pad, Data.OrderId);
                if (!string.IsNullOrEmpty(errMsg))
                    ErrLog.Instance.E("广播结算指令失败：{0}", (int)EnumBroadcastMsgType.Settlement2Pad);

                InfoLog.Instance.I("广播结算指令给手环...");
                var msg = string.Format("{0}|{1}|{2}", Data.WaiterId, Data.TableName, Data.OrderId);
                errMsg = CommonHelper.BroadcastMessage(EnumBroadcastMsgType.Settlement2Wristband, msg);
                if (!string.IsNullOrEmpty(errMsg))
                    ErrLog.Instance.E("广播结算指令失败：{0}", (int)EnumBroadcastMsgType.Settlement2Wristband);
            });
        }

        /// <summary>
        /// 异步发送咖啡模式结账广播消息。
        /// </summary>
        private void BroadcastCoffeeSettlementMsgAsyc()
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                var msg = string.Format("{0}|{1}|{2}", Data.TableNo, Data.OrderId, Data.PaymentAmount);
                var errMsg = CommonHelper.BroadcastMessage(EnumBroadcastMsgType.Settlement2Wristband, msg);
                if (!string.IsNullOrEmpty(errMsg))
                    ErrLog.Instance.E("广播结算指令失败：{0}", (int)EnumBroadcastMsgType.Settlement2Wristband);
            });
        }

        /// <summary>
        /// 异步发送清台广播消息。
        /// </summary>
        private void BroadcastClearTableMsgAsync()
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                var errMsg = CommonHelper.BroadcastMessage(EnumBroadcastMsgType.ClearTable, Data.OrderId);
                if (!string.IsNullOrEmpty(errMsg))
                    ErrLog.Instance.E("广播清台指令失败：{0}", (int)EnumBroadcastMsgType.ClearTable);
                var msg = string.Format("{0}|{1}|{2}", Data.TableNo, Data.OrderId, Data.PaymentAmount);
                errMsg = CommonHelper.BroadcastMessage(EnumBroadcastMsgType.Settlement2Wristband, msg);
                if (!string.IsNullOrEmpty(errMsg))
                    ErrLog.Instance.E("广播手环结算指令失败：{0}", (int)EnumBroadcastMsgType.Settlement2Wristband);
            });
        }

        /// <summary>
        /// 小费结算执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object TipSettlementProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            return service.TipSettlement(Data.OrderId, TipPaymentAmount);
        }

        /// <summary>
        /// 小费计算完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> TipSettlementComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result);
                return new Tuple<bool, object>(false, null);
            }

            return new Tuple<bool, object>(true, null);
        }

        private object BackAllDishProcess(object param)
        {
            InfoLog.Instance.I("开始桌台{0}整桌退菜...", Data.TableNo);
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            return service.BackAllDish(Data.OrderId, Data.TableName, Globals.UserInfo.UserName);
        }

        protected virtual Tuple<bool, object> BackAllDishComplete(object arg)
        {
            var result = (string)arg;
            if (!string.IsNullOrEmpty(result))
            {
                var msg = string.Format("桌台{0}整桌退菜失败：{1}", Data.TableNo, result);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, OwnerWindow);
                BackAllDishFailedProcess();
                return null;
            }

            BackAllDishSuccessProcess();
            InfoLog.Instance.I("桌台{0}整桌退菜成功：{1}", Data.TableNo, result);
            return new Tuple<bool, object>(true, null);
        }

        /// <summary>
        /// 咖啡模式清台执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object ClearCoffeeTableProcess(object param)
        {
            InfoLog.Instance.I("开始咖啡模式清台...");
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            if (!Data.HasBeenPaied && Data.DishInfos.Any())//未结账且有菜品，则先整单退菜。
            {
                var result = (string)BackAllDishProcess(null);
                if (!string.IsNullOrEmpty(result))
                    return result;
            }
            return service.ClearTableCf(_tableInfo.TableName);
        }

        /// <summary>
        /// 咖啡模式清台执行完成。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private void ClearCoffeeTableComplete(object param)
        {
            var result = (string)param;
            if (!string.IsNullOrEmpty(result))
            {
                var msg = string.Format("咖啡模式清台失败“{0}", result);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, OwnerWindow);
                return;
            }

            BroadcastClearTableMsgAsync();
            CloseWindow(true);
        }

        /// <summary>
        /// 退菜失败的处理。
        /// </summary>
        protected virtual void BackAllDishFailedProcess()
        {

        }

        /// <summary>
        /// 退菜成功的处理。
        /// </summary>
        protected virtual void BackAllDishSuccessProcess()
        {

        }

        /// <summary>
        /// 获取餐台菜品信息执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object GetTableDishInfoProcess(object param)
        {
            InfoLog.Instance.I("开始获取餐台所有信息...");
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            var result = service.GetTableDishInfoes(_tableInfo.TableName, Globals.UserInfo.UserName);
            if (!string.IsNullOrEmpty(result.Item1))
                return string.Format("获取餐台明细失败：{0}", result.Item1);

            InfoLog.Instance.I("获取餐台所有信息完成。");
            if (result.Item2 == null)
                return "没有获取到该餐台的账单信息。";

            OwnerWindow.Dispatcher.Invoke((Action)delegate
            {
                if (Data == null)
                    Data = result.Item2;
                else
                    Data.CloneData(result.Item2);
                MemberCardNo = Data.MemberNo;
            });

            if (!string.IsNullOrEmpty(MemberCardNo))//走会员登录的流程。
            {
                InfoLog.Instance.I("该餐台登录了会员，开始会员登录...");
                var memberLoginResult = MemberCanDaoLoginProcess(null) as string;
                if (!string.IsNullOrEmpty(memberLoginResult))
                    ErrLog.Instance.E("会员登录时失败：{0}", memberLoginResult);
                else
                {
                    InfoLog.Instance.I("设置会员价成功，完成整个会员登录流程。");
                    IsMemberLogin = true;
                }
            }

            CalculatePaymentAmount();
            InfoLog.Instance.I("开始获取订单{0}的发票抬头。", Data.OrderId);
            var invoiceResult = service.GetOrderInvoice(Data.OrderId);
            if (!string.IsNullOrEmpty(invoiceResult.Item1))
                return string.Format("获取订单发票抬头失败。" + invoiceResult.Item1);

            InfoLog.Instance.I("结束获取订单发票抬头：{0}。", invoiceResult.Item2);
            Data.OrderInvoiceTitle = invoiceResult.Item2;
            return null;
        }

        /// <summary>
        ///  获取餐台菜品信息执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void GetTableDishInfoComplete(object param)
        {
            var result = (string)param;
            if (!string.IsNullOrEmpty(result))
            {
                MessageDialog.Warning(result, OwnerWindow);
                CloseWindow(false);
            }
        }

        /// <summary>
        /// 获取优惠券执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object GetCouponCategoriesProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return new Tuple<string, List<CouponInfo>>("创建IOrderService服务失败。", null);

            return service.GetCouponInfos(arg.ToString(), Globals.UserInfo.UserName);
        }

        /// <summary>
        ///  获取优惠券执行完成。
        /// </summary>
        /// <param name="obj"></param>
        private void GetCouponCategoriesComplete(object obj)
        {
            var result = (Tuple<string, List<CouponInfo>>)obj;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取优惠券失败：{0}。", result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            InfoLog.Instance.I("结束获取\"{0}\"优惠券。优惠券个数：{1}", SelectedCouponCategory.CategoryName, result.Item2 != null ? result.Item2.Count : 0);
            if (result.Item2 != null)
                result.Item2.ForEach(t =>
                {
                    SelectedCouponCategory.CouponInfos.Add(t);
                    CouponInfos.Add(t);
                });
        }

        /// <summary>
        /// 保存优惠券执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object SaveCouponInfoProcess(object arg)
        {
            IOrderService service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            Tuple<string, List<UsedCouponInfo>> param = (Tuple<string, List<UsedCouponInfo>>)arg;
            return service.SaveUsedCoupon(param.Item1, Globals.UserInfo.UserName, param.Item2);
        }

        /// <summary>
        /// 保存优惠券执行完成。
        /// </summary>
        /// <param name="obj"></param>
        private void SaveCouponInfoComplete(object obj)
        {
            var result = obj as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E("保存折扣类优惠失败：{0}", result);
                MessageDialog.Warning(result, OwnerWindow);
                return;
            }

            InfoLog.Instance.I("结束保存优惠券到使用列表。");
        }

        /// <summary>
        /// 获取退菜信息的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object GetBackDishInfoProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return new Tuple<string, List<BackDishInfo>>("创建IOrderService服务失败。", null);

            var selectedOrderDish = (OrderDishInfo)arg;
            return service.GetBackDishInfo(Data.OrderId, Data.TableName, selectedOrderDish.DishId,
                selectedOrderDish.SrcDishUnit);
        }

        /// <summary>
        /// 获取退菜信息执行完成。
        /// </summary>
        /// <param name="obj"></param>
        private void GetBackDishInfoComplete(object obj)
        {
            var result = (Tuple<string, List<BackDishInfo>>)obj;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            bool allowInputBackNum = true;
            var firstItem = result.Item2.First();
            if (firstItem.IsPot || (firstItem.IsMaster && firstItem.DishType == EnumDishType.FishPot))
                allowInputBackNum = false;

            if (firstItem.DishType == EnumDishType.Packages)
            {
                if (firstItem.ChildDishType != 2)
                {
                    MessageDialog.Warning("请选择套餐名称退整个套餐！", OwnerWindow);
                    return;
                }
                allowInputBackNum = false;
            }

            if (allowInputBackNum)
            {
                //输入退菜数量
            }

            AuthorizationWindow wnd = new AuthorizationWindow(EnumRightType.BackDish);
            if (!WindowHelper.ShowDialog(wnd, OwnerWindow))
                return;

            MessageDialog.Warning("退菜权限验证成功。", OwnerWindow);
        }

        /// <summary>
        /// 计算折扣优免金额执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object CalcDiscountAmountProcess(object arg)
        {
            var discount = (decimal)arg;//折扣率。
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return new Tuple<string, decimal>("创建IOrderService服务失败。", 0m);

            var request = new CalcDiscountAmountRequest
            {
                disrate = discount.ToString(CultureInfo.InvariantCulture),
                userid = Globals.UserInfo.UserName,
                machineno = MachineManage.GetMachineId(),
                orderid = Data.OrderId,
                type = GetCouponTypeString(_curSelectedCouponInfo.CouponType),
                preferentialAmt = (Data.TotalDebitAmount + Data.TotalFreeAmount).ToString(CultureInfo.InvariantCulture),
                preferentialid = _curSelectedCouponInfo.RuleId,
            };
            return service.CalcDiscountAmount(request);
        }

        /// <summary>
        /// 计算折扣优免金额执行完成。
        /// </summary>
        /// <param name="arg"></param>
        private void CalcDiscountAmountComplete(object arg)
        {
            var result = (Tuple<string, decimal>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("折扣类优惠券\"{0}\"计算错误：{1}", _curSelectedCouponInfo.Name, result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            InfoLog.Instance.I("结束折扣类优惠券\"{0}\"计算，结果：{1}。", _curSelectedCouponInfo.Name, result.Item2);
            AddCouponInfoAsUsed(_curSelectedCouponInfo, 1, true, result.Item2);
        }

        /// <summary>
        /// 清台的执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object ClearTableProcess(object param)
        {
            InfoLog.Instance.I("开始清台...");
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            return service.ClearTable(_tableInfo.TableName);
        }

        /// <summary>
        /// 清台执行完成。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private Tuple<bool, object> ClearTableComplete(object param)
        {
            var result = (string)param;
            if (!string.IsNullOrEmpty(result))
            {
                var msg = string.Format("账单取消失败“{0}", result);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, OwnerWindow);
                return null;
            }

            InfoLog.Instance.I("取消账单完成。");
            if (!Data.IsTakeoutTable)
            {
                ThreadPool.QueueUserWorkItem(t =>
                {
                    InfoLog.Instance.I("广播清台消息给PAD...");
                    var errMsg = CommonHelper.BroadcastMessage(EnumBroadcastMsgType.ClearTable, Data.OrderId);
                    if (!string.IsNullOrEmpty(errMsg))
                        ErrLog.Instance.E("广播清台指令失败：{0}", (int)EnumBroadcastMsgType.ClearTable);
                });
            }
            NotifyDialog.Notify("取消账单完成。", OwnerWindow.Owner);
            CloseWindow(true);
            return null;
        }

        #region 反结算

        private object CheckOrderCanResettlementProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            var param = (Tuple<string, string>)arg;
            return service.CheckCanAntiSettlement(param.Item1, param.Item2);
        }

        private Tuple<bool, object> CheckOrderCanResettlementComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E("检测账单是否允许反结算失败：{0}", result);
                MessageDialog.Warning(result, OwnerWindow);
                return null;
            }

            InfoLog.Instance.I("结束检测账单是否允许反结算。");
            InfoLog.Instance.I("弹出选择反结算原因选择窗口，选择反结算原因...");
            var wnd = new AntiSettlementReasonSelectorWindow();
            if (!WindowHelper.ShowDialog(wnd, OwnerWindow))
            {
                InfoLog.Instance.I("取消选择反结算原因，退出反结算流程。");
                return null;
            }

            _antiSettlementReason = wnd.SelectedReason;
            InfoLog.Instance.I("结束选择反结算原因：{0}", wnd.SelectedReason);
            InfoLog.Instance.I("开始反结算授权...");
            if (!WindowHelper.ShowDialog(new AuthorizationWindow(EnumRightType.AntiSettlement), OwnerWindow))
            {
                InfoLog.Instance.I("反结算授权失败，退出反结算流程。");
                return null;
            }

            InfoLog.Instance.I("反结算授权成功，开始会员系统反结算...");
            return new Tuple<bool, object>(true, Data.OrderId);
        }

        /// <summary>
        /// 餐道会员反结算执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object CanDaoMemberResettlementProcess(object arg)
        {
            var orderId = arg as string;
            InfoLog.Instance.I("开始获取订单：\"{0}\"的会员信息...", orderId);
            var orderService = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (orderService == null)
            {
                var msg = "创建IOrderService服务失败。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            var result = orderService.GetOrderMemberInfo(orderId);
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取订单会员信息失败：{0}", result.Item1);
                return result.Item1;
            }

            if (!result.Item2.IsSuccess)//没有获取到订单的会员信息。
            {
                InfoLog.Instance.I("没有获取到订单的会员信息。");
                return null;
            }

            InfoLog.Instance.I("结束获取订单会员信息，交易号：{0}，卡号：{1}", result.Item2.serial, result.Item2.cardno);
            var memberService = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (memberService == null)
            {
                var msg = "创建IMemberService服务失败。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            InfoLog.Instance.I("开始会员消费反结算...");

            return null;
        }

        /// <summary>
        /// 餐道会员反结算执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> CanDaoMemberResettlementComplete(object arg)
        {
            return null;
        }

        /// <summary>
        /// 雅座会员反结算执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object YaZuoMemberResettlementProcess(object arg)
        {
            return null;
        }

        /// <summary>
        /// 雅座会员反结算执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> YaZuoMemberResettlementComplete(object arg)
        {
            return null;
        }



        #endregion

        /// <summary>
        /// 获取优惠券类型对应字符串。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetCouponTypeString(EnumCouponType type)
        {
            return ((int)type).ToString().PadLeft(type != EnumCouponType.Member ? 2 : 4, '0');
        }

        /// <summary>
        /// 计算账单的应收金额。
        /// </summary>
        private void CalculatePaymentAmount()
        {
            if (Data == null)
                return;

            var paymentAmount = Data.TotalAmount - Data.TotalFreeAmount - Data.TotalDebitAmount; //应收=总额-优免-挂账
            paymentAmount = Math.Max(0, paymentAmount);//应收必须大于0
            paymentAmount += Data.TipAmount;//应收需要再加上小费金额。
            var amount = ProcessOdd(paymentAmount, _curOddModel, Globals.OddAccuracy);
            Data.AdjustmentAmount = paymentAmount - amount;
            Data.PaymentAmount = Math.Max(0, amount); //应付金额不能小于0

            if (_curOddModel == EnumOddModel.Rounding)
                RoundingAmount = Math.Abs(Data.AdjustmentAmount);
            else if (_curOddModel == EnumOddModel.Wipe)
                WipeOddAmount = Data.AdjustmentAmount;

            GenerateSettlementInfo();
        }

        /// <summary>
        /// 处理零头。
        /// </summary>
        /// <param name="amount">待处理的金额。</param>
        /// <param name="oddModel">零头处理方式。</param>
        /// <param name="oddAccuracy">零头处理精度。</param>
        /// <returns></returns>
        private decimal ProcessOdd(decimal amount, EnumOddModel oddModel, EnumOddAccuracy oddAccuracy)
        {
            switch (oddModel)
            {
                case EnumOddModel.None:
                    return amount;
                case EnumOddModel.Rounding:
                    switch (oddAccuracy)
                    {
                        case EnumOddAccuracy.Fen:
                            return Math.Round(amount, 1, MidpointRounding.AwayFromZero);
                        case EnumOddAccuracy.Jiao:
                            return Math.Round(amount, 0, MidpointRounding.AwayFromZero);
                        case EnumOddAccuracy.Yuan:
                            return Math.Round(amount / 10, 0, MidpointRounding.AwayFromZero) * 10;
                        default:
                            throw new ArgumentOutOfRangeException("oddAccuracy", oddAccuracy, null);
                    }
                case EnumOddModel.Wipe:
                    switch (oddAccuracy)
                    {
                        case EnumOddAccuracy.Fen:
                            return Math.Floor(amount * 10) / 10;
                        case EnumOddAccuracy.Jiao:
                            return Math.Floor(amount);
                        case EnumOddAccuracy.Yuan:
                            return Math.Floor(amount / 10) * 10;
                        default:
                            throw new ArgumentOutOfRangeException("oddAccuracy", oddAccuracy, null);
                    }
                default:
                    throw new ArgumentOutOfRangeException("oddModel", oddModel, null);
            }
        }

        /// <summary>
        /// 生成收款信息。
        /// </summary>
        private void GenerateSettlementInfo()
        {
            if (Data.HasBeenPaied)
            {
                SettlementInfo = "已结账";
                return;
            }

            if (!_isUserInputCash)//当不是用户输入现金时才更新现金金额。
                CashAmount = Math.Max(0, Data.PaymentAmount - BankAmount - AlipayAmount - IntegralAmount - MemberAmount - WechatAmount - DebitAmount);

            var settlementInfo = new List<string>();

            if (Data.TotalDebitAmount != 0)
                settlementInfo.Add(string.Format("挂账：{0:f2}", Data.TotalDebitAmount));

            if (Data.TotalFreeAmount != 0)
                settlementInfo.Add(string.Format("优免：{0:f2}", Data.TotalFreeAmount));

            if (Data.AdjustmentAmount != 0)
            {
                if (Globals.OddModel == EnumOddModel.Rounding)
                    settlementInfo.Add(string.Format("舍{1}：{0:f2}", Math.Abs(Data.AdjustmentAmount), Data.AdjustmentAmount > 0 ? "去" : "入"));
                else
                    settlementInfo.Add(string.Format("抹零：{0:f2}", Data.AdjustmentAmount));
            }

            //支付部分
            if (CashAmount > 0)
                settlementInfo.Add(string.Format("现金：{0:f2}", CashAmount));

            if (BankAmount > 0)
                settlementInfo.Add(string.Format("银行卡：{0:f2}", BankAmount));

            if (MemberAmount > 0)
                settlementInfo.Add(string.Format("会员消费：{0:f2}", MemberAmount));

            if (IntegralAmount > 0)
                settlementInfo.Add(string.Format("会员积分：{0:f2}", IntegralAmount));

            if (DebitAmount > 0)
                settlementInfo.Add(string.Format("挂账：{0:f2}", DebitAmount));

            if (AlipayAmount > 0)
                settlementInfo.Add(string.Format("支付宝：{0:f2}", AlipayAmount));

            if (WechatAmount > 0)
                settlementInfo.Add(string.Format("微信支付：{0:f2}", WechatAmount));

            Data.TotalAlreadyPayment = CashAmount + BankAmount + MemberAmount + IntegralAmount + DebitAmount + AlipayAmount + WechatAmount; //付款总额=各种付款方式总和。
            var remainderAmount = Data.PaymentAmount - Data.TotalAlreadyPayment;//剩余金额=应付金额-付款总额。
            if (remainderAmount > 0)
            {
                ChargeAmount = 0;
                settlementInfo.Add(Data.TotalAlreadyPayment > 0 ? string.Format("还需再收：{0:f2}", remainderAmount) : string.Format("需收款：{0:f2}", remainderAmount));
            }
            ChargeAmount = Math.Abs(remainderAmount);
            if (ChargeAmount > 0)
                settlementInfo.Add(string.Format("找零：{0:f2}", ChargeAmount));

            //小费当前计算规则：只能从现金扣除，
            if (HasTip && Data.TipAmount > 0)//有小费金额的时候才计算小费实收。
            {
                var realyPayment = Data.PaymentAmount - Data.TipAmount;//真实的应收=明面行应收-小费金额。
                var tipPayment = Data.TotalAlreadyPayment - realyPayment;//小费实付金额=付款总额-真实应收。
                tipPayment = Math.Max(0, tipPayment);//小费实付金额不能小于0。
                tipPayment = Math.Min(CashAmount, tipPayment);//小费实付金额不能大于现金。
                TipPaymentAmount = tipPayment;
            }

            SettlementInfo = string.Format("收款：{0}", string.Join("，", settlementInfo));
        }

        /// <summary>
        /// 添加一个优惠券到使用优惠券列表。
        /// </summary>
        /// <param name="coupon">使用的优惠券。</param>
        /// <param name="usedCount">使用优惠券数量。</param>
        /// <param name="isDiscount">是否是折扣类优惠券。</param>
        /// <param name="freeAmount">单个优惠券优免金额。（只有当折扣类的优惠券才需要传入数值）</param>
        private void AddCouponInfoAsUsed(CouponInfo coupon, int usedCount, bool isDiscount, decimal? freeAmount = null)
        {
            var usedCouponInfo = new UsedCouponInfo
            {
                CouponInfo = coupon,
                IsDiscount = isDiscount,
                Name = coupon.Name,
                Count = usedCount,
                DebitAmount = coupon.DebitAmount * usedCount,
                FreeAmount = freeAmount ?? coupon.FreeAmount ?? 0 * usedCount, //优免金额 = 单个优惠券优免金额 * 数量。(如果单个优惠券优免金额为null，则取优惠券的优免金额，如果还是null，则取0）
            };
            usedCouponInfo.BillAmount = Math.Round((usedCouponInfo.DebitAmount + usedCouponInfo.FreeAmount) * usedCouponInfo.Count * -1, 2);
            AddUsedCouponInfo(usedCouponInfo);
        }

        /// <summary>
        /// 检测账单是否允许结账。
        /// </summary>
        /// <returns>允许结账则返回null，否则返回错误信息。</returns>
        private string CheckTheBillAllowPay()
        {
            if (Data.DishInfos.Any(t => t.DishStatus == EnumDishStatus.ToBeWeighed))
                return "还有未称重菜品。";

            if (Data.TipAmount > 0)//当有小费的时候，应付金额要扣除小费部分，因为允许客户不给小费。
            {
                if (Data.PaymentAmount - Data.TipAmount - Data.TotalAlreadyPayment > 0m)
                    return "还有未收金额。";
            }
            else
            {
                if (Data.PaymentAmount - Data.TotalAlreadyPayment > 0m)
                    return "还有未收金额。";
            }

            if (ChargeAmount > 100)
                return "找零金额不能大于100。";

            if (MemberAmount > 0 && string.IsNullOrEmpty(MemberCardNo))
                return "使用会员储值请先登录会员。";

            if (IntegralAmount > 0 && string.IsNullOrEmpty(MemberCardNo))
                return "使用会员积分请先登录会员。";

            if (DebitAmount > 0 && SelectedOnCmpAccInfo == null)
                return "使用挂账金额请先选择挂账单位。";

            if (AlipayAmount > 0 && string.IsNullOrEmpty(AlipayNo))
                return "使用支付宝支付请输入支付宝账号。";

            if (WechatAmount > 0 && string.IsNullOrEmpty(WechatNo))
                return "使用微信支付请先输入微信账号。";

            return null;
        }

        /// <summary>
        /// 生成账单结算信息集合。
        /// </summary>
        /// <returns></returns>
        private List<BillPayInfo> GenerateBillPayInfos()
        {
            int bankId = SelectedBankInfo != null ? SelectedBankInfo.Id : 0;
            var list = new List<BillPayInfo>
            {
                new BillPayInfo(CashAmount - ChargeAmount, EnumBillPayType.Cash),
                new BillPayInfo(BankAmount, EnumBillPayType.BankCard, BankCardNo, bankId.ToString()),
                new BillPayInfo(MemberAmount, EnumBillPayType.MemberCard, "", MemberCardNo),
                new BillPayInfo(IntegralAmount, EnumBillPayType.MemberIntegral, "", MemberCardNo),
                new BillPayInfo(AlipayAmount, EnumBillPayType.Alipay, AlipayNo),
                new BillPayInfo(WechatAmount, EnumBillPayType.Wechat, WechatNo)
            };

            var onAcc = SelectedOnCmpAccInfo != null ? SelectedOnCmpAccInfo.Name : "";
            var cmpId = SelectedOnCmpAccInfo != null ? SelectedOnCmpAccInfo.Id : "";
            list.Add(new BillPayInfo(DebitAmount, EnumBillPayType.OnCompanyAccount, onAcc) { CouponDetailId = cmpId });

            if (RoundingAmount > 0)
                list.Add(new BillPayInfo(RoundingAmount, EnumBillPayType.Rounding));
            if (WipeOddAmount > 0)
                list.Add(new BillPayInfo(WipeOddAmount, EnumBillPayType.RemoveOdd));

            foreach (var usedCouponInfo in Data.UsedCouponInfos)
            {
                //雅座会员的优惠券需要特殊处理。

                if (usedCouponInfo.FreeAmount > 0)
                {
                    var payInfo = new BillPayInfo(usedCouponInfo.FreeAmount, EnumBillPayType.FreeAmount, usedCouponInfo.Name, usedCouponInfo.CouponInfo.PartnerName)
                    {
                        CouponNum = usedCouponInfo.Count,
                        CouponId = usedCouponInfo.CouponInfo.CouponId,
                        CouponDetailId = usedCouponInfo.CouponInfo.RuleId
                    };
                    list.Add(payInfo);
                }
                if (usedCouponInfo.DebitAmount > 0)
                {
                    var payInfo = new BillPayInfo(usedCouponInfo.DebitAmount, EnumBillPayType.OnAccount, usedCouponInfo.Name, usedCouponInfo.CouponInfo.PartnerName)
                    {
                        CouponNum = usedCouponInfo.Count,
                        CouponId = usedCouponInfo.CouponInfo.CouponId,
                        CouponDetailId = usedCouponInfo.CouponInfo.RuleId
                    };
                    list.Add(payInfo);
                }
            }

            return list;
        }

        /// <summary>
        /// 添加一个优惠信息。
        /// </summary>
        /// <param name="item">添加的优惠券信息。</param>
        private void AddUsedCouponInfo(UsedCouponInfo item)
        {
            if (item == null || Data == null)
                return;

            Data.UsedCouponInfos.Add(item);
            Data.TotalDebitAmount = Data.UsedCouponInfos.Sum(t => t.DebitAmount * t.Count);
            Data.TotalFreeAmount = Data.UsedCouponInfos.Sum(t => t.FreeAmount * t.Count);

            CalculatePaymentAmount(); //优惠券添加完毕后计算实收。
        }

        /// <summary>
        /// 移除一个优惠券。
        /// </summary>
        /// <param name="item">移除的优惠券信息。</param>
        private void RemoveUsedCouponInfo(UsedCouponInfo item)
        {
            if (item == null || Data == null)
                return;

            Data.UsedCouponInfos.Remove(item);
            Data.TotalDebitAmount = Data.UsedCouponInfos.Sum(t => t.DebitAmount * t.Count);
            Data.TotalFreeAmount = Data.UsedCouponInfos.Sum(t => t.FreeAmount * t.Count);

            CalculatePaymentAmount(); //优惠券添加完毕后计算实收。
        }

        /// <summary>
        /// 清除优惠券。
        /// </summary>
        /// <param name="Data"></param>
        private void ClearUsedCouponInfo()
        {
            if (Data == null)
                return;

            Data.UsedCouponInfos.Clear();
            Data.TotalDebitAmount = 0m;
            Data.TotalFreeAmount = 0m;

            CalculatePaymentAmount(); //优惠券添加完毕后计算实收。
        }

        /// <summary>
        /// 结算后的一些处理。
        /// </summary>
        protected virtual void DosomethingAfterSettlement()
        {

        }

        #endregion
    }
}