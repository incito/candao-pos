using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using CanDao.Pos.Common;
using CanDao.Pos.Common.Classes.Mvvms;
using CanDao.Pos.VIPManage.Models;
using CanDao.Pos.VIPManage.Views;
using CanDao.Pos.Common.Models;
using CanDao.Pos.Common.Models.VipModels;
using CanDao.Pos.Common.Operates;
using CanDao.Pos.IService;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;
using CanDao.Pos.ReportPrint;
using CanDao.Pos.Model;

namespace CanDao.Pos.VIPManage.ViewModels
{
    public class UcVipRechargeViewModel : ViewModelBase
    {
        #region 字段

        private UcVipRechargeView _userControl;

        private Action<MListBoxInfo> _itemChangeAction;
        private Action<TextBox> _textEnterAction;

        private List<MListBoxInfo> _infos;

        //列表翻页
        private int _page = 1;
        private int Total;
        private int _pageSize = 1;
        private List<MListBoxInfo> ListData;
        private ObservableCollection<MListBoxInfo> _listBoxInfos;
        private MListBoxInfo _selectInfo;

        private IMemberService _memberService=null;

        private CanDaoMemberStorageResponse _ret;
        #endregion

        #region 属性

        public UcVipRechargeModel Model { set; get; }

        public ObservableCollection<MListBoxInfo> ListBoxInfos
        {
            set
            {
                _listBoxInfos = value;
                RaisePropertyChanged(() => ListBoxInfos);
            }
            get { return _listBoxInfos; }
        }

        public UcVipSelectModel SelectModel = null;

        #endregion

        #region 事件

        /// <summary>
        /// 输入框回车事件
        /// </summary>
        public Action<TextBox> TextEnterAction
        {
            get { return _textEnterAction; }
            set
            {
                _textEnterAction = value;
                RaisePropertyChanged(() => TextEnterAction);
            }
        }

        /// <summary>
        /// 优惠方式选择变化事件
        /// </summary>
        public Action<MListBoxInfo> ItemChangeAction
        {
            get { return _itemChangeAction; }
            set
            {
                _itemChangeAction = value;
                RaisePropertyChanged(() => ItemChangeAction);
            }
        }

        /// <summary>
        /// 绑定卡事件
        /// </summary>
        public RelayCommand TrunUpCommand { set; get; }

        /// <summary>
        /// 绑定卡事件
        /// </summary>
        public RelayCommand TrunDownCommand { set; get; }

        /// <summary>
        /// 注册
        /// </summary>
        public RelayCommand SureCommand { set; get; }

        /// <summary>
        /// 取消
        /// </summary>
        public RelayCommand CloseCommand { set; get; }

        #endregion

        #region 构造函数

        public UcVipRechargeViewModel(UcVipSelectModel model = null)
        {
            _memberService = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            Model = new UcVipRechargeModel();
            ListBoxInfos = new ObservableCollection<MListBoxInfo>();
            //ItemChangeAction = new Action<MListBoxInfo>(ItemChangeHandel);

            if (model != null)
            {
                SelectModel = model;
                Model.TelNum = model.TelNum;
                Model.IsEnabledNum = false;
            }
            else
            {
                SelectModel = new UcVipSelectModel();
            }

            TextEnterAction = new Action<TextBox>(TextEnterHandel);
            SureCommand = new RelayCommand(SureHandel);
            CloseCommand = new RelayCommand(CloseHandel);

            TrunUpCommand = new RelayCommand(TrunUpHandel);
            TrunDownCommand = new RelayCommand(TrunDownHandel);

            Init();
        }

        #endregion

        #region 公共方法

        public UcVipRechargeView GetUserCtl()
        {
            _userControl = new UcVipRechargeView();
            _userControl.DataContext = this;
            _userControl.Loaded += _userControl_Loaded;
            _userControl.SelectAction = new Action<MListBoxInfo>(ItemChangeHandel);
            _userControl.TexRechargeAction = new Action<string>(TexRechargeHandel);
            return _userControl;
        }

        void _userControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SetPageInfo();

        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 充值金额变化
        /// </summary>
        private void TexRechargeHandel(string textValue)
        {
            Model.RechargeValue = textValue;
            ItemChangeHandel(_selectInfo);
        }

