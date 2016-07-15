using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CanDaoCD.Pos.Common.Classes.Mvvms;
using CanDaoCD.Pos.Common.Controls.CSystem;
using CanDaoCD.Pos.Common.Models.VipModels;
using CanDaoCD.Pos.Common.Operates;
using CanDaoCD.Pos.VIPManage.Models;
using CanDaoCD.Pos.VIPManage.Operates;
using CanDaoCD.Pos.VIPManage.Views;
using Common;
using Models.CandaoMember;
using WebServiceReference;

namespace CanDaoCD.Pos.VIPManage.ViewModels
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

                _vipInfo = CanDaoMemberClient.VipQuery(Globals.branch_id, "", Model.SelectNum);

                if (_vipInfo.Result)
                {
                 
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
                    Model.Integral = _vipInfo.CardInfos[0].Integral.ToString();
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
                        string.Format("会员查询错误：{0}", _vipInfo.ResultInfo), false);

                    Model.TelNum = "";
                    Model.CardType = 0;
                    Model.Balance = "";
                    Model.Integral = "";
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
                    string.Format("会员查询错误[{0}-{1}]：{2}", Globals.branch_id, Model.SelectNum, ex.Message), false);
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
                if (OWindowManage.ShowPopupWindow(modifyVipInfo.GetUserCtl()))
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
                if (OWindowManage.ShowPopupWindow(modifyTelNum.GetUserCtl()))
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
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
        }
        /// <summary>
        /// 充值
        /// </summary>
        private void StoredValueHandel()
        {
            var veModel = new UcVipRechargeViewModel(Model);
   
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
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

                if (window.ShowDialog() == true)
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
                    var resInfo = CanDaoMemberClient.CardCancellation(Globals.branch_id, "", Model.CardNum, "", "");
                    if (resInfo.Ret)
                    {
                        OWindowManage.ShowMessageWindow(
                            string.Format("[{0}]注销成功！", Model.CardNum), false);
                        Model = new UcVipSelectModel();
                    }
                    else
                    {
                        OWindowManage.ShowMessageWindow(
                           string.Format("注销失败：[{0}]", resInfo.Retinfo), false);
                    }
                }
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow(
             string.Format("会员注销失败[{0}-{1}]：{2}", Globals.branch_id, Model.CardNum, ex.Message), false);
            }
           
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取用户控件
        /// </summary>
        /// <returns></returns>
        public UserControlBase GetUserCtl()
        {
            _userControl = new UcVipSelectView();
            _userControl.DataContext = this;
            return _userControl;
        }

        #endregion
    }
}
