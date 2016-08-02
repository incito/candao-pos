using System;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 授权窗口VM。
    /// </summary>
    public class AuthorizationWndVm : NormalWindowViewModel
    {
        #region Fields

        /// <summary>
        /// 权限枚举。
        /// </summary>
        private readonly EnumRightType _rightType;

        #endregion

        #region Constructor

        public AuthorizationWndVm(EnumRightType rightType, string userName)
        {
            _rightType = rightType;
            //Account = userName;
            Account = "002";
            Password = "123456";
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
                        return "开业权限验证";
                    case EnumRightType.AntiSettlement:
                        return "反结算权限验证";
                    case EnumRightType.Clearner:
                        return "清机权限验证";
                    case EnumRightType.EndWork:
                        return "结业权限验证";
                    case EnumRightType.FreeDish:
                        return "赠菜权限验证";
                    case EnumRightType.BackDish:
                        return "退菜权限验证";
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

        #endregion

        #region Protected Methods

        protected override void Confirm(object param)
        {
            var userLoginWf = new WorkFlowInfo(LoginProcess, LoginComplete, "用户身份验证...");
            var getUserRightWf = new WorkFlowInfo(GetUserRightProcess, GetUserRightComplete, "获取用户权限中...");
            userLoginWf.NextWorkFlowInfo = getUserRightWf;
            var arg = new Tuple<string, string>(Account, Password);
            WorkFlowService.Start(arg, userLoginWf);
        }

        protected override bool CanConfirm(object param)
        {
            return !string.IsNullOrEmpty(Account) && !string.IsNullOrEmpty(Password);
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
        private Tuple<bool, object> LoginComplete(object param)
        {
            var result = (Tuple<string, string>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return null;
            }

            if (_rightType != EnumRightType.Login)
            {
                Globals.Authorizer.UserName = Account;
                Globals.Authorizer.FullName = result.Item2;
            }

            return new Tuple<bool, object>(true, Account);
        }

        /// <summary>
        /// 执行获取用户权限方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object GetUserRightProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IAccountService>();
            if (service == null)
            {
                var msg = "创建授权登录服务接口失败。";
                ErrLog.Instance.E(msg);
                return new Tuple<string, UserRight>(msg, null);
            }

            return service.GetUserRight((string)param);
        }

        /// <summary>
        /// 获取用户权限执行完成后。
        /// </summary>
        /// <param name="param"></param>
        private Tuple<bool, object> GetUserRightComplete(object param)
        {
            var result = (Tuple<string, UserRight>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return null;
            }

            switch (_rightType)
            {
                case EnumRightType.Opening:
                    TaskService.Start(null, OpeningProcess, OpeningComplete, "开业中...");
                    break;
                case EnumRightType.Login:
                    Globals.UserRight.CloneDataFrom(result.Item2);
                    if (Globals.UserRight.AllowCash)
                        TaskService.Start(Account, CheckPettyCashInputProcess, CheckPettyCashInputComplete, "检测零找金...");
                    break;
                default:
                    CloseWindow(true);
                    break;
            }

            return null;
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

        /// <summary>
        /// 检测是否输入零找金的执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object CheckPettyCashInputProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, bool>("创建IRestaurantService服务接口失败！", false);

            return service.CheckPettyCashInput((string)param);
        }

        /// <summary>
        /// 检测是否输入零找金执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void CheckPettyCashInputComplete(object param)
        {
            var result = (Tuple<string, bool>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1, OwnerWindow);
                ErrLog.Instance.E(result.Item1);
                return;
            }

            if (!result.Item2)
            {
                CloseWindow(WindowHelper.ShowDialog(new PettyCashWindow(), OwnerWindow));
                return;
            }

            CloseWindow(true);
        }
        #endregion
    }
}