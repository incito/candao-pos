using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CanDao.Pos.Common;
using CanDao.Pos.Common.Classes.Mvvms;
using CanDao.Pos.Common.Controls.CSystem;
using CanDao.Pos.Common.Models.VipModels;
using CanDao.Pos.Common.Operates;
using CanDao.Pos.IService;
using CanDao.Pos.Model.Request;
using CanDao.Pos.VIPManage.Models;
using CanDao.Pos.VIPManage.Views;


namespace CanDao.Pos.VIPManage.ViewModels
{
    public class UcVipSelectViewModel : ViewModelBase
    {
        #region 字段

        private UcVipSelectView _userControl;

        private WinShowInfoViewModel _winShowInfo;

        private MVipInfo _vipInfo;

        private Action _textEnterAction;

        //是否可操作
        private bool _isOper = false;

        private IMemberService _memberService = null;
        #endregion

        #region 属性

        public UcVipSelectModel Model { set; get; }

        /// <summary>
        /// 手机、会员卡回车事件
        /// </summary>
        public Action TextEnterAction
        {
            get { return _textEnterAction; }
            set
            {
                _textEnterAction = value;
                RaisePropertyChanged(() => TextEnterAction);
            }
        }

        /// <summary>
        /// 是否可操作
        /// </summary>
        public bool IsOper
        {
            get { return _isOper; }
            set
            {
                _isOper = value;
                RaisePropertyChanged(() => IsOper);
            }
        }
      
        #endregion

        #region 事件

        /// <summary>
        /// 修改密码
        /// </summary>
        public RelayCommand ModifyPswCommand { set; get; }

        /// <summary>
        /// 会员储值
        /// </summary>
        public RelayCommand StoredValueCommand { set; get; }

        /// <summary>
        /// 会员注销
        /// </summary>
        public RelayCommand LogOffCommand { set; get; }

        /// <summary>
        /// 绑定卡事件
        /// </summary>
        public RelayCommand BindingCardCommand { set; get; }

        /// <summary>
        /// 修改基本信息
        /// </summary>
        public RelayCommand ModifyInfoCommand { set; get; }

        /// <summary>
        /// 修改卡号
        /// </summary>
        public RelayCommand ModifyCardNumCommand { set; get; }
        /// <summary>
        /// 会员挂失
        /// </summary>
        public RelayCommand ReportLossCommand { set; get; }
        /// <summary>
        /// 修改手机号码
        /// </summary>
        public RelayCommand ModifyTelNumCommand { set; get; }

