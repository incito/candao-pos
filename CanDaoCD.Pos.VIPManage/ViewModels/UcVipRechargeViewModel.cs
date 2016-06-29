using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using CanDaoCD.Pos.Common.Classes.Mvvms;
using CanDaoCD.Pos.VIPManage.Models;
using CanDaoCD.Pos.Common.Controls.CSystem;
using CanDaoCD.Pos.VIPManage.Views;
using CanDaoCD.Pos.Common.Models;
using CanDaoCD.Pos.Common.Operates;
using CanDaoCD.Pos.PrintManage;
using CanDaoCD.Pos.VIPManage.Operates;
using Common;
using Models;
using WebServiceReference;
using Models.CandaoMember;
using System.ComponentModel;

namespace CanDaoCD.Pos.VIPManage.ViewModels
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

        public UserControlBase GetUserCtl()
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
            var couponList = CanDaoMemberClient.GetCouponList(Globals.branch_id);
            foreach (var coupon in couponList.Coupons)
            {
                var info = new MListBoxInfo();
                info.Title = string.Format("充值{0},赠送{1}", coupon.DealValue, coupon.PresentValu);
                info.ListData = coupon;
                info.Id = coupon.Id;
                boxList.Add(info);
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
                var data = listBoxInfo.ListData as TCandaoCoupon;
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
                        _userControl.Start();
                        if (Model.IsEnabledNum)
                        {
                            var info = CanDaoMemberClient.QueryBalance(Globals.branch_id, "", Model.TelNum, "");
                            if (info.Retcode.Equals("0"))
                            {
                                SelectModel.UserName = info.Name;
                                if (info.Gender == 0)
                                {
                                    SelectModel.Sex = "男";
                                }
                                else
                                {
                                    SelectModel.Sex = "女";
                                }
                                SelectModel.TelNum = info.Mobile;
                                SelectModel.Birthday = info.Birthday;
                                SelectModel.Integral = info.Integraloverall.ToString();
                                SelectModel.Balance = info.Storecardbalance.ToString();
                                SelectModel.CardNum = info.Mcard;

                            }
                            else
                            {
                                OWindowManage.ShowMessageWindow(
                                    string.Format("会员查询错误：{0}", info.Retinfo), true);

                                return;
                            }
                        }
                        if (PrintService.CardCheck())
                        {
                            //会员注册
                            decimal amount = 0;
                            int typeRecharge = Model.IsBankCard == true ? 1 : 0;
                            decimal.TryParse(Model.RechargeValue, out amount);
                            string id = string.Empty;
                            if (_selectInfo != null)
                            {
                                id = _selectInfo.Id;
                            }

                            TCandaoRet_StoreCardDeposit ret =
                                CanDaoMemberClient.StoreCardDepositAddCard(Globals.branch_id, "", Model.TelNum,
                                    Globals.branch_id, amount, 0, typeRecharge, id, Model.GiveValue);

                            if (ret.Ret)
                            {
                                _ret = ret;

                                AsyncLoadServer asyncLoadServer = new AsyncLoadServer();
                                asyncLoadServer.Init();
                                asyncLoadServer.ActionWorkerState = new Action<int>(WorkOk);
                                asyncLoadServer.Start(Print);
                                asyncLoadServer.SetMessage("正在打印复写卡，请稍等... ...");

                            }
                            else
                            {
                                OWindowManage.ShowMessageWindow(
                                    string.Format("注册失败：{0}", ret.Retinfo), false);

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        OWindowManage.ShowMessageWindow(
                            string.Format("注册失败：{0}", ex.Message), false);
                    }
                    finally
                    {
                        _userControl.stop();
                    }
                }
            }
        }

        #region 异步
        private TCandaoRet_StoreCardDeposit _ret;

        private void Print()
        {
            Model.CardBalance = string.Format("卡余额:{0}", _ret.StoreCardbalance);

            PrintVipStore(_ret.Tracecode, _ret.StoreCardbalance.ToString()); //纸质打印

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
            decimal oldRecharge = _ret.StoreCardbalance - recharge;


            //复写卡打印
            PrintService.RechargePrint(SelectModel.UserName, Model.TelNum, oldRecharge.ToString(),
                recharge.ToString(), _ret.StoreCardbalance.ToString(),SelectModel.Integral);

        }

        private void WorkOk(int res)
        {
            OWindowManage.ShowMessageWindow(
                string.Format("储值成功,交易流水号:{0}", _ret.Tracecode), false);
            CloseHandel();
        }

        #endregion

        /// <summary>
        /// 取消
        /// </summary>
        private void CloseHandel()
        {
            try
            {
                if (_userControl.UcClose != null)
                {
                    _userControl.UcClose();
                }
            }
            catch
            {

            }

        }

        /// <summary>
        /// 纸质打印
        /// </summary>
        private void PrintVipStore(string tracecode, string storeCardbalance)
        {
            TMemberStoredInfo memberstoreinfo = new TMemberStoredInfo();
            memberstoreinfo.Cardno = Model.TelNum;
            memberstoreinfo.Treport_membertitle =
                WebServiceReference.WebServiceReference.Report_membertitle;
            memberstoreinfo.Pzh = tracecode;
            DateTime date = DateTime.Now;
            date = DateTime.Now;
            string datestr = string.Format("{0:yyyy-MM-dd}", date);
            memberstoreinfo.Date = datestr;
            datestr = string.Format("{0:hh:mm}", date);
            memberstoreinfo.Time = datestr;
            memberstoreinfo.Store = storeCardbalance;
            memberstoreinfo.Point = "0"; // ret.Integral.ToString();//ret.Giftamount.ToString();
            memberstoreinfo.Amount = Model.RechargeValue;
            ReportsFastReport.ReportPrint.PrintMemberStore(memberstoreinfo);
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
            if (int.Parse(Model.RechargeValue) == 0)
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
            var width = _userControl.ActualWidth - 80;
            var height = _userControl.ActualHeight;
            int rowNum = (int) width/70;
            rowNum = rowNum > 0 ? rowNum : 1;
            int colNum = (int) height/70;
            colNum = colNum > 0 ? colNum : 1;
            _page = 1;
            _pageSize = rowNum - 1;
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
