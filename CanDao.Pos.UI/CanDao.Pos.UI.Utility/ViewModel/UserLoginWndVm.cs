using System;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 用户登录窗口Vm。
    /// </summary>
    public class UserLoginWndVm : NormalWindowViewModel
    {
        #region Fields

        /// <summary>
        /// 权限枚举。
        /// </summary>
        private readonly EnumRightType _rightType;

        #endregion

        #region Constructor

        public UserLoginWndVm()
        {
            _rightType = EnumRightType.Login;
            Account = "002";
            Password = "123456";
        }

        #endregion

        #region Properties

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
            TaskService.Start(new Tuple<string, string>(Account, Password), LoginProcess, LoginComplete, "授权验证中...");
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
            var info = (Tuple<string, string>)param;
            var service = ServiceManager.Instance.GetServiceIntance<IAccountService>();
            if (service == null)
            {
                var msg = "创建IAccountService服务接口失败。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            return service.Login(info.Item1, info.Item2, _rightType);
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
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            Globals.UserInfo.UserName = Account;
            Globals.UserInfo.Password = Password;
            Globals.UserInfo.FullName = result.Item2;
            TaskService.Start(Account, GetUserRightProcess, GetUserRightComplete, "获取用户权限中...");
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
        private void GetUserRightComplete(object param)
        {
            var result = (Tuple<string, UserRight>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1, OwnerWindow);
            }

            Globals.UserRight.CloneDataFrom(result.Item2);
            if (Globals.UserRight.AllowCash)
                TaskService.Start(Account, CheckPettyCashInputProcess, CheckPettyCashInputComplete, "检测零找金...");
            else
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
                CloseWindow(false);
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