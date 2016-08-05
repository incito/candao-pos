using System;
using System.IO;
using System.Windows.Input;
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
        /// 保存登录信息的文件名。
        /// </summary>
        private const string LoginInfoFileName = "LoginInfo.pos";

        #endregion

        #region Constructor

        public UserLoginWndVm()
        {
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

        /// <summary>
        /// 是否保存登录信息。
        /// </summary>
        private bool _isSaveLoginInfo;
        /// <summary>
        /// 是否保存登录信息。
        /// </summary>
        public bool IsSaveLoginInfo
        {
            get { return _isSaveLoginInfo; }
            set
            {
                _isSaveLoginInfo = value;
                RaisePropertyChanged("IsSaveLoginInfo");
            }
        }

        #endregion

        #region Protected Methods

        protected override void Confirm(object param)
        {
            SaveLoginInfo();
            TaskService.Start(new Tuple<string, string>(Account, Password), LoginProcess, LoginComplete, "授权验证中...");
        }

        protected override bool CanConfirm(object param)
        {
            return !string.IsNullOrEmpty(Account) && !string.IsNullOrEmpty(Password);
        }

        protected override void OnWindowLoaded(object param)
        {
            LoadLoginInfo();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 加载保存的登录信息。
        /// </summary>
        private void LoadLoginInfo()
        {
            InfoLog.Instance.I("读取保存登录信息文件...");
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LoginInfoFileName);
            if (!File.Exists(file))
            {
                InfoLog.Instance.I("登录信息文件不存在。");
                IsSaveLoginInfo = false;
                ((UserLoginWindow)OwnerWindow).TbUserName.Focus();
                return;
            }

            try
            {
                var data = File.ReadAllText(file);
                if (!string.IsNullOrEmpty(data))
                {
                    IsSaveLoginInfo = true;
                    Account = data;
                }
                else
                {
                    ((UserLoginWindow)OwnerWindow).TbUserName.Focus();
                }
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("读取登录信息文件时异常。", ex);
            }
        }

        /// <summary>
        /// 保存登录信息。
        /// </summary>
        private void SaveLoginInfo()
        {
            try
            {
                var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LoginInfoFileName);
                if (IsSaveLoginInfo)
                    File.WriteAllText(file, Account);
                else
                    File.Delete(file);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("保存(删除)登录信息文件时异常。", ex);
            }
        }

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

            return service.Login(info.Item1, info.Item2, EnumRightType.Login);
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
            InfoLog.Instance.I("开始获取用户权限...");
            var service = ServiceManager.Instance.GetServiceIntance<IAccountService>();
            if (service == null)
            {
                var msg = "创建IAccountService服务失败。";
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
                ErrLog.Instance.E("获取用户权限失败：{0}。", result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
            }

            InfoLog.Instance.I("获取用户权限成功。");
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
            InfoLog.Instance.I("开始检测是否输入零找金...");
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
                ErrLog.Instance.E("检测时候输入零找金时错误：{0}。", result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                CloseWindow(false);
                return;
            }

            if (!result.Item2)
            {
                InfoLog.Instance.I("未输入零找金，显示零找金设置窗口...");
                CloseWindow(WindowHelper.ShowDialog(new PettyCashWindow(), OwnerWindow));
                return;
            }

            InfoLog.Instance.I("已经输入过零找金，关闭登录窗口。");
            CloseWindow(true);
        }

        #endregion
    }
}