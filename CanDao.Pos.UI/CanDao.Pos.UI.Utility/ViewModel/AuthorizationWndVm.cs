using System;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 授权窗口VM。
    /// </summary>
    public class AuthorizationWndVm : NormalWindowViewModel<AuthorizationWindow>
    {
        #region Fields

        /// <summary>
        /// 权限枚举。
        /// </summary>
        private readonly EnumRightType _rightType;

        #endregion

        #region Constructor

        public AuthorizationWndVm(EnumRightType rightType, string userName = "")
        {
            _rightType = rightType;
            Account = userName;
            //Password = "123456";

            AllowInputUserName = rightType != EnumRightType.Clearner;//清机不允许修改用户名。
        }

        #endregion

        #region Properties

        public string WindowNotice
        {
            get
            {
                switch (_rightType)
                {
                    case EnumRightType.Login:
                        return "用户登录";
                    case EnumRightType.Opening:
                        return "开业授权";
                    case EnumRightType.AntiSettlement:
                        return "反结算授权";
                    case EnumRightType.Clearner:
                        return "清机授权";
                    case EnumRightType.EndWork:
                        return "结业授权";
                    case EnumRightType.FreeDish:
                        return "赠菜授权";
                    case EnumRightType.BackDish:
                        return "退菜授权";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 账户。
        /// </summary>
        private string _account;
        /// <summary>
        /// 账户。
        /// </summary>
        public string Account
        {
            get { return _account; }
            set
            {
                _account = value;
                RaisePropertiesChanged("Account");
            }
        }

        /// <summary>
        /// 账户密码。
        /// </summary>
        private string _password;
        /// <summary>
        /// 账户密码。
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertiesChanged("Password");
            }
        }

        /// <summary>
        /// 是否允许输入用户名。
        /// </summary>
        public bool AllowInputUserName { get; set; }

        #endregion

        #region Protected Methods

        protected override void Confirm(object param)
        {
            var arg = new Tuple<string, string>(Account, Password);
            TaskService.Start(arg, LoginProcess, LoginComplete, "身份验证中...");
        }

        protected override bool CanConfirm(object param)
        {
            return !string.IsNullOrEmpty(Account) && !string.IsNullOrEmpty(Password);
        }

        protected override void OnWindowLoaded(object param)
        {
            OwnerWnd.TbUserName.Focus();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 执行授权登录方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object LoginProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IAccountService>();
            if (service == null)
            {
                var msg = "创建IAccountService服务接口失败。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            return service.Login(Account, Password, _rightType);
        }

        /// <summary>
        /// 授权登录执行完成后。
        /// </summary>
        /// <param name="param"></param>
        private void LoginComplete(object param)
        {
            var result = (Tuple<string, string>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("授权失败：{0}", result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            Globals.Authorizer.UserName = Account;
            Globals.Authorizer.FullName = result.Item2;

            if (_rightType == EnumRightType.Opening)
                TaskService.Start(null, OpeningProcess, OpeningComplete, "开业中...");
            else
                CloseWindow(true);
        }

        /// <summary>
        /// 执行开业方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object OpeningProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
            {
                var msg = "创建IRestaurantService服务接口失败。";
                ErrLog.Instance.E(msg);
                return msg;
            }
            return service.RestaurantOpening(Account, Password);
        }

        /// <summary>
        /// 执行开业完成后。
        /// </summary>
        /// <param name="param"></param>
        private void OpeningComplete(object param)
        {
            var result = param as string;
            if (!string.IsNullOrEmpty(result))
            {
                MessageDialog.Warning(result, OwnerWindow);
                return;
            }

            CloseWindow(true);
        }

        #endregion
    }
}