        /// <summary>
        /// 输入框回车事件响应
        /// </summary>
        /// <param name="textBox"></param>
        private void TextEnterHandel(TextBox textBox)
        {
            switch (textBox.Name)
            {
                case "TexRecharge": //储值金额
                {
                    Model.RechargeValue = textBox.Text;
                    ItemChangeHandel(_selectInfo);
                    break;
                }
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void Init()
        {
            GetBoxList();
        }

        /// <summary>
        /// 获取优惠列表
        /// </summary>
        private void GetBoxList()
        {

            var boxList = new List<MListBoxInfo>();
            var couponList = _memberService.GetCouponList(Globals.BranchInfo.BranchId);
            if (string.IsNullOrEmpty(couponList.Item1))
            {
                foreach (var coupon in couponList.Item2)
                {
                    var info = new MListBoxInfo();
                    info.Title = string.Format("充值{0},赠送{1}", coupon.DealValue, coupon.PresentValu);
                    info.ListData = coupon;
                    info.Id = coupon.Id;
                    boxList.Add(info);
                }

            }
            ListData = boxList;
        }

        /// <summary>
        /// 优惠方式选择变化
        /// </summary>
        /// <param name="listBoxInfo"></param>
        private void ItemChangeHandel(MListBoxInfo listBoxInfo)
        {
            _selectInfo = listBoxInfo;
            if (listBoxInfo == null)
            {
                Model.GiveValue = "0";
            }
            else
            {

                //设置选择金额
                var data = listBoxInfo.ListData as MVipCoupon;
                float giveValue = 0;
                float present = 0;

                if (string.IsNullOrEmpty(data.DealValue)) //充值为空代表不论充值多少都送赠送金额
                {
                    if (float.TryParse(data.PresentValu, out present))
                    {
                        giveValue = present;
                    }
                }
                else
                {
                    float recharge = 0;
                    float deal = 0;

                    if (float.TryParse(Model.RechargeValue, out recharge) & float.TryParse(data.DealValue, out deal) &
                        float.TryParse(data.PresentValu, out present))
                    {

                        if (data.CouponType.Equals("1"))
                        {

                            giveValue = ((int) (recharge/deal))*present;
                        }
                        else
                        {
                            if (recharge > deal)
                            {
                                giveValue = deal;
                            }
                        }

                    }
                }

                Model.GiveValue = giveValue.ToString();
            }
        }

        /// <summary>
        ///充值
        /// </summary>
        private void SureHandel()
        {
            if (IsCheckInput())
            {
                if (
                    OWindowManage.ShowMessageWindow(
                        string.Format("确定为用户{0}充值{1}，赠送{2}吗？", Model.TelNum, Model.RechargeValue, Model.GiveValue), true))
                {

                    try
                    {
                      
                        if (Model.IsEnabledNum)
                        {
                            var request = new CanDaoMemberQueryRequest();
                            request.cardno = Model.TelNum;
                            request.branch_id = Globals.BranchInfo.BranchId;
                            request.securityCode = "";
                            request.password = "";
                            var info = _memberService.QueryCanndao(request);

                            if (string.IsNullOrEmpty(info.Item1))
                            {
                                SelectModel.UserName = info.Item2.Name;
                                if (info.Item2.Gender == 0)
                                {
                                    SelectModel.Sex = "男";
                                }
                                else
                                {
                                    SelectModel.Sex = "女";
                                }
                                SelectModel.TelNum = info.Item2.Mobile;
                                SelectModel.Birthday = info.Item2.Birthday.ToString("yyyy-MM-dd");
                                SelectModel.Integral = info.Item2.Integral;
                                SelectModel.Balance = info.Item2.StoredBalance.ToString();
                                SelectModel.CardNum = info.Item2.CardNo;

                            }
                            else
                            {
                                OWindowManage.ShowMessageWindow(
                                    string.Format("会员查询错误：{0}", info.Item1), true);

                                return;
                            }
                        }

                        CopyReportHelper.CardCheck();

                        //会员注册
                        decimal amount = 0;
                        int typeRecharge = Model.IsBankCard == true ? 1 : 0;
                        decimal.TryParse(Model.RechargeValue, out amount);
                        string id = string.Empty;
                        if (_selectInfo != null)
                        {
                            id = _selectInfo.Id;
                        }

                        var requestStorage = new CanDaoMemberStorageRequest();
                        requestStorage.cardno = Model.TelNum;
                        requestStorage.branch_id = Globals.BranchInfo.BranchId;
                        requestStorage.Amount = amount;
                        requestStorage.TransType = "0";
                        requestStorage.ChargeType = typeRecharge.ToString();
                        requestStorage.Serial = Globals.BranchInfo.BranchId;
                        requestStorage.preferential_id = id;
                        requestStorage.giveValue = Model.GiveValue;
                        var ret = _memberService.StorageCanDao(requestStorage);

                        if (string.IsNullOrEmpty(ret.Item1))
                        {
                            _ret = ret.Item2;
                            Print();
                        }
                        else
                        {
                            OWindowManage.ShowMessageWindow(
                                string.Format("注册失败：{0}", ret.Item1), false);
                        }
                    }
                    catch (Exception ex)
                    {
                        OWindowManage.ShowMessageWindow(
                            string.Format("注册失败：{0}", ex.MyMessage()), false);
                    }
                    finally
                    {
                     
                    }
                }
            }
        }

        #region 异步
       
        private void Print()
        {
            Model.CardBalance = string.Format("卡余额:{0}", _ret.StoreCardBalance);

            PrintVipStore(_ret.TraceCode, _ret.StoreCardBalance); //纸质打印

            decimal recharge = 0;

            decimal temp = 0;
            if (decimal.TryParse(Model.RechargeValue, out temp))
            {
                recharge = temp;
            }

            if (decimal.TryParse(Model.GiveValue, out temp))
            {
                recharge += temp;
            }
            decimal oldRecharge = _ret.StoreCardBalance - recharge;


            //复写卡打印
            CopyReportHelper.RechargePrint(SelectModel.UserName, Model.TelNum, oldRecharge.ToString(),
                recharge.ToString(), _ret.StoreCardBalance.ToString(), SelectModel.Integral.ToString(), WorkOk);

        }

        private void WorkOk(int res)
        {
            OWindowManage.ShowMessageWindow(
                string.Format("储值成功,交易流水号:{0}", _ret.TraceCode), false);
            _userControl.DialogResult = true;
        }

        #endregion

        /// <summary>
        /// 取消
        /// </summary>
        private void CloseHandel()
        {
            _userControl.DialogResult = false;
        }

        /// <summary>
        /// 纸质打印
        /// </summary>
        private void PrintVipStore(string tracecode, decimal storeCardbalance)
        {
            var memberstoreinfo = new PrintMemberStoredInfo();
            memberstoreinfo.CardNo = Model.TelNum;
            memberstoreinfo.ReportTitle = Globals.BranchInfo.BranchName;

            memberstoreinfo.TraceCode = tracecode;
            memberstoreinfo.TradeTime = DateTime.Now;
            memberstoreinfo.StoredBalance = storeCardbalance;
            memberstoreinfo.ScoreBalance = SelectModel.Integral;
            memberstoreinfo.StoredAmount = decimal.Parse(Model.RechargeValue);

           
            ReportPrintHelper.PrintMemberStoredReport(memberstoreinfo);
        }

        /// <summary>
        /// 检查输入项
        /// </summary>
        /// <returns></returns>
        private bool IsCheckInput()
        {
            float recharge;
            if (string.IsNullOrEmpty(Model.TelNum))
            {
                OWindowManage.ShowMessageWindow("手机号码或卡号不能为空，请检查！", false);
                return false;
            }
            if (!float.TryParse(Model.RechargeValue, out recharge))
            {
                OWindowManage.ShowMessageWindow("充值金额为空或格式不正确！", false);
                return false;
            }
            if (recharge.Equals(0))
            {
                OWindowManage.ShowMessageWindow("充值金额不能为零！", false);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 向上翻页
        /// </summary>
        private void TrunUpHandel()
        {
            _page--;
            ContentShow();
        }

        /// <summary>
        /// 向下翻页
        /// </summary>
        private void TrunDownHandel()
        {
            _page++;
            ContentShow();
        }

        /// <summary>
        /// 设置翻页控件属性
        /// </summary>
        private void SetPageInfo()
        {
            var width = 635;
            var height = 75;

            //行
            int rowNum = (int) height/70;
            rowNum = rowNum > 0 ? rowNum : 1;
            
            //列
            int colNum = (int)width / 70;
            colNum = colNum > 0 ? colNum : 1;

            _page = 1;

            //每页数量
            _pageSize = colNum*rowNum;
            _pageSize = _pageSize > 0 ? _pageSize : 1;

            ContentShow();

        }

        /// <summary>
        /// 显示数据
        /// </summary>
        private void ContentShow()
        {
            try
            {
                ListBoxInfos =
                    new ObservableCollection<MListBoxInfo>(ListData.Take(_pageSize*_page).Skip(_pageSize*(_page - 1)));
                if (ListData.Count%_pageSize == 0)
                {
                    Total = ListData.Count/_pageSize;
                }
                else
                {
                    Total = ListData.Count/_pageSize + 1;
                }
                Model.IsUp = _page > 1 ? true : false;
                Model.IsDown = _page < Total ? true : false;
            }
            catch
            {

            }

        }

        #endregion
    }
}
