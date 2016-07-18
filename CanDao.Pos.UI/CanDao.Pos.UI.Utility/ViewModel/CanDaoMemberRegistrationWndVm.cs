using System;
using System.Timers;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class CanDaoMemberRegistrationWndVm : NormalWindowViewModel
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

        #endregion

        #region Constructor

        public CanDaoMemberRegistrationWndVm()
        {
            SendVerifyCodeBtnText = "发送";
            _allowSendVerifyCode = true;
            _currentRemainTime = DisableMaxTime;
            Birthday = new DateTime(1990, 1, 1);

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

        /// <summary>
        /// 用户输入的手机验证码。
        /// </summary>
        public string VerifyCode { get; set; }

        /// <summary>
        /// 用户姓名。
        /// </summary>
        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        /// <summary>
        /// 新密码。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 确认密码。
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 性别。
        /// </summary>
        public EnumGender Gender { get; set; }

        #endregion

        #region Command

        /// <summary>
        /// 发送验证码命令。
        /// </summary>
        public ICommand SendVerifyCodeCmd { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 发送验证码命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
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

        /// <summary>
        /// 发送验证码命令是否可用的判断方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
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

            var request = new CanDaoMemberCheckMobileRepeatRequest()
            {
                branch_id = Globals.BranchInfo.BranchId,
                mobile = Mobile,
            };
            var checkMobileRepeat = new WorkFlowInfo(CheckMobileRepeatProcess, CheckMobileRepeatComplete, "检测手机号中...")
            {
                NextWorkFlowInfo = new WorkFlowInfo(RegistProcess, RegistComplete, "会员注册中...")
            };
            WorkFlowService.Start(request, checkMobileRepeat);
        }

        /// <summary>
        /// 定时器触发时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
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
            if (Password.Length < 6)
                return "密码长度不足6位";
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
        /// 生成会员注册请求类。
        /// </summary>
        /// <returns></returns>
        private CanDaoMemberRegistRequest GenerateRegistRequest()
        {
            return new CanDaoMemberRegistRequest
            {
                birthday = Birthday.ToString("yyyy-MM-dd"),
                branch_id = Globals.BranchInfo.BranchId,
                createuser = Globals.UserInfo.FullName,
                gender = ((int)Gender).ToString(),
                mobile = Mobile,
                name = Name,
                password = Password,
            };
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
        /// 检测手机号执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object CheckMobileRepeatProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            return service.CheckMobileRepeat((CanDaoMemberCheckMobileRepeatRequest)arg);
        }

        /// <summary>
        /// 检测手机号执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> CheckMobileRepeatComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                MessageDialog.Warning(result, OwnerWindow);
                return null;
            }

            return new Tuple<bool, object>(true, GenerateRegistRequest());
        }

        /// <summary>
        /// 会员注册执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object RegistProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            return service.Regist((CanDaoMemberRegistRequest)arg);
        }

        /// <summary>
        /// 会员注册执行完成。
        /// </summary>
        /// <param name="arg"></param>
        private Tuple<bool, object> RegistComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                MessageDialog.Warning(result, OwnerWindow);
                return null;
            }

            MessageDialog.Warning("注册成功。", OwnerWindow);
            CloseWindow(true);
            return null;
        }

        #endregion
    }
}