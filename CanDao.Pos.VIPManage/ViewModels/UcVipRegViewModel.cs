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
using CanDao.Pos.Model.Response;
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
        public bool IsStartTime
        {
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
            CloseCommand = new RelayCommand(CloseHandel);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 发送验证码请求
        /// </summary>
        private void SendCodeHandel()
        {
            if (!OCheckFormat.IsMobilePhone(Model.TelNum))
            {
                MessageDialog.Warning("手机号码不正确，请检查！");
                return;
            }

            IsStartTime = true;
            _receiveCode = "";
            TaskService.Start(null, SendCodeProcess, SendCodeComplete, "发送验证码中...");
        }

        /// <summary>
        /// 绑定卡
        /// </summary>
        private void BindingCardHandel()
        {
            _winShowInfo = new WinShowInfoViewModel();
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
                        MessageDialog.Warning(string.Format("会员注册错误：{0}", res));

                        return;
                    }

                    CopyReportHelper.CardCheck();
                    TaskService.Start(null, RegistProcess, RegistComplete, "会员注册中...");
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Warning("会员注册失败：" + ex.MyMessage());
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
            MessageDialog.Warning(string.Format("会员注册成功!"));

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

                MessageDialog.Warning("手机号码为空或格式不正确，请检查！");
                return false;
            }
            if (string.IsNullOrEmpty(Model.Code))
            {
                MessageDialog.Warning("验证码不能为空，请检查！");
                return false;
            }
            if (!Model.Code.Equals(_receiveCode))
            {
                MessageDialog.Warning("验证码错误！");
                return false;
            }
            if (string.IsNullOrEmpty(Model.UserName))
            {
                MessageDialog.Warning("姓名不能为空，请检查！");
                return false;
            }
            if (string.IsNullOrEmpty(Model.Psw))
            {
                MessageDialog.Warning("密码不能为空，请检查！");
                return false;
            }
            if (Model.Psw.Length < 6)
            {
                MessageDialog.Warning("密码长度不能少于6位，请检查！");
                return false;
            }
            if (Model.Psw != Model.PswConfirm)
            {
                MessageDialog.Warning("两次密码不相同，请检查！");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送验证码的执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object SendCodeProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, SendVerifyCodeResponse>("创建服务失败。", null);

            var request = new SendVerifyCodeRequest
            {
                branch_id = Globals.BranchInfo.BranchId,
                mobile = Model.TelNum,
            };
            return service.SendVerifyCode(request);
        }

        /// <summary>
        /// 发送验证码执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void SendCodeComplete(object param)
        {
            var result = (Tuple<string, SendVerifyCodeResponse>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E(result.Item1);
                MessageDialog.Warning(result.Item1);
                return;
            }

            _receiveCode = result.Item2.valicode;
        }

        private object RegistProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, SendVerifyCodeResponse>("创建服务失败。", null);

            var memberinfo = new CanDaoMemberRegistRequest
            {
                branch_id = Globals.BranchInfo.BranchId,
                mobile = Model.TelNum,
                cardno = Model.CardNum,
                password = Model.Psw.Trim(),
                name = Model.UserName,
                birthday = Model.Birthday.ToString("yyyy-MM-dd"),
                tenant_id = "",
                member_avatar = "",
                channel = "0",
                createuser = Globals.UserInfo.UserName,
                gender = Model.SexNan ? "0" : "1"
            };

            return service.Regist(memberinfo);
        }

        private void RegistComplete(object param)
        {
            var result = param as string;
            if (!string.IsNullOrEmpty(result))
            {
                var errMsg = string.Format("会员注册失败：{0}", result);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg);
                return;
            }

            Print();
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