        /// <summary>
        /// 查询信息
        /// </summary>
        public RelayCommand SelectCardCommand { set; get; }
        #endregion

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        public UcVipSelectViewModel()
        {
            _memberService = ServiceManager.Instance.GetServiceIntance<IMemberService>();

            Model = new UcVipSelectModel();

            TextEnterAction = new Action(SelectHandel);
            StoredValueCommand=new RelayCommand(StoredValueHandel);
            LogOffCommand=new RelayCommand(LogOffHandel);
            ModifyPswCommand=new RelayCommand(ModifyPswHandel);
            BindingCardCommand = new RelayCommand(BindingCardHandel);
            ModifyCardNumCommand = new RelayCommand(ModifyCardNumHandel);
            ModifyInfoCommand = new RelayCommand(ModifyInfoHandel);
            ReportLossCommand = new RelayCommand(ReportLossNumHandel);
            ModifyTelNumCommand = new RelayCommand(ModifyTelNumHandel);

            SelectCardCommand = new RelayCommand(SelectHandel);

            IsOper = false;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 查询事件
        /// </summary>
        private void SelectHandel()
        {
            try
            {
                if (string.IsNullOrEmpty(Model.SelectNum))//检查查询不能为空
                {
                    OWindowManage.ShowMessageWindow(
                       string.Format("查询信息不能为空！"), false);
                    return;
                }

                CanDaoVipQueryRequest request =new CanDaoVipQueryRequest();
                request.cardno = Model.SelectNum;
                request.branch_id = Globals.BranchInfo.BranchId;

               var res = _memberService.VipQuery(request);

                if (string.IsNullOrEmpty(res.Item1))
                {
                    _vipInfo = res.Item2;
                    Model.TelNum = _vipInfo.TelNum;
                    Model.UserName = _vipInfo.VipName;

                    Model.Birthday = DateTime.Parse(_vipInfo.Birthday).ToString("yyyy-MM-dd");
                    if (_vipInfo.Sex == 0)
                    {
                        Model.Sex = "男";
                    }
                    else
                    {
                        Model.Sex = "女";
                    }

                    Model.CardLevel = _vipInfo.CardInfos[0].CardLevelName;
                    Model.Integral = _vipInfo.CardInfos[0].Integral;
                    Model.Balance = _vipInfo.CardInfos[0].Balance.ToString();
                    Model.CardNum = _vipInfo.CardInfos[0].CardNum;
                    Model.CardType = _vipInfo.CardInfos[0].CardType;
                    switch (_vipInfo.CardInfos[0].CardState)
                    {
                        case 0:
                        {
                            Model.CardState = "注销";
                           break; 
                        }
                        case 1:
                        {
                            Model.CardState = "正常";
                            break;
                        }
                        case 2:
                        {
                            Model.CardState = "挂失";
                            break;
                        }
                        default:
                        {
                            Model.CardState = "未知";
                            break;
                        }
                    }
                 
                    IsOper = true; //启用操作区域
                }
                else
                {
                    OWindowManage.ShowMessageWindow(
                        string.Format("会员查询错误：{0}", res.Item1), false);

                    Model.TelNum = "";
                    Model.CardType = 0;
                    Model.Balance = "";
                    Model.Integral = 0;
                    Model.Birthday = "";
                    Model.CardLevel = "";
                    Model.CardNum = "";
                    Model.CardState = "";
                    Model.UserName = "";
                    Model.Sex = "";

                    IsOper = false; //禁用操作区域
                }
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow(
                    string.Format("会员查询错误[{0}-{1}]：{2}", Globals.BranchInfo.BranchId, Model.SelectNum, ex.Message), false);
            }
        }
        /// <summary>
        /// 修改卡号
        /// </summary>
        private void ModifyCardNumHandel()
        {
            try
            {
                if (Model.CardType != 1)
                {
                    OWindowManage.ShowMessageWindow(
                      string.Format("该会员未绑定实体卡，不能进行修改。"), false);
                    return;
                }

                _winShowInfo = new WinShowInfoViewModel();

                var vipChangeInfo = new MVipChangeInfo();
                vipChangeInfo.TelNum = Model.TelNum;
                vipChangeInfo.CardNum = Model.CardNum;

                _winShowInfo.VipChangeInfo = vipChangeInfo;

                var window = _winShowInfo.GetShoWindow();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                _winShowInfo.Model.Title = "修改卡号-请刷卡";

                if (window.ShowDialog() == true)
                {
                    Model.CardNum = _winShowInfo.Model.CardNum;
                }
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow(
                      string.Format("修改卡号失败：{0}",ex.Message), false);
                return;
            }
           
        }

        /// <summary>
        /// 修改会员信息
        /// </summary>
        private void ModifyInfoHandel()
        {
            try
            {
                var vipChangeInfo = new MVipChangeInfo();
                vipChangeInfo.TelNum = Model.TelNum;
                vipChangeInfo.CardNum = Model.CardNum;
                vipChangeInfo.Birthday = Model.Birthday;
                vipChangeInfo.VipName = Model.UserName;

                if (Model.Sex.Equals("男"))
                {
                    vipChangeInfo.Sex = 0;
                }
                else
                {
                    vipChangeInfo.Sex = 1;
                }

                var modifyVipInfo = new UcVipModifyVipInfoViewModel(vipChangeInfo);

                if (WindowHelper.ShowDialog(modifyVipInfo.GetUserCtl(),_userControl))
                {
                    Model.UserName = modifyVipInfo.Model.UserName;
                    Model.Birthday = modifyVipInfo.Model.Birthday.ToString("yyyy-MM-dd");
                    if (modifyVipInfo.Model.SexNan)
                    {
                        Model.Sex = "男";
                    }
                    else
                    {
                        Model.Sex = "女";
                    }
                }
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow(
                    string.Format("修改会员信息失败：{0}", ex.Message), false);
                return;
            }
        }

        /// <summary>
        /// 会员挂失
        /// </summary>
        private void ReportLossNumHandel()
        {

        }

        /// <summary>
        /// 修改手机号码
        /// </summary>
        private void ModifyTelNumHandel()
        {
            try
            {


                var vipChangeInfo = new MVipChangeInfo();
                vipChangeInfo.TelNum = Model.TelNum;

                var modifyTelNum = new UcVipModifyTelNumViewModel(vipChangeInfo);
                if(WindowHelper.ShowDialog(modifyTelNum.GetUserCtl(),_userControl))
                {
                     Model.TelNum = modifyTelNum.Model.NTelNum;
                }
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow(
                    string.Format("修改手机号码失败：{0}", ex.Message), false);
                return;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        private void ModifyPswHandel()
        {
            var veModel = new UcVipModifyPswViewModel(Model.TelNum);

            WindowHelper.ShowDialog(veModel.GetUserCtl(), _userControl);
        }
        /// <summary>
        /// 充值
        /// </summary>
        private void StoredValueHandel()
        {
            var veModel = new UcVipRechargeViewModel(Model);
            WindowHelper.ShowDialog(veModel.GetUserCtl(), null);
        }

        /// <summary>
        /// 新增实体卡
        /// </summary>
        private void BindingCardHandel()
        {
            try
            {

                _winShowInfo = new WinShowInfoViewModel();
                _winShowInfo.InsideId = Model.TelNum;
                var window = _winShowInfo.GetShoWindow();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                _winShowInfo.Model.Title = "新增实体卡-请刷卡";

                if (WindowHelper.ShowDialog(window,_userControl))
                {
                    Model.CardNum = _winShowInfo.Model.CardNum;
                }
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow(
                    string.Format("新增实体卡失败：{0}", ex.Message), false);
                return;
            }
        }

        /// <summary>
        /// 会员注销
        /// </summary>
        private void LogOffHandel()
        {
            try
            {
                if (OWindowManage.ShowMessageWindow(
               string.Format("是否要注销[{0}]?", Model.UserName), true))
                {
                    var request = new CanDaoMemberReportLossRequest(Globals.BranchInfo.BranchId, Model.CardNum);

                    var resInfo = _memberService.Cancel(request);
                    if (string.IsNullOrEmpty(resInfo))
                    {
                        OWindowManage.ShowMessageWindow(
                            string.Format("[{0}]注销成功！", Model.CardNum), false);
                        Model = new UcVipSelectModel();
                    }
                    else
                    {
                        OWindowManage.ShowMessageWindow(
                           string.Format("注销失败：[{0}]", resInfo), false);
                    }
                }
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow(
             string.Format("会员注销失败[{0}-{1}]：{2}", Globals.BranchInfo.BranchId, Model.CardNum, ex.Message), false);
            }
           
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取用户控件
        /// </summary>
        /// <returns></returns>
        public UcVipSelectView GetUserCtl()
        {
            _userControl = new UcVipSelectView();
            _userControl.DataContext = this;
            return _userControl;
        }

        #endregion
    }
}
