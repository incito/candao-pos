using System;
using System.Timers;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class CanDaoMemberModifyPwdWndVm : NormalWindowViewModel
    {
        #region Fields

        private const int DisableMaxTime = 60;//验证码发送禁用最大时间。

        /// <summary>
        /// 当前剩余禁用时间。
        /// </summary>
        private int _currentRemainTime;

        /// <summary>
        /// 允许发送验证码。
        /// </summary>
        private bool _allowSendVerifyCode;

        /// <summary>
        /// 后台验证的验证码。
        /// </summary>
        private string _verifyCode;

        /// <summary>
        /// 定时器。
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// 当前会员信息。
        /// </summary>
        private MemberInfo _memberInfo;

        #endregion

        #region Constructor

        public CanDaoMemberModifyPwdWndVm(MemberInfo memberInfo)
        {
            SendVerifyCodeBtnText = "发送";
            _allowSendVerifyCode = true;
            _currentRemainTime = DisableMaxTime;
            _memberInfo = memberInfo;
            Mobile = memberInfo.Mobile;

            _timer = new Timer(1000);
            _timer.Elapsed += TimerOnElapsed;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 发送验证码的按钮文本。
        /// </summary>
        private string _sendVerifyCodeBtnText;
        /// <summary>
        /// 发送验证码的按钮文本。
        /// </summary>
        public string SendVerifyCodeBtnText
        {
            get { return _sendVerifyCodeBtnText; }
            set
            {
                _sendVerifyCodeBtnText = value;
                RaisePropertiesChanged("SendVerifyCodeBtnText");
            }
        }

        /// <summary>
        /// 手机号。
        /// </summary>
        public string Mobile { get; set; }

        public string VerifyCode { get; set; }

        /// <summary>
        /// 新密码。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 确认密码。
        /// </summary>
        public string ConfirmPassword { get; set; }

        #endregion

        #region Command

        public ICommand SendVerifyCodeCmd { get; private set; }

        #endregion

        #region Command Methods

        private void SendVerifyCode(object arg)
        {
            var request = new SendVerifyCodeRequest()
            {
                branch_id = Globals.BranchInfo.BranchId,
                mobile = Mobile,
                securityCode = "",
            };
            TaskService.Start(request, SendVerifyCodeProcess, SendVerifyCodeComplete, "发送验证码中...");
        }

        private bool CanSendVerifyCode(object arg)
        {
            return _allowSendVerifyCode;
        }

        #endregion

        #region Protected Method

        protected override void InitCommand()
        {
            base.InitCommand();
            SendVerifyCodeCmd = CreateDelegateCommand(SendVerifyCode, CanSendVerifyCode);
        }

        protected override void Confirm(object param)
        {
            var msg = CheckInputValid();
            if (!string.IsNullOrEmpty(msg))
            {
                MessageDialog.Warning(msg, OwnerWindow);
                return;
            }

            var request = new CanDaoMemberModifyPasswordRequest
            {
                branch_id = Globals.BranchInfo.BranchId,
                birthday = _memberInfo.Birthday.ToString("yyyy-MM-dd"),
                cardno = _memberInfo.CardNo,
                gender = ((int)_memberInfo.Gender).ToString(),
                mobile = Mobile,
                name = _memberInfo.Name,
                password = Password,
                securitycode = "",
                member_avatar = "",
            };
            TaskService.Start(request, ModifyPasswordProcess, ModifyPasswordComplete, "修改密码中...");
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _currentRemainTime--;
            if (_currentRemainTime == 0)
            {
                _currentRemainTime = DisableMaxTime;
                _allowSendVerifyCode = true;
                SendVerifyCodeBtnText = "发送";
                _timer.Stop();
            }
            else
            {
                _allowSendVerifyCode = false;
                SendVerifyCodeBtnText = string.Format("发送({0})", _currentRemainTime);
            }

            OwnerWindow.Dispatcher.BeginInvoke((Action)CommandManager.InvalidateRequerySuggested);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 检测用户输入有效性。
        /// </summary>
        /// <returns>如果全部输入都有效则返回null，否则返回错误信息。</returns>
        private string CheckInputValid()
        {
            if (string.IsNullOrEmpty(Password))
                return "请输入密码。";
            if (string.IsNullOrEmpty(ConfirmPassword))
                return "请输入确认密码。";
            if (!Password.Equals(ConfirmPassword))
                return "两次输入密码不一致。";
            if (string.IsNullOrEmpty(_verifyCode))
                return "请发送验证码。";
            if (string.IsNullOrEmpty(VerifyCode))
                return "请输入验证码。";
            if (!VerifyCode.Equals(_verifyCode))
                return "手机验证码错误。";
            return null;
        }

        /// <summary>
        /// 发送验证码的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object SendVerifyCodeProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, SendVerifyCodeResponse>("创建IMemberService服务失败。", null);

            return service.SendVerifyCode((SendVerifyCodeRequest)arg);
        }

        /// <summary>
        /// 发送验证码执行完成。
        /// </summary>
        /// <param name="arg"></param>
        private void SendVerifyCodeComplete(object arg)
        {
            var result = (Tuple<string, SendVerifyCodeResponse>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            _timer.Start();
            _verifyCode = result.Item2.valicode;
        }

        /// <summary>
        /// 修改密码的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private object ModifyPasswordProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            return service.ModifyPassword((CanDaoMemberModifyPasswordRequest)arg);
        }

        /// <summary>
        /// 修改密码执行完成。
        /// </summary>
        /// <param name="arg"></param>
        private void ModifyPasswordComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                MessageDialog.Warning(result, OwnerWindow);
                return;
            }

            MessageDialog.Warning("修改成功。", OwnerWindow);
            CloseWindow(true);
        }

        #endregion
    }
}