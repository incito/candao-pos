using CanDaoCD.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CanDaoCD.Pos.Common.Controls.CSystem;
using CanDaoCD.Pos.Common.Operates;
using CanDaoCD.Pos.PrintManage;
using CanDaoCD.Pos.VIPManage.Models;
using CanDaoCD.Pos.VIPManage.Views;
using Common;
using Models;
using WebServiceReference;

namespace CanDaoCD.Pos.VIPManage.ViewModels
{
    /// <summary>
    /// 会员注册
    /// </summary>
    public class UcVipRegViewModel : ViewModelBase
    {
        #region 字段

        private UserControlBase _userControl;

        private WinShowInfoViewModel _winShowInfo;

        //接收验证码
        private string _receiveCode;
        private bool _isStartTime = false;
        #endregion

        #region 属性

        public UcVipRegModel Model { set; get; }

        /// <summary>
        /// 验证码发送按钮样式启动
        /// </summary>
        public bool IsStartTime {
            set
            {
                _isStartTime = value;
                RaisePropertyChanged(() => IsStartTime);
            }
            get { return _isStartTime; }

        }
        #endregion

        #region 事件

        /// <summary>
        /// 绑定卡事件
        /// </summary>
        public RelayCommand BindingCardCommand { set; get; }

        /// <summary>
        /// 绑定卡事件
        /// </summary>
        public RelayCommand SendCodeCommand { set; get; }

        /// <summary>
        /// 注册
        /// </summary>
        public RelayCommand RegCommand { set; get; }

        /// <summary>
        /// 取消
        /// </summary>
        public RelayCommand CloseCommand { set; get; }

        #endregion

        #region 构造函数

        public UcVipRegViewModel()
        {
            Model = new UcVipRegModel();
            BindingCardCommand = new RelayCommand(BindingCardHandel);
            RegCommand = new RelayCommand(RegHandel);
            SendCodeCommand = new RelayCommand(SendCodeHandel);
            CloseCommand=new RelayCommand(CloseHandel);
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 发送验证码请求
        /// </summary>
        private void SendCodeHandel()
        {
           
            if (OCheckFormat.IsMobilePhone(Model.TelNum))
            {
                IsStartTime = true;
                //调用会员接口
                _receiveCode = "";
                CanDaoMemberClient.SendAccountByMobile(Globals.branch_id, "", Model.TelNum, out _receiveCode);
                if (_receiveCode.Equals(""))
                {
                    OWindowManage.ShowMessageWindow("发送失败，请重试！", false);
                }
            }
            else
            {
                OWindowManage.ShowMessageWindow("手机号码不正确，请检查！",false);
            }
        }


        /// <summary>
        /// 绑定卡
        /// </summary>
        private void BindingCardHandel()
        {
            _winShowInfo= new WinShowInfoViewModel();
            _winShowInfo.OkReturn = new Action<string>(ReceiveCarNum);
            var window = _winShowInfo.GetShoWindow();
            window.Topmost = true;
            window.WindowStartupLocation= WindowStartupLocation.CenterScreen;
            window.Show();
        }

        /// <summary>
        /// 接收实体卡号
        /// </summary>
        /// <param name="carNum"></param>
        private void ReceiveCarNum(string carNum)
        {
            Model.CardNum = carNum;
            Model.IsShowCardBut = false;
            Model.IsShowCardNum = true;
        }
        /// <summary>
        /// 会员注册
        /// </summary>
        private void RegHandel()
        {
            try
            {
                if (IsCheckInput())
                {
                    //验证手机验证码
                    TCandaoRetBase ret2 = CanDaoMemberClient.ValidateTbMemberManager(Globals.branch_id, "",
                         Model.TelNum);
                    if (!ret2.Retcode.Equals("0"))
                    {
                        OWindowManage.ShowMessageWindow(string.Format("手机验证码错误：{0}", ret2.Retinfo), false);

                        return;
                    }

                    PrintService.CardCheck();
                    //if (PrintService.CardCheck())//判断复写卡是否在位
                    //{
                        //调用注册接口
                        TCandaoRegMemberInfo memberinfo = new TCandaoRegMemberInfo();
                        memberinfo.Branch_id = Globals.branch_id;
                        memberinfo.Securitycode = "";
                        memberinfo.Mobile = Model.TelNum;
                        memberinfo.Cardno = Model.CardNum;
                        memberinfo.Password = Model.Psw.Trim();
                        memberinfo.Name = Model.UserName.ToString();

                        if (Model.SexNan)
                        {
                            memberinfo.Gender = "0";
                        }
                        else
                        {
                            memberinfo.Gender = "1";
                        }

                        memberinfo.Birthday = Model.Birthday.ToString("yyyy-MM-dd");
                        memberinfo.Tenant_id = 0;
                        memberinfo.Regtype = 0;
                        memberinfo.Member_avatar = "";
                        TCandaoRetBase ret = CanDaoMemberClient.MemberReg(memberinfo);

                        if (!ret.Ret)
                        {
                            OWindowManage.ShowMessageWindow(string.Format("注册失败:{0}", ret.Retinfo), false);
                        }
                        else
                        {
                            Print();
                            //OWindowManage.ShowMessageWindow(string.Format("会员注册成功!"), false);
                    
                            //CloseHandel();
                        }
                    //}
                }
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow("会员注册失败：" + ex.Message, false);
            }

        }
        #region 异步
       
        private void Print()
        {
            //复写卡打印
            PrintService.RegPrint(Model.UserName, Model.TelNum, WorkOk);
        }
        /// <summary>
        /// 异步完成
        /// </summary>
        /// <param name="res"></param>
        private void WorkOk(int res)
        {
            OWindowManage.ShowMessageWindow(string.Format("会员注册成功!"), false);

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
        /// 检查输入项
        /// </summary>
        /// <returns></returns>
        private bool IsCheckInput()
        {
            if (!OCheckFormat.IsMobilePhone(Model.TelNum))
            {
                OWindowManage.ShowMessageWindow("手机号码为空或格式不正确，请检查！", false);
                return false;
            }
            if (string.IsNullOrEmpty(Model.Code))
            {
                OWindowManage.ShowMessageWindow("验证码不能为空，请检查！", false);
                return false;
            }
            if (!Model.Code.Equals(_receiveCode))
            {
                OWindowManage.ShowMessageWindow("验证码错误！", false);
                return false;
            }
            if (string.IsNullOrEmpty(Model.UserName))
            {
                OWindowManage.ShowMessageWindow("姓名不能为空，请检查！", false);
                return false;
            }
            if (string.IsNullOrEmpty(Model.Psw))
            {
                OWindowManage.ShowMessageWindow("密码不能为空，请检查！", false);
                return false;
            }
            if (Model.Psw.Length<6)
            {
                OWindowManage.ShowMessageWindow("密码长度不能少于6位，请检查！", false);
                return false;
            }
            if (Model.Psw != Model.PswConfirm)
            {
                OWindowManage.ShowMessageWindow("2个密码不一致，请检查！", false);
                return false;
            }
            return true;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取用户控件
        /// </summary>
        /// <returns></returns>
        public UserControlBase GetUserCtl()
        {
            _userControl = new UcVipRegView();
            _userControl.DataContext = this;
            return _userControl;
        }

        #endregion
    }
}
