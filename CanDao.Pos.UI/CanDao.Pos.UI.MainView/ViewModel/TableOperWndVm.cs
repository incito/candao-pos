using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.Common.PublicValues;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Event;
using CanDao.Pos.Model.Request;
using CanDao.Pos.ReportPrint;
using CanDao.Pos.UI.MainView.View;
using CanDao.Pos.UI.Utility;
using CanDao.Pos.UI.Utility.View;
using CanDao.Pos.UI.Utility.ViewModel;
using Timer = System.Timers.Timer;
using CanDao.Pos.Model.Response;

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
        protected Timer _couponLongPressTimer;

        /// <summary>
        /// 长按定时器的触发间隔（1秒）
        /// </summary>
        private const int LongPressTimerSecond = 1;

        /// <summary>
        /// 是否是长按优惠券处理模式，当是该模式时，优惠券弹起事件里不处理。
        /// </summary>
        private bool _isLongPressModel;

        /// <summary>
        /// 零头是否保留枚举。
        /// </summary>
        private EnumKeepOdd _isKeepOdd;

        /// <summary>
        /// 刷新定时器。
        /// </summary>
        protected Timer _refreshTimer;

        /// <summary>
        /// 刷新定时器间隔（秒）。
        /// </summary>
        private const int RefreshTimerSecond = 10;

        /// <summary>
        /// 雅座优惠券集合。
        /// </summary>
        private List<CouponInfo> _yaZuoMemberCouponInfos;

        /// <summary>
        /// 是否是已经释放资源的状态。
        /// </summary>
        protected bool _isDisposed;

        #endregion

        #region Constructor

        public TableOperWndVm(TableInfo tableInfo)
        {
            _isKeepOdd = EnumKeepOdd.Process;
            _tableInfo = tableInfo;
            InitCouponCategories();
            InitCouponLongPressTimer();
            InitRefreshTimer();

            Data = new TableFullInfo();
            Data.CloneDataFromTableInfo(tableInfo);

            PayWayInfos = new ObservableCollection<PayWayInfo>();
            TaskService.Start(null, GetPayWayInfoProcess, GetPayWayInfoComplete, null);
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
        /// 支付方式集合。
        /// </summary>
        public ObservableCollection<PayWayInfo> PayWayInfos { get; set; }

        /// <summary>
        /// 选择的结算方式。
        /// </summary>
        public PayWayInfo SelectedPayWayInfo { get; set; }

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
                if (_selectedCouponCategory.CategoryType == "88" && Globals.IsYazuoMember)//雅座会员。
                {
                    if (_yaZuoMemberCouponInfos != null)
                        _yaZuoMemberCouponInfos.ForEach(CouponInfos.Add);
                }
                else
                {
                    InfoLog.Instance.I("开始获取\"{0}\"优惠券...", value.CategoryName);
                    TaskService.Start(value.CategoryType, GetCouponCategoriesProcess, GetCouponCategoriesComplete);
                }
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
        /// 是否有小费。（外卖不支持小费）
        /// </summary>
        protected bool HasTip { get; set; }

        #region 结算方式

        /// <summary>
        /// 现金结算方式。
        /// </summary>
        private PayWayCashInfo _cashPayWay;

        /// <summary>
        /// 银行卡结算方式。
        /// </summary>
        private PayWayBankInfo _bankPayWay;

        /// <summary>
        /// 会员结算方式。
        /// </summary>
        private PayWayMemberInfo _memberPayWay;

        /// <summary>
        /// 挂账单位结算方式。
        /// </summary>
        private PayWayOnCmpAccount _onCmpAccountPayWay;

        /// <summary>
        /// 获取现金金额。
        /// </summary>
        public decimal CashAmount
        {
            get { return _cashPayWay != null ? _cashPayWay.Amount : 0m; }
        }

        /// <summary>
        /// 获取找零金额。
        /// </summary>
        public decimal ChargeAmount
        {
            get { return _cashPayWay != null ? _cashPayWay.ChargeAmount : 0m; }
        }

        /// <summary>
        /// 获取会员积分金额。
        /// </summary>
        public decimal IntegralAmount
        {
            get { return _memberPayWay != null ? _memberPayWay.IntegralAmount : 0m; }
        }

        /// <summary>
        /// 获取会员储值消费金额。
        /// </summary>
        public decimal MemberAmount
        {
            get { return _memberPayWay != null ? _memberPayWay.Amount : 0m; }
        }

        /// <summary>
        /// 获取会员号。
        /// </summary>
        public string MemberNo
        {
            get { return _memberPayWay != null ? _memberPayWay.Remark : Data.MemberNo; }
        }

        #endregion

        #endregion

        #region Command

        /// <summary>
        /// 菜单列表操作命令。
        /// </summary>
        public ICommand DataGridPageOperCmd { get; private set; }

        /// <summary>
        /// 打印命令。
        /// </summary>
        public ICommand PrintCmd { get; private set; }

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

        #endregion

        #region Command Methods

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
                case "PrePayWayGroup":
                    wnd.PayWayGroup.PreviousGroup();
                    break;
                case "NextPayWayGroup":
                    wnd.PayWayGroup.NextGroup();
                    break;
            }
        }

        /// <summary>
        /// 菜单列表操作命令是否可用的判断方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
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
                case "PrePayWayGroup":
                    return wnd.PayWayGroup.CanPreviousGroup;
                case "NextPayWayGroup":
                    return wnd.PayWayGroup.CanNextGruop;
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
            var print = new ReportPrintHelper2(OwnerWindow);
            switch (param as string)
            {
                case "PreSettlement":
                    IsPrintMoreOpened = false;
                    InfoLog.Instance.I("开始重印餐台\"{0}\"的预结单...", Data.TableName);
                    print.PrintPresettlementReport(Data.OrderId, Globals.UserInfo.UserName);
                    break;
                case "ReprintCustomUseBill":
                    IsPrintMoreOpened = false;
                    InfoLog.Instance.I("开始重印餐台\"{0}\"的客用单...", Data.TableName);
                    print.PrintCustomUseBillReport(Data.OrderId, Globals.UserInfo.UserName);
                    InfoLog.Instance.I("结束重印餐台\"{0}\"的客用单。", Data.TableName);
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
                case "ReprintCustomUseBill":
                    enable = Data.DishInfos.Any();
                    break;
            }
            return enable;
        }

        /// <summary>
        /// 优惠券鼠标按下命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void CouponMouseDown(object arg)
        {
            var coupon = (CouponInfo)((CouponInfo)arg).Clone();
            _curSelectedCouponInfo = coupon;

            _isLongPressModel = false;
            SetCouponPressTimerStatus(true);
        }

        /// <summary>
        /// 优惠券鼠标弹起命令的执行方法。
        /// </summary>
        private void CouponMouseUp(object arg)
        {
            SetCouponPressTimerStatus(false);
            if (_isLongPressModel)
                return;

            if (!Data.DishInfos.Any())
            {
                MessageDialog.Warning("还未下单，不能使用优惠。", OwnerWindow);
                return;
            }

            if (Data.TotalAmount == 0)
            {
                MessageDialog.Warning("账单金额为0，不能使用优惠。", OwnerWindow);
                return;
            }

            if (_curSelectedCouponInfo == null)
                return;

            if (_curSelectedCouponInfo.CouponType == EnumCouponType.HandFree) //手工优惠类特殊处理。
            {
                CouponHandFreeProcess();
            }
            else if (_curSelectedCouponInfo.IsDiscount) //折扣类处理流程。
            {
                if (!MessageDialog.Quest(string.Format("确定使用{0}？", _curSelectedCouponInfo.Name)))
                    return;

                InfoLog.Instance.I("开始折扣类优惠券：{0}计算接口。", _curSelectedCouponInfo.Name);

                CreatUsePreferentialRequest(1, 0, 0, 0);

                //TaskService.Start(0m, CalcDiscountAmountProcess, CalcDiscountAmountComplete, "计算折扣金额中...");//优惠券折扣时，自定义折扣为0。
            }
            else
            {
                if (_curSelectedCouponInfo.BillAmount == 999999) //特殊优惠券。
                {
                    var amountSelectWnd = new OfferTypeSelectWindow(EnumOfferType.Amount);
                    if (!WindowHelper.ShowDialog(amountSelectWnd, OwnerWindow))
                        return;

                    var debitAmount = amountSelectWnd.Amount;//挂账金额、
                    if (debitAmount > Data.PaymentAmount)
                    {
                        MessageDialog.Warning(string.Format("挂账金额不能大于剩余应收金额（{0}）。", Data.PaymentAmount));
                        return;
                    }

                    CreatUsePreferentialRequest(1, 0, debitAmount, 1);
                }
                else
                {
                    if (_curSelectedCouponInfo.FreeAmount > 0 || _curSelectedCouponInfo.DebitAmount > 0)
                    {
                        //选择数量的窗口
                        var maxNum = 0;
                        var preferentialAmout = 0m;
                        if (_curSelectedCouponInfo.CouponType == EnumCouponType.YaZuoFree)
                        {
                            maxNum = _curSelectedCouponInfo.MaxCouponCount;//雅座优惠券的时候，最大数值为雅座优惠券的数量。
                            preferentialAmout = _curSelectedCouponInfo.FreeAmount ?? 0;//雅座优惠券的时候，优免金额为优惠券的优免金额。
                        }
                        var numWnd = new NumInputWindow("优惠券", _curSelectedCouponInfo.Name, "使用数量", maxNum, false);
                        if (!WindowHelper.ShowDialog(numWnd, OwnerWindow))
                            return;

                        CreatUsePreferentialRequest(numWnd.InputNum, 0, preferentialAmout, 0);
                    }
                    else if (_curSelectedCouponInfo.BillAmount <= 0 && _curSelectedCouponInfo.DebitAmount <= 0)
                    {
                        //如果都是0，则弹出窗口输入金额
                        var offSelectWnd = new OfferTypeSelectWindow(EnumOfferType.Both);
                        if (!WindowHelper.ShowDialog(offSelectWnd, OwnerWindow))
                            return;

                        if (offSelectWnd.SelectedOfferType == EnumOfferType.Amount)
                        {
                            if (offSelectWnd.Amount > Data.PaymentAmount)
                            {
                                MessageDialog.Warning("优免金额不能大于应收金额。");
                                return;
                            }

                            CreatUsePreferentialRequest(1, 0, offSelectWnd.Amount, 1);
                        }
                        else if (offSelectWnd.SelectedOfferType == EnumOfferType.Discount)
                        {
                            CreatUsePreferentialRequest(1, offSelectWnd.Discount, 0, 1);
                        }
                    }
                }
            }

            //SaveCouponInfo();
        }

        /// <summary>
        /// 现金控件获取或失去焦点命令时执行。
        /// </summary>
        /// <param name="param"></param>
        private void CashControlFocus(object param)
        {
            _isUserInputCash = Convert.ToBoolean(param);
        }

        #region 优惠券处理

        /// <summary>
        /// 创建使用优惠券参数
        /// </summary>
        /// <param name="num">使用券数量</param>
        /// <param name="disRate">手工折扣额</param>
        /// <param name="preferentialAmout">手工优惠金额</param>
        /// <param name="isCustom">是否收银员输入（1：是：0不是）</param>
        /// <returns></returns>
        private void CreatUsePreferentialRequest(decimal num, decimal disRate, decimal preferentialAmout, int isCustom)
        {
            var usePreferential = new UsePreferentialRequest
            {
                orderid = _tableInfo.OrderId,
                preferentialid = _curSelectedCouponInfo.CouponId,
                type = _curSelectedCouponInfo.CouponType.GetHashCode().ToString(),
                preferentialAmt = Data.CouponAmount.ToString(),
                adjAmout = Data.AdjustmentAmount.ToString(),
                toalDebitAmount = Data.TotalDebitAmount.ToString(),
                toalFreeAmount = Data.TotalFreeAmount.ToString(),
                toalDebitAmountMany = Data.ToalDebitAmountMany.ToString(),
                preferentialNum = num.ToString(),
                preferentialName = _curSelectedCouponInfo.Name,
                dishid = "",
                isCustom = isCustom.ToString(),
                disrate = disRate,
                preferentialAmout = preferentialAmout.ToString()
            };

            if (_curSelectedCouponInfo.HandCouponType != null)
                usePreferential.sub_type = ((int)_curSelectedCouponInfo.HandCouponType).ToString();

            TaskService.Start(usePreferential, UseCouponProcess, UseCouponInfoComplete, "保存使用优惠券...");
        }

        private void CreatUsePreferentialRequest(List<GiftDishInfo> giftDishInfos)
        {
            var dishNumString = string.Join(",", giftDishInfos.Select(t => t.SelectGiftNum.ToString()));
            var dishIdString = string.Join(",", giftDishInfos.Select(t => t.DishId));
            var dishUnitString = string.Join(",", giftDishInfos.Select(t => t.DishUnit));

            var usePreferential = new UsePreferentialRequest
            {
                orderid = _tableInfo.OrderId,
                preferentialid = _curSelectedCouponInfo.CouponId,
                type = _curSelectedCouponInfo.CouponType.GetHashCode().ToString(),
                preferentialAmt = Data.CouponAmount.ToString(),
                adjAmout = Data.AdjustmentAmount.ToString(),
                toalDebitAmount = Data.TotalDebitAmount.ToString(),
                toalFreeAmount = Data.TotalFreeAmount.ToString(),
                toalDebitAmountMany = Data.ToalDebitAmountMany.ToString(),
                preferentialNum = dishNumString,
                preferentialName = _curSelectedCouponInfo.Name,
                dishid = dishIdString,
                unit = dishUnitString,
                isCustom = "0",
                disrate = 0,
                preferentialAmout = "0",
            };

            if (_curSelectedCouponInfo.HandCouponType != null)
                usePreferential.sub_type = ((int)_curSelectedCouponInfo.HandCouponType).ToString();

            TaskService.Start(usePreferential, UseCouponProcess, UseCouponInfoComplete, "保存使用优惠券...");
        }

        /// <summary>
        /// 保存使用优惠券处理
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object UseCouponProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return new Tuple<string, PreferentialInfoResponse>("创建IOrderService服务失败。", null);

            return service.UsePreferential((UsePreferentialRequest)arg);
        }

        /// <summary>
        /// 使用优惠券完成
        /// </summary>
        /// <param name="obj"></param>
        private void UseCouponInfoComplete(object obj)
        {
            var result = (Tuple<string, TableAfterUseCouponInfo>)obj;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var errMsg = string.Format("使用优惠券失败：{0}", result.Item1);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg, OwnerWindow);
                return;
            }

            ProcessCouponShow(result.Item2);
            InfoLog.Instance.I("结束使用优惠券流程。");
        }

        /// <summary>
        /// 手工优免类优惠券处理方式。
        /// </summary>
        private void CouponHandFreeProcess()
        {
            switch (_curSelectedCouponInfo.HandCouponType)
            {
                case EnumHandCouponType.FreeDish:
                    TaskService.Start(null, GetDishGiftCouponInfoProcess, GetDishGiftCouponInfoComplete, "获取赠菜优惠券使用信息...");
                    break;
                case EnumHandCouponType.Discount:
                    InfoLog.Instance.I("选择手工优惠折扣类优惠券，弹出折扣输入窗口...");
                    var diacountSelectWnd = new OfferTypeSelectWindow(EnumOfferType.Discount);
                    if (WindowHelper.ShowDialog(diacountSelectWnd, OwnerWindow))
                    {
                        InfoLog.Instance.I("选择折扣率：{0}，开始调用接口计算折扣金额...", diacountSelectWnd.Discount);
                        CreatUsePreferentialRequest(1, diacountSelectWnd.Discount, 0, 1);

                        //TaskService.Start(diacountSelectWnd.Discount, CalcDiscountAmountProcess, CalcDiscountAmountComplete, "计算折扣金额中...");
                    }
                    break;
                case EnumHandCouponType.Amount:
                    InfoLog.Instance.I("选择手工优惠优免类优惠券，弹出优免金额输入窗口...");
                    var amountSelectWnd = new OfferTypeSelectWindow(EnumOfferType.Amount);
                    if (WindowHelper.ShowDialog(amountSelectWnd, OwnerWindow))
                    {
                        CreatUsePreferentialRequest(1, 0, amountSelectWnd.Amount, 1);

                        //AddCouponInfoAsUsed(_curSelectedCouponInfo, 1, false, amountSelectWnd.Amount);
                    }
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 获取赠菜优惠券使用信息执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object GetDishGiftCouponInfoProcess(object param)
        {
            InfoLog.Instance.I("选择手工优惠赠菜类优惠券，先获取赠菜优惠券使用信息...");
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return new Tuple<string, List<DishGiftCouponInfo>>("创建IOrderService服务失败。", null);

            return service.GetDishGiftCouponInfo(Data.OrderId);
        }

        /// <summary>
        /// 获取赠菜优惠券使用信息执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void GetDishGiftCouponInfoComplete(object param)
        {
            var result = (Tuple<string, List<DishGiftCouponInfo>>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var msg = string.Format("获取赠菜优惠券使用信息时错误：{0}", result.Item1);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg);
                return;
            }

            InfoLog.Instance.I("赠菜优惠券使用信息获取完成，弹出赠菜选择窗口...");
            var giftDishWnd = new SelectGiftDishWndVm(Data.DishInfos.ToList(), result.Item2);
            if (WindowHelper.ShowDialog(giftDishWnd, OwnerWindow))
            {
                if (giftDishWnd.SelectedGiftDishInfos.Any())
                    CreatUsePreferentialRequest(giftDishWnd.SelectedGiftDishInfos);
            }
        }

        /// <summary>
        /// 删除优惠券处理
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object DeleCouponInfoProcess(object arg)
        {
            IOrderService service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return new Tuple<string, PreferentialInfoResponse>("创建IOrderService服务失败。", null);

            return service.DelPreferential(_tableInfo.OrderId, arg as string);
        }

        /// <summary>
        /// 删除优惠券完成
        /// </summary>
        /// <param name="obj"></param>
        private void DeleCouponInfoComplete(object obj)
        {
            var result = (Tuple<string, TableAfterUseCouponInfo>)obj;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var errMsg = string.Format("删除优惠券信息失败：{0}", result);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg, OwnerWindow);
                return;
            }

            Data.UsedCouponInfos.Clear();
            ProcessCouponShow(result.Item2);
            InfoLog.Instance.I("结束删除优惠券。");
        }

        /// <summary>
        /// 删除雅座优惠券的执行方法。（当退出雅座会员时如果有使用的雅座优惠券则删除之）
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object DeleYaZuoCouponInfoProcess(object arg)
        {
            IOrderService service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            var idList = (List<string>)arg;
            idList.ForEach(t => service.DelPreferential(_tableInfo.OrderId, t));
            return null;
        }

        /// <summary>
        /// 删除雅座优惠券信息后，重新拉取账单信息。
        /// </summary>
        /// <param name="arg"></param>
        private void DeleYaZuoCouponInfoComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                var errMsg = string.Format("删除优惠券信息失败：{0}", result);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg);
            }

            GetTableDishInfoAsync();
        }

        /// <summary>
        /// 移除一个优惠券。
        /// </summary>
        /// <param name="item">移除的优惠券信息。</param>
        private void RemoveUsedCouponInfo(UsedCouponInfo item)
        {
            TaskService.Start(item.RelationId, DeleCouponInfoProcess, DeleCouponInfoComplete, "正在删除优惠券...");
        }

        /// <summary>
        /// 清除优惠券。
        /// </summary>
        /// <param name="Data"></param>
        private void ClearUsedCouponInfo()
        {
            TaskService.Start(null, DeleCouponInfoProcess, DeleCouponInfoComplete, "正在清空优惠券...");
        }

        /// <summary>
        /// 处理优惠券的相关数据。
        /// </summary>
        /// <param name="info">优惠券返回信息。</param>
        private void ProcessCouponShow(TableAfterUseCouponInfo info)
        {
            Data.CloneFromTableServiceChargePart(info);
            GenerateSettlementInfo();
        }

        #endregion

        #endregion

        #region Protected Method

        protected override void InitCommand()
        {
            base.InitCommand();
            DataGridPageOperCmd = CreateDelegateCommand(DataGridPageOper, CanDataGridPageOper);
            PrintCmd = CreateDelegateCommand(Print, CanPrint);
            CashControlFocusCmd = CreateDelegateCommand(CashControlFocus);
            CouponMouseDownCmd = CreateDelegateCommand(CouponMouseDown);
            CouponMouseUpCmd = CreateDelegateCommand(CouponMouseUp);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs arg)
        {
            if (arg.Key == Key.Enter)
            {
                arg.Handled = true;
                PayTheBill();
            }
        }

        protected override void OperMethod(object param)
        {
            switch (param as string)
            {
                case "More":
                    IsPrintMoreOpened = !IsPrintMoreOpened;
                    break;
                case "CancelOrder":
                    CancelOrder();
                    break;
                case "Resettlement":
                    ResettlementSync();
                    break;
                case "Order":
                    OrderDish();
                    break;
                case "OpenCashBox":
                    OpenCashBoxAsync();
                    break;
                case "KeepOdd":
                    _isKeepOdd = EnumKeepOdd.None;
                    GetTableDishInfoAsync();
                    break;
                case "BackDish":
                    BackDish();
                    break;
                case "DishWeight":
                    DishWeight();
                    break;
                case "SelectBank":
                    InfoLog.Instance.I("开始选择银行...");
                    var selectBankWnd = new SelectBankWndVm(_bankPayWay.SelectedBankInfo);
                    if (WindowHelper.ShowDialog(selectBankWnd, OwnerWindow))
                        _bankPayWay.SelectedBankInfo = selectBankWnd.SelectedBank;
                    break;
                case "SelectOnAccountCompany":
                    InfoLog.Instance.I("开始获取挂账单位...");
                    var companyWnd = new OnAccountCompanySelectWndVm();
                    if (WindowHelper.ShowDialog(companyWnd, OwnerWindow))
                        _onCmpAccountPayWay.SelectedOnCmpAccInfo = companyWnd.SelectedCompany;
                    break;
                case "MemberLogin":
                    InfoLog.Instance.I("会员登录按钮点击...");
                    MemberLogin();
                    break;
                case "MemberLogout":
                    InfoLog.Instance.I("会员登出按钮点击...");
                    WorkFlowService.Start(null, new WorkFlowInfo(MemberLogoutProcess, MemberLogoutComplete, "会员登出中..."));
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
                    BackAllOrderDish();
                    break;
                case "ServiceCharge":
                    if (!WindowHelper.ShowDialog(new AuthorizationWndVm(EnumRightType.AntiSettlement), OwnerWindow))
                        return;

                    WindowHelper.ShowDialog(new ServiceChargeSettingWndVm(Data.ServiceChargeInfo));
                    GetTableDishInfoAsync();
                    break;
            }
        }

        protected override bool CanOperMethod(object param)
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
                    return _memberPayWay != null && !string.IsNullOrEmpty(_memberPayWay.Remark) && _memberPayWay.Remark.Length > 0;
                case "CouponRemove":
                    return SelectedUsedCouponInfo != null;
                case "CouponClear":
                    return Data != null && Data.UsedCouponInfos.Any();
                case "BackAllDish":
                    return Data != null && Data.OrderStatus == EnumOrderStatus.Ordered && Data.DishInfos.Any();
                case "BackDish":
                    return SelectedOrderDish != null;
                case "DishWeight":
                    return SelectedOrderDish != null && SelectedOrderDish.DishStatus == EnumDishStatus.ToBeWeighed;
                case "ServiceCharge":
                    return Data.ServiceChargeInfo != null;
                default:
                    return true;
            }
        }

        protected void SetRefreshTimerStatus(bool enabled)
        {
            if (_isDisposed)
                return;

            _refreshTimer.Enabled = enabled;
            InfoLog.Instance.I("设置结账页面刷新定时器状态：{0}", enabled ? "启用" : "停用");
        }

        protected void SetCouponPressTimerStatus(bool enabled)
        {
            if (_isDisposed)
                return;

            _couponLongPressTimer.Enabled = enabled;
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
        /// 初始化刷新定时器。
        /// </summary>
        private void InitRefreshTimer()
        {
            _refreshTimer = new Timer(RefreshTimerSecond * 1000);
            _refreshTimer.Elapsed += RefreshTimerOnElapsed;
        }

        /// <summary>
        /// 刷新定时器触发时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void RefreshTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            SetRefreshTimerStatus(false);
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
            {
                ErrLog.Instance.E("创建IOrderService服务失败。");
                SetRefreshTimerStatus(true);
                return;
            }

            var result = service.GetOrderInfo(Data.OrderId, "", ((int)_isKeepOdd));
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取餐台信息错误：{0}。", result.Item1);
                SetRefreshTimerStatus(true);
                return;
            }

            if (result.Item2 == null)
            {
                ErrLog.Instance.E("获取到餐台信息为空。");
                SetRefreshTimerStatus(true);
                return;
            }

            OwnerWindow.Dispatcher.BeginInvoke((Action)delegate { Data.CloneSimpleData(result.Item2); });
            if (Data.TotalAmount != result.Item2.TotalAmount || Data.DishInfos.Sum(t => t.DishNum) != result.Item2.DishInfos.Sum(t => t.DishNum)) //当总价或菜品数量改变时再触发刷新方法。
            {
                _tableInfo.OrderId = result.Item2.OrderId; //可能会有并台导致订单号改变。
                OwnerWindow.Dispatcher.BeginInvoke((Action)GetTableDishInfoAsync);
            }
            else
            {
                SetRefreshTimerStatus(true);
            }
        }

        /// <summary>
        /// 长按定时器触发时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void CouponLongPressTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            SetCouponPressTimerStatus(false);
            _isLongPressModel = true;
            OwnerWindow.Dispatcher.BeginInvoke((Action)delegate
            {
                var msg = string.Format(_curSelectedCouponInfo.IsUncommonlyUsed ? "恢复\"{0}\"为常用优惠{1}（恢复后可在对应分类查看、使用）" : "设置\"{0}\"为不常用优惠{1}（设置后可在不常用优惠分类查看、使用）", _curSelectedCouponInfo.Name, Environment.NewLine);

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

                msg = string.Format(_curSelectedCouponInfo.IsUncommonlyUsed ? "恢复\"{0}\"为常用优惠成功。" : "设置\"{0}\"为不常用优惠成功。", _curSelectedCouponInfo.Name, Environment.NewLine);
                InfoLog.Instance.I(msg);
                NotifyDialog.Notify(msg, OwnerWindow);
                SelectedCouponCategory = SelectedCouponCategory;//触发优惠券的重新获取。
            });
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
                var tipAmount = _cashPayWay != null ? _cashPayWay.TipPaymentAmount : 0m;
                if (tipAmount < Data.TipAmount)
                {
                    if (Data.TotalAlreadyPayment - Data.PaymentAmount + Data.TipAmount > tipAmount)//总共支付金额计算出的小费超过约定规则计算后的小费金额，说明小费金额部分不是由现金支付的，不允许结账。
                    {
                        MessageDialog.Warning(string.Format("小费{0}元，必须使用现金结算。", Data.TipAmount), OwnerWindow);
                        return;
                    }

                    if (!MessageDialog.Quest(string.Format("还有{0}元小费未结算，点击确定继续结算，点击取消取消结算。", Data.TipAmount - tipAmount), OwnerWindow))
                        return;
                }
            }

            if (!MessageDialog.Quest(string.Format("桌号：{0} 确定现在结算吗？", Data.TableName), OwnerWindow))
                return;

            var param = new Tuple<string, string, List<BillPayInfo>>(Data.OrderId, Globals.UserInfo.UserName, GenerateBillPayInfos());
            InfoLog.Instance.I("开始账单结算...");

            var payBillWorkFlow = new WorkFlowInfo(PayTheBillProcess, PayTheBillComplete, "账单结算中...");
            var curStepWf = payBillWorkFlow;

            if (HasTip && Data.TipAmount > 0)
            {
                var tipSettlementWf = new WorkFlowInfo(TipSettlementProcess, TipSettlementComplete, "小费结算中...");
                curStepWf.NextWorkFlowInfo = tipSettlementWf;
                curStepWf = tipSettlementWf;
            }

            if (!string.IsNullOrEmpty(Data.MemberNo))
            {
                var helper = new AntiSettlementHelper();
                var antiSettlementWf = helper.GetAntiSettlement(Data.OrderId, Data.MemberNo, OwnerWindow);
                antiSettlementWf.ErrorWorkFlowInfo = new WorkFlowInfo(null, AfterAntisettlementFailed);

                if (_memberPayWay != null && !_memberPayWay.IsMemberLogin)
                {
                    var queryMemberWf = new WorkFlowInfo(QueryMemberProcess, QueryMemberComplete, "会员查询中...") { ErrorWorkFlowInfo = antiSettlementWf };//会员查询错误时执行自动反结算工作流。
                    curStepWf.NextWorkFlowInfo = queryMemberWf;
                    curStepWf = queryMemberWf;
                }

                var saleMemberWf = GenerateMemberSaleWf();
                curStepWf.NextWorkFlowInfo = saleMemberWf;
                curStepWf = saleMemberWf;

                curStepWf.ErrorWorkFlowInfo = antiSettlementWf;//会员消费结算错误时执行自动反结算工作流。
            }

            //屏蔽功能，由后台进行操作。
            //var jdeDebitAmountWf = new WorkFlowInfo(JdeDebitAmountProcess, JdeDebitAmountComplete, "计算实收执行中...");
            //curStepWf.NextWorkFlowInfo = jdeDebitAmountWf;//结算的最后一个执行步骤为调用JDE计算实收。

            curStepWf.NextWorkFlowInfo = new WorkFlowInfo(null, PrintSettlementReportAndInvoice);//打印和开发票

            WorkFlowService.Start(param, payBillWorkFlow);
        }

        /// <summary>
        /// 会员登录。
        /// </summary>
        private void MemberLogin()
        {
            var queryMemberWf = new WorkFlowInfo(QueryMemberProcess, QueryMemberComplete2, "会员查询中..."); ;
            queryMemberWf.NextWorkFlowInfo = new WorkFlowInfo(MemberLoginProcess, MemberLoginComplete, "会员登录中...");
            WorkFlowService.Start(null, queryMemberWf);
        }

        /// <summary>
        /// 点菜。
        /// </summary>
        protected void OrderDish()
        {
            var orderWnd = new OrderDishWindow(Data);
            if (WindowHelper.ShowDialog(orderWnd, OwnerWindow))
            {
                if (orderWnd.IsOrderHanged)
                    CloseWindow(true);
                else
                    GetTableDishInfoAsync();
            }
        }

        /// <summary>
        /// 退菜执行方法。
        /// </summary>
        private void BackDish()
        {
            var selectedDish = SelectedOrderDish;//这里做一个临时变量，是为了解决定时器刷新时更新了菜品列表，导致当前选择的菜品改变的问题。
            if (selectedDish == null)
                return;

            if (selectedDish.IsComboDish)
            {
                MessageDialog.Warning("请选择套餐主体退整个套餐。");
                return;
            }

            if (selectedDish.IsFishPotDish && selectedDish.IsPot) //选中了鱼锅的锅。
            {
                if (!MessageDialog.Quest("选择鱼锅锅底退菜会退掉整个鱼锅，确定继续退菜？"))
                    return;

                InfoLog.Instance.I("选择了鱼锅锅底退菜，已经操作确认，即将退整个鱼锅...");
            }

            var backDishReasonWnd = new BackDishReasonSelectWindow();
            if (!WindowHelper.ShowDialog(backDishReasonWnd, OwnerWindow))
                return;

            InfoLog.Instance.I("选择退菜原因：\"{0}\"。", backDishReasonWnd.SelectedReason);
            bool allowInputBackNum = !(selectedDish.IsPot || (selectedDish.IsMaster && selectedDish.DishType == EnumDishType.FishPot));//鱼锅和套餐， 那不用输入数量

            var backDishNum = selectedDish.DishNum;
            if (allowInputBackNum)
            {
                var numWnd = new NumInputWindow("退菜", selectedDish.DishName, "退菜数量：", selectedDish.DishNum);
                if (!WindowHelper.ShowDialog(numWnd, OwnerWindow))
                    return;

                backDishNum = numWnd.InputNum;
                InfoLog.Instance.I("输入退菜数量：\"{0}\"。", numWnd.InputNum);
                if (selectedDish.DishType == EnumDishType.Packages && backDishNum != (int)backDishNum)
                {
                    MessageDialog.Warning("套餐只接受整数数量的退菜。");
                    return;
                }
            }

            if (!WindowHelper.ShowDialog(new AuthorizationWndVm(EnumRightType.BackDish), OwnerWindow))
                return;

            InfoLog.Instance.I("退菜授权人：\"{0}\"", Globals.Authorizer.UserName);
            var param = new BackDishComboInfo
            {
                AuthorizerUser = Globals.Authorizer.UserName,
                BackDishNum = backDishNum,
                BackDishReason = backDishReasonWnd.SelectedReason,
                DishInfo = selectedDish,
                OrderId = Data.OrderId,
                TableNo = Data.TableNo,
                Waiter = Globals.UserInfo.UserName
            };

            TaskService.Start(param, BackDishProess, BackDishComplete, "退菜处理中，请稍后...");
        }

        /// <summary>
        /// 整单退菜。
        /// </summary>
        private void BackAllOrderDish()
        {
            var backDishReasonWnd = new BackDishReasonSelectWindow();
            if (!WindowHelper.ShowDialog(backDishReasonWnd, OwnerWindow))
                return;

            if (WindowHelper.ShowDialog(new AuthorizationWndVm(EnumRightType.BackDish), OwnerWindow))
                WorkFlowService.Start(backDishReasonWnd.SelectedReason, GenerateBackAllDishWf());
        }

        /// <summary>
        /// 菜品称重。
        /// </summary>
        /// <returns></returns>
        private void DishWeight()
        {
            if (SelectedOrderDish.DishStatus != EnumDishStatus.ToBeWeighed)
                return;

            InfoLog.Instance.I("选中的菜时称重菜品，弹出称重窗体...");
            var dishWeightWnd = new NumInputWindow("称重：", SelectedOrderDish.DishName, "称重数量：", 0);
            if (WindowHelper.ShowDialog(dishWeightWnd, OwnerWindow))
            {
                InfoLog.Instance.I("菜品\"{0}\"称重数量：{1}", SelectedOrderDish.DishName, dishWeightWnd.InputNum);
                var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
                if (service == null)
                {
                    ErrLog.Instance.E("创建IOrderService服务失败。");
                    MessageDialog.Warning("创建IOrderService服务失败。");
                    return;
                }

                InfoLog.Instance.I("开始调用菜品称重接口...");
                var result = service.UpdateDishWeight(Data.OrderId, SelectedOrderDish.DishId, SelectedOrderDish.PrimaryKey, dishWeightWnd.InputNum);
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

            var helper = new AntiSettlementHelper();
            var afterAntiSettlementWf = new WorkFlowInfo(null, delegate
            {
                NotifyDialog.Notify(string.Format("订单号：\"{0}\"反结算成功。", Data.OrderId), OwnerWindow.Owner);
                //_tableInfo.TableStatus = EnumTableStatus.Dinner;//将餐桌状态设置成就餐，调用GetTableDishInfo获取餐桌信息时就不会弹出开台窗口了。
                GetTableDishInfoAsync();
                return null;
            });
            helper.AntiSettlementAsync(Data.OrderId, Data.MemberNo, afterAntiSettlementWf);
        }

        /// <summary>
        /// 取消账单。
        /// </summary>
        private void CancelOrder()
        {
            if (Data.DishInfos.Any())
            {
                MessageDialog.Warning("只能取消空账单。", OwnerWindow);
                return;
            }

            if (!MessageDialog.Quest(string.Format("确定要取消桌号：{0}的帐单吗?", _tableInfo.TableName)))
                return;

            TaskService.Start(null, ClearTableProcess, ClearTableComplete, "取消账单中...");
        }

        /// <summary>
        /// 打开钱箱。（异步）
        /// </summary>
        private void OpenCashBoxAsync()
        {
            //密码验证
            if (PvSystemConfig.VSystemConfig.IsEnabledCheck)
            {
                if (!WindowHelper.ShowDialog(new CashboxPasswordInputWndVm(), OwnerWindow))
                    return;
            }

            ThreadPool.QueueUserWorkItem(t => { OpenCashBoxProcess(); });
        }

        /// <summary>
        /// 开钱箱的执行方法。
        /// </summary>
        private static void OpenCashBoxProcess()
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
        }

        /// <summary>
        /// 异步获取餐台详情。
        /// </summary>
        protected void GetTableDishInfoAsync()
        {
            SetRefreshTimerStatus(false);
            TaskService.Start(null, GetOrderInfoProcess, GetOrderInfoComplete, "加载/更新餐台详情...");
        }

        /// <summary>
        /// 生成会员消费工作流。
        /// </summary>
        /// <returns></returns>
        private WorkFlowInfo GenerateMemberSaleWf()
        {
            WorkFlowInfo wf = null;
            if (Globals.IsCanDaoMember)
                wf = new WorkFlowInfo(SaleMemberCanDaoProcess, SaleMemberComplete, "会员消费结算中...");
            else if (Globals.IsYazuoMember)
                wf = new WorkFlowInfo(SaleMemberYazuoProcess, SaleMemberComplete, "会员消费结算中...");

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
        /// 异步发送结算广播消息。
        /// </summary>
        private void BroadcastSettlementMsgAsync()
        {
            InfoLog.Instance.I("广播结算消息给PAD...");
            CommonHelper.BroadcastMessageAsync(EnumBroadcastMsgType.Settlement2Pad, Data.OrderId);

            InfoLog.Instance.I("广播结算指令给手环...");
            var msg = string.Format("{0}|{1}|{2}", Data.WaiterId, Data.TableName, Data.OrderId);
            CommonHelper.BroadcastMessageAsync(EnumBroadcastMsgType.Settlement2Wristband, msg);
        }

        /// <summary>
        /// 异步发送咖啡模式结账广播消息。
        /// </summary>
        private void BroadcastCoffeeSettlementMsgAsyc()
        {
            var msg = string.Format("{0}|{1}|{2}", Data.TableNo, Data.OrderId, Data.PaymentAmount);
            CommonHelper.BroadcastMessageAsync(EnumBroadcastMsgType.Settlement2Wristband, msg);
        }

        /// <summary>
        /// 异步发送清台广播消息。
        /// </summary>
        private void BroadcastClearTableMsgAsync()
        {
            CommonHelper.BroadcastMessageAsync(EnumBroadcastMsgType.ClearTable, Data.OrderId);
            var msg = string.Format("{0}|{1}|{2}", Data.TableNo, Data.OrderId, Data.PaymentAmount);
            CommonHelper.BroadcastMessageAsync(EnumBroadcastMsgType.Settlement2Wristband, msg);
        }

        /// <summary>
        /// 计算账单的应收金额。
        /// </summary>
        private void CalculatePaymentAmount()
        {
            if (Data == null)
                return;

            //计算自动填充现金金额。
            var otherPayWayWithoutCashPayWay = PayWayInfos.Where(t => t.PayWayType != EnumPayWayType.Cash).ToList();
            var amoumtSum = otherPayWayWithoutCashPayWay.Sum(t => t.Amount) + IntegralAmount;//所有结算方式输入金额的总和，注意要加上会员的积分金额。
            SetCashAmount(Math.Max(0, Data.PaymentAmount - amoumtSum));

            //计算总共已付金额。
            Data.TotalAlreadyPayment = PayWayInfos.Sum(t => t.Amount) + IntegralAmount; //付款总额=各种付款方式总和。
            Data.TotalAlreadyPayment = Math.Round(Data.TotalAlreadyPayment, 2);
            Data.RemainderAmount = Data.PaymentAmount - Data.TotalAlreadyPayment;//剩余金额=应付金额-付款总额。

            if (_cashPayWay != null)
            {
                var tempChargeAmount = Math.Abs(Math.Min(0, Data.RemainderAmount));
                _cashPayWay.ChargeAmount = Math.Min(tempChargeAmount, _cashPayWay.Amount);//找零金额只能是现金

                //小费当前计算规则：只能从现金扣除，
                if (HasTip && Data.TipAmount > 0 && _cashPayWay.IsVisible)//有小费金额的时候才计算小费实收。
                {
                    var realyPayment = Data.PaymentAmount - Data.TipAmount;//真实的应收=明面行应收-小费金额。
                    var tipPayment = Data.TotalAlreadyPayment - realyPayment;//小费实付金额=付款总额-真实应收。
                    tipPayment = Math.Max(0, tipPayment);//小费实付金额不能小于0。
                    tipPayment = Math.Min(CashAmount, tipPayment);//小费实付金额不能大于现金。
                    tipPayment = Math.Min(Data.TipAmount, tipPayment);//小费金额不能大于用户设置的小费金额。
                    _cashPayWay.TipPaymentAmount = tipPayment;
                }
            }
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

            CalculatePaymentAmount();

            var settlementInfo = new List<string>();
            foreach (var payWayInfo in PayWayInfos)
            {
                var info = payWayInfo.GenerateSettlementInfo();
                if (info.Any())
                    settlementInfo.AddRange(info);
            }

            if (Data.RemainderAmount > 0)
                settlementInfo.Add(Data.TotalAlreadyPayment > 0 ? string.Format("还需再收：{0:f2}", Data.RemainderAmount) : string.Format("需收款：{0:f2}", Data.RemainderAmount));

            if (Data.TotalDebitAmount != 0)
                settlementInfo.Add(string.Format("挂账：{0:f2}", Data.TotalDebitAmount));

            if (Data.TotalFreeAmount != 0)
                settlementInfo.Add(string.Format("优免：{0:f2}", Data.TotalFreeAmount));
            if (Data.AdjustmentAmount != 0)
                settlementInfo.Add(string.Format("优免调整：{0:f2}", Data.AdjustmentAmount));
            if (Data.ToalDebitAmountMany != 0)
                settlementInfo.Add(string.Format("挂账多收：{0:f2}", Data.ToalDebitAmountMany));

            if (Data.RoundAmount != 0)
                settlementInfo.Add(string.Format("舍{1}：{0:f2}", Math.Abs(Data.RoundAmount), Data.RoundAmount > 0 ? "去" : "入"));

            if (Data.RemovezeroAmount != 0)
                settlementInfo.Add(string.Format("抹零：{0:f2}", Data.RemovezeroAmount));

            SettlementInfo = string.Format("收款：{0}", string.Join("，", settlementInfo));
        }

        /// <summary>
        /// 检测账单是否允许结账。
        /// </summary>
        /// <returns>允许结账则返回null，否则返回错误信息。</returns>
        private string CheckTheBillAllowPay()
        {
            if (Data.DishInfos == null || !Data.DishInfos.Any())
                return "不能结账空账单。";

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

            if (_memberPayWay != null && Globals.IsCanDaoMember && _memberPayWay.IsMemberLogin &&
                (IntegralAmount > 0 || MemberAmount > 0) &&
                string.IsNullOrWhiteSpace(_memberPayWay.MemberPassword))//雅座会员消费不必输入密码。
                return "使用会员储值或积分请输入会员密码。";

            if (Data.TotalAlreadyPayment - ChargeAmount > Data.PaymentAmount)
                return string.Format("实际支付金额\"{0}\"超过应收金额\"{1}\"。", Data.TotalAlreadyPayment, Data.PaymentAmount);

            var errMsgList = PayWayInfos.Select(t => t.CheckTheBillAllowPay()).ToList();
            var errMsg = errMsgList.FirstOrDefault(t => !string.IsNullOrEmpty(t));
            if (!string.IsNullOrEmpty(errMsg))
                return errMsg;

            return null;
        }

        /// <summary>
        /// 生成账单结算信息集合。
        /// </summary>
        /// <returns></returns>
        private List<BillPayInfo> GenerateBillPayInfos()
        {
            var list = new List<BillPayInfo>();
            foreach (var payWayInfo in PayWayInfos)
            {
                var payInfo = payWayInfo.GenerateBillPayInfo();
                if (payInfo != null)
                    list.AddRange(payInfo);
            }

            if (Data.RoundAmount != 0)
                list.Add(new BillPayInfo(Data.RoundAmount, (int)EnumBillPayType.Rounding));
            if (Data.RemovezeroAmount != 0)
                list.Add(new BillPayInfo(Data.RemovezeroAmount, (int)EnumBillPayType.RemoveOdd));
            if (Data.AdjustmentAmount != 0)//优免调整
            {
                list.Add(new BillPayInfo(Data.AdjustmentAmount, (int)EnumBillPayType.FreeAmount, "优免调整"));
            }
            if (Data.ToalDebitAmountMany != 0)//挂账多收
            {
                list.Add(new BillPayInfo(Data.ToalDebitAmountMany, (int)EnumBillPayType.OnAccount, "挂账多收"));
            }

            foreach (var usedCouponInfo in Data.UsedCouponInfos)
            {
                //雅座会员的优惠券需要特殊处理。

                if (usedCouponInfo.FreeAmount > 0)
                {
                    var payType = usedCouponInfo.UsedCouponType == EnumUsedCouponType.YaZuo
                        ? EnumBillPayType.YazuoMemberCoupon
                        : EnumBillPayType.FreeAmount;
                    var payInfo = new BillPayInfo(usedCouponInfo.FreeAmount, (int)payType, usedCouponInfo.Name, usedCouponInfo.CouponInfo.PartnerName)
                    {
                        CouponNum = usedCouponInfo.Count,
                        CouponId = usedCouponInfo.CouponInfo.CouponId,
                        CouponDetailId = usedCouponInfo.CouponInfo.RuleId
                    };
                    list.Add(payInfo);
                }
                if (usedCouponInfo.DebitAmount > 0)
                {
                    var payInfo = new BillPayInfo(usedCouponInfo.DebitAmount, (int)EnumBillPayType.OnAccount, usedCouponInfo.Name, usedCouponInfo.CouponInfo.PartnerName)
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

        private void AmountChanged(object sender, AmountChangedEventArgs eventArgs)
        {
            _isUserInputCash = eventArgs.IsCashAmountChanged;
            GenerateSettlementInfo();
            _isUserInputCash = false;
        }

        /// <summary>
        /// 设置现金金额。
        /// </summary>
        /// <param name="amount"></param>
        private void SetCashAmount(decimal amount)
        {
            if (_isUserInputCash || !SystemConfigCache.AutoFillCashAmount)
                return;

            if (_cashPayWay != null)
                _cashPayWay.Amount = amount;
        }

        /// <summary>
        /// 结算后的一些处理。
        /// </summary>
        protected virtual void DosomethingAfterSettlement()
        {

        }

        #region Process Methods

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
        /// 会员查询执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object QueryMemberProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, MemberInfo>("创建IMemberService服务失败。", null);

            if (Globals.IsCanDaoMember)
            {
                var request = new CanDaoMemberQueryRequest
                {
                    branch_id = Globals.BranchInfo.BranchId,
                    cardno = MemberNo,
                };
                InfoLog.Instance.I("开始执行餐道会员查询...");
                return service.QueryCandao(request);
            }
            if (Globals.IsYazuoMember)
            {
                InfoLog.Instance.I("开始执行雅座会员查询...");
                var result = service.QueryYaZuo(MemberNo);
                return new Tuple<string, MemberInfo>(result.Item1, result.Item2);
            }
            return new Tuple<string, MemberInfo>("错误的会员配置项。", null);
        }

        /// <summary>
        /// 会员查询执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> QueryMemberComplete(object arg)
        {
            var result = (Tuple<string, MemberInfo>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("会员查询失败：{0}", result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return new Tuple<bool, object>(false, null);//不能return null，因为要执行错误工作流。
            }

            InfoLog.Instance.I("会员查询成功。");
            _memberPayWay.MemberInfo = result.Item2;
            return new Tuple<bool, object>(true, result.Item2);
        }

        /// <summary>
        /// 会员查询执行完成。（区别是这个方法在遇到多卡的情况下，会弹出会员卡选择窗口让用户选择一个卡）。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> QueryMemberComplete2(object arg)
        {
            var result = (Tuple<string, MemberInfo>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("会员查询失败：{0}", result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return new Tuple<bool, object>(false, null);//不能return null，因为要执行错误工作流。
            }

            InfoLog.Instance.I("会员查询成功。");
            _memberPayWay.MemberInfo = result.Item2;

            if (Globals.IsYazuoMember)
            {
                var yazuoMemberInfo = (YaZuoMemberInfo)_memberPayWay.MemberInfo;

                //处理雅座多卡
                if (yazuoMemberInfo.CardNoList != null && yazuoMemberInfo.CardNoList.Any())
                {
                    var cardSelectVm = new MultMemberCardSelectWndVm(yazuoMemberInfo.CardNoList, yazuoMemberInfo.CardNo);
                    if (WindowHelper.ShowDialog(cardSelectVm, OwnerWindow))
                    {
                        if (!_memberPayWay.Remark.Equals(cardSelectVm.SelectedCard))
                        {
                            _memberPayWay.Remark = cardSelectVm.SelectedCard;
                            MemberLogin();
                            return null;
                        }
                    }
                }

                ProcessYaZuoCouponInfo(yazuoMemberInfo);
            }

            return new Tuple<bool, object>(true, _memberPayWay.MemberInfo);
        }

        /// <summary>
        /// 反结算失败以后关闭窗口。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> AfterAntisettlementFailed(object arg)
        {
            CloseWindow(true);
            return null;
        }

        /// <summary>
        /// 处理雅座优惠券。
        /// </summary>
        /// <param name="yazuoMemberInfo"></param>
        private void ProcessYaZuoCouponInfo(YaZuoMemberInfo yazuoMemberInfo)
        {
            if (_yaZuoMemberCouponInfos != null)
                _yaZuoMemberCouponInfos.Clear();

            if (yazuoMemberInfo != null && yazuoMemberInfo.CouponList != null && yazuoMemberInfo.CouponList.Any())
            {
                _yaZuoMemberCouponInfos = yazuoMemberInfo.CouponList.Select(Convert2CouponInfo).ToList();

                if (_selectedCouponCategory.CategoryType == "88" && Globals.IsYazuoMember)//雅座会员。
                {
                    CouponInfos.Clear();
                    if (_yaZuoMemberCouponInfos != null)
                        _yaZuoMemberCouponInfos.ForEach(CouponInfos.Add);
                }
            }
        }

        /// <summary>
        /// 将雅座优惠券信息转成系统的优惠券信息，方便使用。
        /// </summary>
        /// <param name="yaZuoCouponInfo"></param>
        /// <returns></returns>
        private CouponInfo Convert2CouponInfo(YaZuoCouponInfo yaZuoCouponInfo)
        {
            return new CouponInfo
            {
                CouponId = yaZuoCouponInfo.CouponId,
                Name = yaZuoCouponInfo.CouponName,
                CouponType = EnumCouponType.YaZuoFree,
                FreeAmount = yaZuoCouponInfo.CouponAmount,
                MaxCouponCount = yaZuoCouponInfo.CouponCount,
                Color = "LightBlue",
            };
        }

        /// <summary>
        /// 会员登录执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object MemberLoginProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            InfoLog.Instance.I("开始执行会员登入...");
            var loginResult = service.MemberLogin(Data.OrderId, _memberPayWay.Remark);
            if (!string.IsNullOrEmpty(loginResult))
                return string.Format("会员登入失败：{0}。", loginResult);

            InfoLog.Instance.I("会员登入成功。");
            return null;
        }

        /// <summary>
        /// 会员登录执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> MemberLoginComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result, OwnerWindow);
                return new Tuple<bool, object>(false, null);//不能return null，因为要执行错误工作流。
            }

            InfoLog.Instance.I("完成会员登录流程。");
            _memberPayWay.IsMemberLogin = true;
            GetTableDishInfoAsync();
            return new Tuple<bool, object>(true, null);
        }

        /// <summary>
        /// 会员登出时执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object MemberLogoutProcess(object arg)
        {
            InfoLog.Instance.I("开始执行会员登出...");
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            var logoutResult = service.MemberLogout(Data.OrderId, Data.MemberNo);
            if (!string.IsNullOrEmpty(logoutResult))
                return string.Format("会员登出失败：{0}。", logoutResult);

            InfoLog.Instance.I("会员登出成功。");
            return null;
        }

        /// <summary>
        /// 会员登出执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> MemberLogoutComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result, OwnerWindow);
                return null;
            }

            InfoLog.Instance.I("完成会员登出流程。");
            _memberPayWay.MemberInfo = null;

            if (Globals.IsYazuoMember)
            {
                _yaZuoMemberCouponInfos = null;
                if (SelectedCouponCategory.CategoryType == "88")
                    CouponInfos.Clear();

                var couponIds = Data.UsedCouponInfos.Where(t => t.UsedCouponType == EnumUsedCouponType.YaZuo).Select(t => t.RelationId).Distinct().ToList();
                if (couponIds.Any())
                {
                    TaskService.Start(couponIds, DeleYaZuoCouponInfoProcess, DeleYaZuoCouponInfoComplete, "移除使用的雅座优惠券...");
                    return null;
                }
            }
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

            var otherPayWays = PayWayInfos.Where(t => t.PayWayType != EnumPayWayType.Member && t.PayWayType != EnumPayWayType.WeChat).ToList();
            var wechatPayWay = PayWayInfos.FirstOrDefault(t => t.PayWayType == EnumPayWayType.WeChat);
            var cardNo = _memberPayWay != null ? _memberPayWay.MemberInfo.CardNo : Data.MemberNo;
            var request = new CanDaoMemberSaleRequest
            {
                branch_id = Globals.BranchInfo.BranchId,
                cardno = cardNo,
                password = _memberPayWay != null ? _memberPayWay.MemberPassword : "",
                FCash = otherPayWays.Sum(t => t.Amount),
                FWeChat = wechatPayWay != null ? wechatPayWay.Amount : 0,
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
                cardno = cardNo,
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
        /// 雅座会员消费执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object SaleMemberYazuoProcess(object arg)
        {
            InfoLog.Instance.I("开始会员消费结算...");
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            return service.SettlementYaZuo(GenerateYaZuoSettlementInfo());
        }

        /// <summary>
        /// 会员消费执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> SaleMemberComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result, OwnerWindow);
                return new Tuple<bool, object>(false, null);//会员结算失败，走错误流程。
            }

            return new Tuple<bool, object>(true, null);
        }

        /// <summary>
        /// 生成雅座会员结算信息。
        /// </summary>
        /// <returns></returns>
        private YaZuoSettlementInfo GenerateYaZuoSettlementInfo()
        {
            var otherPayWays = PayWayInfos.Where(t => t.PayWayType != EnumPayWayType.Member).ToList();
            return new YaZuoSettlementInfo
            {
                CashAmount = otherPayWays.Sum(t => t.Amount),
                CouponTotalAmount = 0,
                CouponUsedInfo = GenerateYaZuoCouponUsedInfo(),
                IntegralValue = IntegralAmount,
                MemberCardNo = MemberNo,
                OrderId = Data.OrderId,
                Password = _memberPayWay != null ? _memberPayWay.MemberPassword : "",
                StoredPayAmount = MemberAmount,
                UserId = Globals.UserInfo.UserName,
            };
        }

        /// <summary>
        /// 生成雅座优惠券使用信息。
        /// </summary>
        /// <returns></returns>
        private string GenerateYaZuoCouponUsedInfo()
        {
            if (Data.UsedCouponInfos != null && Globals.IsYazuoMember)
            {
                var couponIds = Data.UsedCouponInfos.Where(t => t.UsedCouponType == EnumUsedCouponType.YaZuo).Select(t => t.CouponInfo.CouponId).Distinct().ToList();
                if (couponIds.Any())
                {
                    var result = "";
                    foreach (var couponId in couponIds)
                    {
                        var couponName = Data.UsedCouponInfos.First(t => t.CouponInfo.CouponId == couponId).Name;
                        var items = Data.UsedCouponInfos.Where(t => t.CouponInfo.CouponId == couponId).ToList();
                        var totalFreeAmount = items.Sum(t => t.FreeAmount);
                        var totalNum = items.Sum(t => t.Count);

                        var nameLength = Encoding.Default.GetBytes(couponName).Length;//名称如果包含中文，如果直接用string.Length会导致长度计算问题。
                        var name = couponName + "".PadLeft(30 - nameLength, ' ');//优惠券名称，不足30位时后面的补空格。
                        var code = couponId.PadLeft(15, '0');//优惠券编号，不足15位时采用高位字符0。
                        var amount = Convert.ToInt32((totalFreeAmount * 100)).ToString("D12");//优惠券金额，不足12位时采用高位字符0。
                        var count = totalNum.ToString().PadLeft(4, '0');//优惠券编号，不足4位时采用高位补字符0。

                        result += string.Format("{0}{1}{2}{3}", name, code, amount, count);
                    }
                    return result;
                }
            }
            return "";
        }

        /// <summary>
        /// 打印结账单和发票。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> PrintSettlementReportAndInvoice(object arg)
        {
            if (Data.TableType == EnumTableType.Outside || Data.TableType == EnumTableType.Room)
            {
                //BroadcastSettlementMsgAsync(); //转成后台进行发送。
                CommonHelper.SendMsgAsync(Data.OrderId, EnumMsgType.Settlement);//结账完成广播消息，通知PAD清台。
            }
            else if (Data.TableType == EnumTableType.CFTable)
                BroadcastCoffeeSettlementMsgAsyc();

            //结账成功打开钱箱。
            ThreadPool.QueueUserWorkItem(t => { OpenCashBoxProcess(); });

            InfoLog.Instance.I("开始打印结账单...");
            var print = new ReportPrintHelper2(OwnerWindow);
            print.PrintSettlementReport(Data.OrderId, Globals.UserInfo.UserName);
            InfoLog.Instance.I("结束打印结账单。");

            if (!string.IsNullOrEmpty(Data.MemberNo))
            {
                InfoLog.Instance.I("开始打印交易凭条...");
                print.PrintMemberPayBillReport(Data.OrderId, Globals.UserInfo.UserName);
                InfoLog.Instance.I("结束打印交易凭条。");
            }

            if (!string.IsNullOrEmpty(Data.OrderInvoiceTitle))
            {
                InfoLog.Instance.I("有发票信息，显示发票金额设置窗口...");
                WindowHelper.ShowDialog(new SetInvoiceAmountWndVm(Data), OwnerWindow);
            }

            NotifyDialog.Notify(string.Format("桌号\"{0}\"结账成功。", Data.TableName), OwnerWindow.Owner);
            InfoLog.Instance.I("桌号\"{0}\"结账成功。结束整个结账流程，关闭窗口。", Data.TableName);
            CloseWindow(true);//结算完成后关闭窗口。
            return null;
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

            var tipAmount = _cashPayWay != null ? _cashPayWay.TipPaymentAmount : 0m;
            return service.TipSettlement(Data.OrderId, tipAmount);
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

        /// <summary>
        /// 整单退菜的执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object BackAllDishProcess(object param)
        {
            InfoLog.Instance.I("开始桌台{0}整桌退菜...", Data.TableNo);
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            var backDishReason = param as string;
            return service.BackAllDish(Data.OrderId, Data.TableName, Globals.Authorizer.UserName, backDishReason);
        }

        /// <summary>
        /// 整单退菜执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
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
        /// 退菜的执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object BackDishProess(object param)
        {
            InfoLog.Instance.I("开始桌台{0}退菜...", Data.TableNo);
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            return service.BackDish((BackDishComboInfo)param);
        }

        /// <summary>
        /// 退菜执行完成。
        /// </summary>
        /// <param name="param"></param>
        public void BackDishComplete(object param)
        {
            var result = (string)param;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E("退菜失败。{0}", result);
                MessageDialog.Warning(result);
                return;
            }

            InfoLog.Instance.I("退菜成功。调用异步广播消息给PAD...");
            NotifyDialog.Notify("退菜成功", OwnerWindow);
            CommonHelper.BroadcastMessageAsync(EnumBroadcastMsgType.SyncOrder, Data.OrderId);
            GetTableDishInfoAsync();
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
                var result = (string)BackAllDishProcess("清台自动整单退菜");
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
        /// 获取餐台账单明细
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object GetOrderInfoProcess(object param)
        {
            InfoLog.Instance.I("开始获取餐台所有信息...");
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            var result = service.GetOrderInfo(_tableInfo.OrderId, "", ((int)_isKeepOdd));
            if (!string.IsNullOrEmpty(result.Item1))
                return string.Format("获取餐台明细失败：{0}", result.Item1);

            InfoLog.Instance.I("获取餐台所有信息完成。");
            if (result.Item2 == null)
                return "没有获取到该餐台的账单信息。";

            OwnerWindow.Dispatcher.Invoke((Action)delegate
            {
                Data.CloneOrderData(result.Item2);//合并餐台账单明细(金额，菜，优惠)
                if (_memberPayWay != null)
                    _memberPayWay.Remark = Data.MemberNo;
                if (_cashPayWay != null)
                    _cashPayWay.TipPaymentAmount = Data.TipAmount;
            });

            if (_memberPayWay != null && !string.IsNullOrEmpty(_memberPayWay.Remark) && !_memberPayWay.IsMemberLogin)//走会员登录的流程。
            {
                InfoLog.Instance.I("该餐台登录了会员，开始会员信息查询...");
                var memberQueryResult = (Tuple<string, MemberInfo>)QueryMemberProcess(null);
                if (!string.IsNullOrEmpty(memberQueryResult.Item1))
                {
                    var msg = string.Format("会员信息查询时失败：{0}", memberQueryResult.Item1);
                    ErrLog.Instance.E(msg);
                    msg += "\n，请联系管理员处理，不然可能导致结算失败。";
                    OwnerWindow.Dispatcher.Invoke((Action)delegate { MessageDialog.Warning(msg); });
                }
                else
                {
                    InfoLog.Instance.I("完成会员查询。");
                    _memberPayWay.MemberInfo = memberQueryResult.Item2;

                    if (Globals.IsYazuoMember)
                    {
                        ProcessYaZuoCouponInfo((YaZuoMemberInfo)_memberPayWay.MemberInfo);
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///  获取餐台菜品信息执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void GetOrderInfoComplete(object param)
        {
            var result = (string)param;
            SetRefreshTimerStatus(true);
            if (!string.IsNullOrEmpty(result))
            {
                MessageDialog.Warning(result, OwnerWindow);
                CloseWindow(false);
            }
            GenerateSettlementInfo();
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
                result.Item2.ForEach(t => { CouponInfos.Add(t); });
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

            return service.ClearTable(_tableInfo.OrderId);
        }

        /// <summary>
        /// 清台执行完成。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private void ClearTableComplete(object param)
        {
            var result = (string)param;
            if (!string.IsNullOrEmpty(result))
            {
                var msg = string.Format("账单取消失败“{0}", result);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, OwnerWindow);
                return;
            }

            InfoLog.Instance.I("取消账单完成。");
            if (!Data.IsTakeoutTable)
            {
                InfoLog.Instance.I("广播清台消息给PAD...");
                CommonHelper.BroadcastMessageAsync(EnumBroadcastMsgType.ClearTable, Data.OrderId);
            }
            NotifyDialog.Notify("取消账单完成。", OwnerWindow.Owner);
            CloseWindow(true);
        }

        /// <summary>
        /// 获取结算方式执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object GetPayWayInfoProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, List<PayWayInfo>>("创建IRestaurantService服务失败。", null);

            return service.GetPayWayInfo();
        }

        /// <summary>
        /// 获取结算方式完成时执行。
        /// </summary>
        /// <param name="obj"></param>
        private void GetPayWayInfoComplete(object obj)
        {
            var result = (Tuple<string, List<PayWayInfo>>)obj;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var errMsg = string.Format("获取结算方式失败：{0}", result.Item1);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg);
            }

            if (result.Item2 != null)
            {
                foreach (var info in result.Item2)
                {
                    var temp = info.CloneObject();
                    switch (info.PayWayType)
                    {
                        case EnumPayWayType.Cash:
                            var cashPayWay = new PayWayCashInfo();
                            cashPayWay.CloneData(info);
                            temp = cashPayWay;
                            _cashPayWay = cashPayWay;
                            break;
                        case EnumPayWayType.Bank:
                            var bankPayWay = new PayWayBankInfo();
                            bankPayWay.CloneData(info);
                            bankPayWay.SelectedBankInfo = Globals.BankInfos != null ? Globals.BankInfos.FirstOrDefault(t => t.Id == 0) : null;
                            temp = bankPayWay;
                            _bankPayWay = bankPayWay;
                            break;
                        case EnumPayWayType.Member:
                            var memberPayWay = new PayWayMemberInfo();
                            memberPayWay.CloneData(info);
                            temp = memberPayWay;
                            _memberPayWay = memberPayWay;
                            break;
                        case EnumPayWayType.OnAccount:
                            var onCmpAccPayWay = new PayWayOnCmpAccount();
                            onCmpAccPayWay.CloneData(info);
                            temp = onCmpAccPayWay;
                            _onCmpAccountPayWay = onCmpAccPayWay;
                            break;
                    }

                    if (temp.IsVisible)
                    {
                        temp.AmountChanged += AmountChanged;
                        PayWayInfos.Add(temp);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}