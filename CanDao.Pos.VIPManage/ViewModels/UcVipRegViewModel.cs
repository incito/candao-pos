using CanDao.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.Common.Controls.CSystem;
using CanDao.Pos.Common.Operates;
using CanDao.Pos.VIPManage.Models;
using CanDao.Pos.VIPManage.Views;
using CanDao.Pos.IService;
using CanDao.Pos.Model.Request;
using CanDao.Pos.ReportPrint;


namespace CanDao.Pos.VIPManage.ViewModels
{
    /// <summary>
    /// 会员注册
    /// </summary>
    public class UcVipRegViewModel : ViewModelBase
    {
        #region 字段

        private UcVipRegView _userControl;

        private WinShowInfoViewModel _winShowInfo;

        //接收验证码
        private string _receiveCode;
        private bool _isStartTime = false;

        IMemberService _memberService = null;
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
            _memberService = ServiceManager.Instance.GetServiceIntance<IMemberService>();
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

                var response = new SendVerifyCodeRequest();
                response.branch_id = Globals.BranchInfo.BranchId;
                response.mobile = Model.TelNum;

               var res=_memberService.SendVerifyCode(response);
                if (!string.IsNullOrEmpty(res.Item1))
                {
                    OWindowManage.ShowMessageWindow("发送失败，请重试！", false);
                }
                else
                {
                    _receiveCode = res.Item2.valicode;
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
            var window = _winShowInfo.GetShoWindow();

            if (WindowHelper.ShowDialog(window, _userControl))
            {
                Model.CardNum = _winShowInfo.Model.CardNum;
                Model.IsShowCardBut = false;
                Model.IsShowCardNum = true;
            }
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
                    var request = new CanDaoMemberCheckMobileRepeatRequest();
                    request.branch_id = Globals.BranchInfo.BranchId;
                    request.mobile = Model.TelNum;
                    //验证手机是否存在
                    var res = _memberService.CheckMobileRepeat(request);
                    if (!string.IsNullOrEmpty(res))
                    {
                        OWindowManage.ShowMessageWindow(string.Format("会员注册错误：{0}", res), false);

                        return;
                    }

                    CopyReportHelper.CardCheck();

                    //调用注册接口
                    var memberinfo = new CanDaoMemberRegistRequest();
                    memberinfo.branch_id = Globals.BranchInfo.BranchId;
                    memberinfo.mobile = Model.TelNum;
                    memberinfo.cardno = Model.CardNum;
                    memberinfo.password = Model.Psw.Trim();
                    memberinfo.name = Model.UserName;

                    if (Model.SexNan)
                    {
                        memberinfo.gender = "0";
                    }
                    else
                    {
                        memberinfo.gender = "1";
                    }

                    memberinfo.birthday = Model.Birthday.ToString("yyyy-MM-dd");
                    memberinfo.tenant_id = "";
                    memberinfo.member_avatar = "";
                    memberinfo.channel = "0";
                    memberinfo.createuser = Globals.UserInfo.UserName;
                    var ret = _memberService.Regist(memberinfo);

                    if (!string.IsNullOrEmpty(ret))
                    {
                        OWindowManage.ShowMessageWindow(string.Format("注册失败:{0}", ret), false);
                    }
                    else
                    {
                        Print();
                    }
                }
            }
            catch (Exception ex)
            {
                OWindowManage.ShowMessageWindow("会员注册失败：" + ex.MyMessage(), false);
            }

        }

        #region 异步
       
        private void Print()
        {
            //复写卡打印
            CopyReportHelper.RegPrint(Model.UserName, Model.TelNum, WorkOk);
        }
        /// <summary>
        /// 异步完成
        /// </summary>
        /// <param name="res"></param>
        private void WorkOk(int res)
        {
            OWindowManage.ShowMessageWindow(string.Format("会员注册成功!"), false);

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
        public UcVipRegView GetUserCtl()
        {
            _userControl = new UcVipRegView();
            _userControl.DataContext = this;
            return _userControl;
        }

        #endregion
    }
}
