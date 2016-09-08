using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.Common.PublicValues;
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
        /// 反结原因。
        /// </summary>
        private string _antiSettlementReason;

        /// <summary>
        /// 当前选中的优惠券。
        /// </summary>
        private CouponInfo _curSelectedCouponInfo;

        /// <summary>
        /// 是否是用户输入现金金额。
        /// </summary>
        private bool _isUserInputCash;

        /// <summary>
        /// 是否是用户输入会员号。
        /// </summary>
        private bool _isUserInputMember;

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

        #endregion

        #region Constructor

        public TableOperWndVm(TableInfo tableInfo)
        {
            _isKeepOdd = EnumKeepOdd.Process;
            _tableInfo = tableInfo;
            InitCouponCategories();
            InitCouponLongPressTimer();
            InitRefreshTimer();
            SelectedBankInfo = Globals.BankInfos != null ? Globals.BankInfos.FirstOrDefault(t => t.Id == 0) : null;
            Data = new TableFullInfo();
            Data.CloneDataFromTableInfo(tableInfo);
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

        /// <summary>
        /// 会员控件获取或失去焦点命令。
        /// </summary>
        public ICommand MemberControlFocusCmd { get; private set; }

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
        /// 现金控件获取或失去焦点命令时执行。
        /// </summary>
        /// <param name="param"></param>
        private void CashControlFocus(object param)
        {
            _isUserInputCash = Convert.ToBoolean(param);
        }

        /// <summary>
        /// 会员控件获取获取或失去焦点命令的执行。
        /// </summary>
        /// <param name="param"></param>
        private void MemberControlFocus(object param)
        {
            _isUserInputMember = Convert.ToBoolean(param);
        }

        #region 优惠券处理

        /// <summary>
        /// 优惠券鼠标按下命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void CouponMouseDown(object arg)
        {
            var coupon = (CouponInfo)((CouponInfo)arg).Clone();
            _curSelectedCouponInfo = coupon;

            _isLongPressModel = false;
            _couponLongPressTimer.Start();
        }

        /// <summary>
        /// 优惠券鼠标弹起命令的执行方法。
        /// </summary>
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

            if (_curSelectedCouponInfo.CouponType == EnumCouponType.HandFree) //手工优惠类特殊处理。
            {
                if (!CouponHandFreeProcess())
                    return;
            }
            else if (_curSelectedCouponInfo.IsDiscount) //折扣类处理流程。
            {
                if (!MessageDialog.Quest(string.Format("确定使用{0}？", _curSelectedCouponInfo.Name)))
                    return;

                InfoLog.Instance.I("开始折扣类优惠券：{0}计算接口。", _curSelectedCouponInfo.Name);

                CreatUsePreferentialRequest("1", "", 0, 0, 0);

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

                    //_curSelectedCouponInfo.BillAmount = debitAmount;
                    //_curSelectedCouponInfo.FreeAmount = 0;
                    //_curSelectedCouponInfo.Amount = debitAmount;

                    CreatUsePreferentialRequest("1", "", 0, debitAmount, 1);

                    //AddCouponInfoAsUsed(_curSelectedCouponInfo, 1, false);
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
                            //增加选择数量的窗口
                            var numWnd = new NumInputWindow("优惠券", _curSelectedCouponInfo.Name, "使用数量", 0, false);
                            if (!WindowHelper.ShowDialog(numWnd, OwnerWindow))
                                return;

                            //AddCouponInfoAsUsed(_curSelectedCouponInfo, Convert.ToInt32(numWnd.InputNum), false);

                            CreatUsePreferentialRequest(numWnd.InputNum.ToString(), "", 0, 0, 0);

                        }
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

                            CreatUsePreferentialRequest("1", "", 0, offSelectWnd.Amount, 1);

                            //_curSelectedCouponInfo.BillAmount = offSelectWnd.Amount;
                            //_curSelectedCouponInfo.FreeAmount = offSelectWnd.Amount;
                            //AddCouponInfoAsUsed(_curSelectedCouponInfo, 1, false, offSelectWnd.Amount);
                        }
                        else if (offSelectWnd.SelectedOfferType == EnumOfferType.Discount)
                        {
                            CreatUsePreferentialRequest("1", "", offSelectWnd.Discount, 0, 1);


                            //TaskService.Start(offSelectWnd.Discount, CalcDiscountAmountProcess, CalcDiscountAmountComplete, "计算折扣金额中...");
                        }
                    }
                }
            }

            //SaveCouponInfo();
        }
        /// <summary>
        /// 创建使用优惠券参数
        /// </summary>
        /// <param name="pNum">使用券数量</param>
        /// <param name="dishId">菜品Id</param>
        /// <param name="disRate">手工折扣额</param>
        /// <param name="preferentialAmout">手工优惠金额</param>
        /// <param name="isCustom">是否收银员输入（1：是：0不是）</param>
        /// <returns></returns>
        private void CreatUsePreferentialRequest(string pNum, string dishid, decimal disRate, decimal preferentialAmout, int isCustom)
        {
            var usePreferential = new UsePreferentialRequest();
            usePreferential.orderid = _tableInfo.OrderId;
            usePreferential.preferentialid = _curSelectedCouponInfo.CouponId;
            usePreferential.type = _curSelectedCouponInfo.CouponType.GetHashCode().ToString();
            if (_curSelectedCouponInfo.HandCouponType != null)
            {
                usePreferential.sub_type = ((int)_curSelectedCouponInfo.HandCouponType).ToString();
            }
            usePreferential.preferentialAmt = Data.CouponAmount.ToString();
            usePreferential.adjAmout = Data.AdjustmentAmount.ToString();
            usePreferential.toalDebitAmount = Data.TotalDebitAmount.ToString();
            usePreferential.toalFreeAmount = Data.TotalFreeAmount.ToString();
            usePreferential.toalDebitAmountMany = Data.ToalDebitAmountMany.ToString();

            usePreferential.preferentialNum = pNum;
            usePreferential.dishid = dishid;
            usePreferential.isCustom = isCustom.ToString();
            usePreferential.disrate = disRate;
            usePreferential.preferentialAmout = preferentialAmout.ToString();

            TaskService.Start(usePreferential, UseCouponProcess, UseCouponInfoComplete, "保存使用优惠券...");
        }

        /// <summary>
        /// 保存使用优惠券处理
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object UseCouponProcess(object arg)
        {
            IOrderService service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return new Tuple<string, preferentialInfoResponse>("创建IOrderService服务失败。", null);

            return service.UsePreferential((UsePreferentialRequest)arg);
        }

        /// <summary>
        /// 使用优惠券完成
        /// </summary>
        /// <param name="obj"></param>
        private void UseCouponInfoComplete(object obj)
        {
            var result = obj as Tuple<string, preferentialInfoResponse>;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var errMsg = string.Format("保存优惠券信息失败：{0}", result.Item1);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg, OwnerWindow);
                return;
            }
            else
            {
                ProcessCouponShow(result.Item2);

                InfoLog.Instance.I("结束保存优惠券到使用列表。");
                //AddCouponInfoAsUsed(_curSelectedCouponInfo, 1, false);
            }
        }

        /// <summary>
        /// 手工优免类优惠券处理方式。
        /// </summary>
        private bool CouponHandFreeProcess()
        {
            var result = false;
            switch (_curSelectedCouponInfo.HandCouponType)
            {
                case EnumHandCouponType.FreeDish:
                    InfoLog.Instance.I("选择手工优惠赠菜类优惠券，弹出赠菜选择窗口...");
                    var giftDishWnd = new SelectGiftDishWindow(Data);
                    if (WindowHelper.ShowDialog(giftDishWnd, OwnerWindow))
                    {
                        if (giftDishWnd.SelectedGiftDishInfos.Count > 0)
                        {
                            var dishIds = new List<string>();
                            var dishNum = new List<string>();
                            foreach (var giftDishInfo in giftDishWnd.SelectedGiftDishInfos)
                            {

                                dishIds.Add(giftDishInfo.DishId);
                                dishNum.Add(giftDishInfo.SelectGiftNum.ToString());
                            }
                            CreatUsePreferentialRequest(string.Join(",", dishNum), string.Join(",", dishIds), 0, 0, 0);

                        }

                        result = true;
                    }
                    break;
                case EnumHandCouponType.Discount:
                    InfoLog.Instance.I("选择手工优惠折扣类优惠券，弹出折扣输入窗口...");
                    var diacountSelectWnd = new OfferTypeSelectWindow(EnumOfferType.Discount);
                    if (WindowHelper.ShowDialog(diacountSelectWnd, OwnerWindow))
                    {
                        InfoLog.Instance.I("选择折扣率：{0}，开始调用接口计算折扣金额...", diacountSelectWnd.Discount);
                        CreatUsePreferentialRequest("1", "", diacountSelectWnd.Discount, 0, 1);

                        //TaskService.Start(diacountSelectWnd.Discount, CalcDiscountAmountProcess, CalcDiscountAmountComplete, "计算折扣金额中...");
                        result = true;
                    }
                    break;
                case EnumHandCouponType.Amount:
                    InfoLog.Instance.I("选择手工优惠优免类优惠券，弹出优免金额输入窗口...");
                    var amountSelectWnd = new OfferTypeSelectWindow(EnumOfferType.Amount);
                    if (WindowHelper.ShowDialog(amountSelectWnd, OwnerWindow))
                    {
                        CreatUsePreferentialRequest("1", "", 0, amountSelectWnd.Amount, 1);

                        //AddCouponInfoAsUsed(_curSelectedCouponInfo, 1, false, amountSelectWnd.Amount);
                        result = true;
                    }
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
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
                return new Tuple<string, preferentialInfoResponse>("创建IOrderService服务失败。", null);

            return service.DelPreferential((DelPreferentialRequest)arg);
        }

        /// <summary>
        /// 删除优惠券完成
        /// </summary>
        /// <param name="obj"></param>
        private void DeleCouponInfoComplete(object obj)
        {
            var result = obj as Tuple<string, preferentialInfoResponse>;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var errMsg = string.Format("删除优惠券信息失败：{0}", result);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg, OwnerWindow);
                return;
            }
            else
            {
                //先清除
                Data.UsedCouponInfos.Clear();

                ProcessCouponShow(result.Item2);

                InfoLog.Instance.I("结束删除优惠券。");
                //AddCouponInfoAsUsed(_curSelectedCouponInfo, 1, false);
            }
        }

        /// <summary>
        /// 移除一个优惠券。
        /// </summary>
        /// <param name="item">移除的优惠券信息。</param>
        private void RemoveUsedCouponInfo(UsedCouponInfo item)
        {
            //if (item == null || Data == null)
            //    return;

            //Data.UsedCouponInfos.Remove(item);
            //Data.TotalDebitAmount = Data.UsedCouponInfos.Sum(t => t.DebitAmount * t.Count);
            //Data.TotalFreeAmount = Data.UsedCouponInfos.Sum(t => t.FreeAmount * t.Count);

            //CalculatePaymentAmount(); //优惠券添加完毕后计算实收。
            var parma = new DelPreferentialRequest();
            parma.orderid = _tableInfo.OrderId;
            parma.DetalPreferentiald = item.RelationId;
            parma.clear = 0;

            TaskService.Start(parma, DeleCouponInfoProcess, DeleCouponInfoComplete, "正在删除优惠券...");
        }

        /// <summary>
        /// 清除优惠券。
        /// </summary>
        /// <param name="Data"></param>
        private void ClearUsedCouponInfo()
        {
            var parma = new DelPreferentialRequest();
            parma.orderid = _tableInfo.OrderId;
            parma.DetalPreferentiald = "";
            parma.clear = 1;

            TaskService.Start(parma, DeleCouponInfoProcess, DeleCouponInfoComplete, "正在清空优惠券...");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preferential"></param>
        private void ProcessCouponShow(preferentialInfoResponse preferential)
        {
            Data.ClonePreferentialInfo(preferential);
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
            MemberControlFocusCmd = CreateDelegateCommand(MemberControlFocus);
            CouponMouseDownCmd = CreateDelegateCommand(CouponMouseDown);
            CouponMouseUpCmd = CreateDelegateCommand(CouponMouseUp);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs arg)
        {
            if (arg.Key == Key.Enter)
            {
                arg.Handled = true;
                if (_isUserInputMember)
                    MemberLogin();
                else
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
                    var wnd = new SelectBankWindow(SelectedBankInfo);
                    if (WindowHelper.ShowDialog(wnd, OwnerWindow))
                        SelectedBankInfo = wnd.SelectedBank;
                    break;
                case "SelectOnAccountCompany":
                    InfoLog.Instance.I("开始获取挂账单位...");
                    var companyWnd = new OnAccountCompanySelectWindow();
                    if (WindowHelper.ShowDialog(companyWnd, OwnerWindow))
                        SelectedOnCmpAccInfo = companyWnd.SelectedCompany;
                    break;
                case "MemberLogin":
                    MemberLogin();
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
                    BackAllOrderDish();
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
                    return !string.IsNullOrEmpty(MemberCardNo) && MemberCardNo.Length >= 0;
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
                default:
                    return true;
            }
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
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
            {
                ErrLog.Instance.E("创建IOrderService服务失败。");
                return;
            }

            var result = service.GetTableDishInfoes(Data.TableName, Globals.UserInfo.UserName);
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取餐台信息错误：{0}。", result.Item1);
                return;
            }

            if (result.Item2 == null)
            {
                ErrLog.Instance.E("获取到餐台信息为空。");
                return;
            }

            if (Data.TotalAmount != result.Item2.TotalAmount || Data.DishInfos.Sum(t => t.DishNum) != result.Item2.DishInfos.Sum(t => t.DishNum))//当总价或菜品数量改变时再触发刷新方法。
            {
                _tableInfo.OrderId = result.Item2.OrderId;//可能会有并台导致订单号改变。
                GetTableDishInfoAsync();
            }
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
                if (Data.MemberInfo == null)
                {
                    var queryMemberWf = GenerateMemberQueryWf();
                    curStepWf.NextWorkFlowInfo = queryMemberWf;
                    curStepWf = queryMemberWf;
                }

                var saleMemberWf = GenerateMemberSaleWf();
                curStepWf.NextWorkFlowInfo = saleMemberWf;
                curStepWf = saleMemberWf;

                var helper = new AntiSettlementHelper();
                var antiSettlementWf = helper.GetAntiSettlement(Data.OrderId, MemberCardNo, OwnerWindow);
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
            InfoLog.Instance.I("会员登录按钮点击...");
            var loginWf = GenerateMemberLoginWf();
            ((TableOperWindow)OwnerWindow).TbMemAmount.Focus(); //这里是为了解决登录以后直接点回车，不触发结账的问题。
            if (loginWf != null)
                WorkFlowService.Start(MemberCardNo, loginWf);
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
            }

            var wnd = new AuthorizationWindow(EnumRightType.BackDish);
            if (!WindowHelper.ShowDialog(wnd, OwnerWindow))
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

            if (WindowHelper.ShowDialog(new AuthorizationWindow(EnumRightType.BackDish), OwnerWindow))
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
            helper.AntiSettlementAsync(Data.OrderId, Data.MemberNo, OwnerWindow, afterAntiSettlementWf);
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

            WorkFlowService.Start(null, new WorkFlowInfo(ClearTableProcess, ClearTableComplete, "取消账单中..."));
        }

        /// <summary>
        /// 打开钱箱。（异步）
        /// </summary>
        private void OpenCashBoxAsync()
        {
            //密码验证
            if (PvSystemConfig.VSystemConfig.IsEnabledCheck)
            {
                var viewModel = new UCCashboxPswViewModel();
                if (WindowHelper.ShowDialog(viewModel.GetUserCtl(), OwnerWindow) == false)
                {
                    return;
                }
            }

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
            TaskService.Start(null, GetOrderInfoProcess, GetOrderInfoComplete, "加载餐台详情...");
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

            //var paymentAmount = Data.TotalAmount - Data.TotalFreeAmount - Data.TotalDebitAmount; //应收=总额-优免-挂账
            //paymentAmount = Math.Max(0, paymentAmount);//应收必须大于0
            //paymentAmount += Data.TipAmount;//应收需要再加上小费金额。
            //var amount = ProcessOdd(paymentAmount, _curOddModel, Globals.OddAccuracy);
            //Data.AdjustmentAmount = paymentAmount - amount;
            //Data.PaymentAmount = Math.Max(0, amount); //应付金额不能小于0

            //if (_curOddModel == EnumOddModel.Rounding)
            //    RoundingAmount = Math.Abs(Data.AdjustmentAmount);
            //else if (_curOddModel == EnumOddModel.Wipe)
            //    WipeOddAmount = Data.AdjustmentAmount;

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
                settlementInfo.Add(string.Format("优免调整：{0:f2}", Data.AdjustmentAmount));
            if (Data.ToalDebitAmountMany != 0)
                settlementInfo.Add(string.Format("挂账多收：{0:f2}", Data.ToalDebitAmountMany));
            //if (Data.AdjustmentAmount != 0)
            //{
            //    if (Globals.OddModel == EnumOddModel.Rounding)
            //        settlementInfo.Add(string.Format("舍{1}：{0:f2}", Math.Abs(Data.AdjustmentAmount), Data.AdjustmentAmount > 0 ? "去" : "入"));
            //    else
            //        settlementInfo.Add(string.Format("抹零：{0:f2}", Data.AdjustmentAmount));
            //}

            if (Data.RoundAmount != 0)
            {
                settlementInfo.Add(string.Format("舍{1}：{0:f2}", Math.Abs(Data.RoundAmount), Data.RoundAmount > 0 ? "去" : "入"));
                RoundingAmount = Data.RoundAmount;
            }
            if (Data.RemovezeroAmount != 0)
            {
                settlementInfo.Add(string.Format("抹零：{0:f2}", Data.RemovezeroAmount));
                WipeOddAmount = Data.RemovezeroAmount;
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
            Data.TotalAlreadyPayment = Math.Round(Data.TotalAlreadyPayment, 2);
            var remainderAmount = Data.PaymentAmount - Data.TotalAlreadyPayment;//剩余金额=应付金额-付款总额。
            if (remainderAmount > 0)
            {
                ChargeAmount = 0;
                settlementInfo.Add(Data.TotalAlreadyPayment > 0 ? string.Format("还需再收：{0:f2}", remainderAmount) : string.Format("需收款：{0:f2}", remainderAmount));
            }
            var tempChargeAmount = Math.Abs(Math.Min(0, remainderAmount));
            ChargeAmount = Math.Min(tempChargeAmount, CashAmount);//找零金额只能是现金
            if (ChargeAmount > 0)
                settlementInfo.Add(string.Format("找零：{0:f2}", ChargeAmount));

            //小费当前计算规则：只能从现金扣除，
            if (HasTip && Data.TipAmount > 0)//有小费金额的时候才计算小费实收。
            {
                var realyPayment = Data.PaymentAmount - Data.TipAmount;//真实的应收=明面行应收-小费金额。
                var tipPayment = Data.TotalAlreadyPayment - realyPayment;//小费实付金额=付款总额-真实应收。
                tipPayment = Math.Max(0, tipPayment);//小费实付金额不能小于0。
                tipPayment = Math.Min(CashAmount, tipPayment);//小费实付金额不能大于现金。
                tipPayment = Math.Min(Data.TipAmount, tipPayment);//小费金额不能大于用户设置的小费金额。
                TipPaymentAmount = tipPayment;
                settlementInfo.Add(string.Format("小费：{0:f2}", TipPaymentAmount));
            }

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

            if (ChargeAmount > 100)
                return "找零金额不能大于100。";

            if (MemberAmount > 0 && string.IsNullOrEmpty(MemberCardNo))
                return "使用会员储值请先登录会员。";

            if (IntegralAmount > 0 && string.IsNullOrEmpty(MemberCardNo))
                return "使用会员积分请先登录会员。";

            if (Globals.IsCanDaoMember &&
                !string.IsNullOrWhiteSpace(MemberCardNo) &&
                (IntegralAmount > 0 || MemberAmount > 0) &&
                string.IsNullOrWhiteSpace(MemberPassword))//雅座会员消费不必输入密码。
                return "使用会员储值或积分请输入会员密码。";

            if (DebitAmount > 0 && SelectedOnCmpAccInfo == null)
                return "使用挂账金额请先选择挂账单位。";

            if (AlipayAmount > 0 && string.IsNullOrEmpty(AlipayNo))
                return "使用支付宝支付请输入支付宝账号。";

            if (WechatAmount > 0 && string.IsNullOrEmpty(WechatNo))
                return "使用微信支付请先输入微信账号。";

            if (Data.TotalAlreadyPayment - ChargeAmount > Data.PaymentAmount)
                return string.Format("实际支付金额\"{0}\"超过应收金额\"{1}\"。", Data.TotalAlreadyPayment, Data.PaymentAmount);

            return null;
        }

        /// <summary>
        /// 生成账单结算信息集合。
        /// </summary>
        /// <returns></returns>
        private List<BillPayInfo> GenerateBillPayInfos()
        {
            int bankId = SelectedBankInfo != null ? SelectedBankInfo.Id : 0;
            var realCashAmount = CashAmount - ChargeAmount - TipPaymentAmount;//真实的现金消费金额为输入的现金-找零的现金-小费金额
            var list = new List<BillPayInfo>
            {
                new BillPayInfo(realCashAmount, EnumBillPayType.Cash),
                new BillPayInfo(BankAmount, EnumBillPayType.BankCard, BankCardNo, bankId.ToString()),
                new BillPayInfo(MemberAmount, EnumBillPayType.MemberCard, "", MemberCardNo),
                new BillPayInfo(IntegralAmount, EnumBillPayType.MemberIntegral, "", MemberCardNo),
                new BillPayInfo(AlipayAmount, EnumBillPayType.Alipay, AlipayNo),
                new BillPayInfo(WechatAmount, EnumBillPayType.Wechat, WechatNo)
            };

            var onAcc = SelectedOnCmpAccInfo != null ? SelectedOnCmpAccInfo.Name : "";
            var cmpId = SelectedOnCmpAccInfo != null ? SelectedOnCmpAccInfo.Id : "";
            list.Add(new BillPayInfo(DebitAmount, EnumBillPayType.OnCompanyAccount, onAcc) { CouponDetailId = cmpId });

            if (Data.RoundAmount != 0)
                list.Add(new BillPayInfo(Data.RoundAmount, EnumBillPayType.Rounding));
            if (Data.RemovezeroAmount != 0)
                list.Add(new BillPayInfo(Data.RemovezeroAmount, EnumBillPayType.RemoveOdd));
            if (Data.AdjustmentAmount != 0)//优免调整
            {
                list.Add(new BillPayInfo(Data.AdjustmentAmount, EnumBillPayType.FreeAmount, "优免调整"));
            }
            if (Data.ToalDebitAmountMany != 0)//挂账多收
            {
                list.Add(new BillPayInfo(Data.ToalDebitAmountMany, EnumBillPayType.OnAccount, "挂账多收"));
            }

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

            //InfoLog.Instance.I("开始设置会员价...");
            //var orderService = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            //if (orderService == null)
            //    return "创建IOrderService服务失败。";

            //var setMemberPriceResult = orderService.SetMemberPrice(Data.OrderId, MemberCardNo);
            //if (!string.IsNullOrEmpty(setMemberPriceResult))
            //    return string.Format("设置会员价失败：{0}", setMemberPriceResult);

            //InfoLog.Instance.I("设置会员价完成。");
            //InfoLog.Instance.I("从新获取餐台所有信息...");

            //var result = new Tuple<string, TableFullInfo>("未赋值", null);
            //if (Data.TableType == EnumTableType.CFTakeout || Data.TableType == EnumTableType.Takeout)
            //    result = orderService.GetTableDishInfoByOrderId(_tableInfo.OrderId, Globals.UserInfo.UserName);
            //else
            //    result = orderService.GetTableDishInfoes(_tableInfo.TableName, Globals.UserInfo.UserName);

            //if (!string.IsNullOrEmpty(result.Item1))
            //    return string.Format("获取餐台明细失败：{0}", result.Item1);

            //InfoLog.Instance.I("获取餐台所有信息完成。");
            //if (result.Item2 == null)
            //    return "没有获取到该餐台的账单信息。";

            //result.Item2.MemberInfo = Data.MemberInfo;//会员信息保留，不用再次查询了。
            //Data = result.Item2;
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

            return null;
            //InfoLog.Instance.I("开始设置成正常价...");
            //var orderService = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            //if (orderService == null)
            //    return "创建IOrderService服务失败。";

            //var setNormalPriceResult = orderService.SetNormalPrice(Data.OrderId);
            //return !string.IsNullOrEmpty(setNormalPriceResult) ? string.Format("设置成正常价失败：{0}", setNormalPriceResult) : null;
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
                FCash = CashAmount + BankAmount + DebitAmount + AlipayAmount,
                FWeChat = WechatAmount,
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
        /// 打印结账单和发票。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> PrintSettlementReportAndInvoice(object arg)
        {
            if (Data.TableType == EnumTableType.Outside || Data.TableType == EnumTableType.Room)
            {
                //BroadcastSettlementMsgAsync(); //转成后台进行发送。
            }

            else if (Data.TableType == EnumTableType.CFTable)
                BroadcastCoffeeSettlementMsgAsyc();

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
                WindowHelper.ShowDialog(new SetInvoiceAmountWindow(Data), OwnerWindow);
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
            return service.BackAllDish(Data.OrderId, Data.TableName, Globals.UserInfo.UserName, backDishReason);
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

            var result = service.GetOrderInfo(_tableInfo.OrderId, "", ((int)_isKeepOdd).ToString());
            if (!string.IsNullOrEmpty(result.Item1))
                return string.Format("获取餐台明细失败：{0}", result.Item1);

            InfoLog.Instance.I("获取餐台所有信息完成。");
            if (result.Item2 == null)
                return "没有获取到该餐台的账单信息。";

            OwnerWindow.Dispatcher.Invoke((Action)delegate
            {
                Data.CloneOrderData(result.Item2);//合并餐台账单明细(金额，菜，优惠)
                MemberCardNo = Data.MemberNo;
                RoundingAmount = Data.RoundAmount;
                WipeOddAmount = Data.RemovezeroAmount;
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

            //新后台接口每次获取会返回发票抬头，注销此业务

            //InfoLog.Instance.I("开始获取订单{0}的发票抬头。", Data.OrderId);
            //var invoiceResult = service.GetOrderInvoice(Data.OrderId);
            //if (!string.IsNullOrEmpty(invoiceResult.Item1))
            //    return string.Format("获取订单发票抬头失败。" + invoiceResult.Item1);

            //InfoLog.Instance.I("结束获取订单发票抬头：{0}。", invoiceResult.Item2);
            //Data.OrderInvoiceTitle = invoiceResult.Item2;

            return null;
        }

        /// <summary>
        ///  获取餐台菜品信息执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void GetOrderInfoComplete(object param)
        {
            var result = (string)param;
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
                result.Item2.ForEach(t =>
                {
                    SelectedCouponCategory.CouponInfos.Add(t);
                    CouponInfos.Add(t);
                });
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
                InfoLog.Instance.I("广播清台消息给PAD...");
                CommonHelper.BroadcastMessageAsync(EnumBroadcastMsgType.ClearTable, Data.OrderId);
            }
            NotifyDialog.Notify("取消账单完成。", OwnerWindow.Owner);
            CloseWindow(true);
            return null;
        }

        #endregion

        #endregion
    }
}