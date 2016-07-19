using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.UI.MainView.View;
using CanDao.Pos.UI.Utility;
using CanDao.Pos.UI.Utility.View;
using DevExpress.Xpf.Core;

namespace CanDao.Pos.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        private readonly object _syncObj = new object();

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //ThemeManager.ApplicationThemeName = "Office2010Blue";
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            InfoLog.Instance.I("程序启动，当前主程序版本号：{0}", ResourceAssembly.GetName(false).Version);
            InfoLog.Instance.I("读取配置信息...");
            SystemConfigCache.LoadCfgFile();//它是系统级配置，必须最先初始化。
            ServiceAddrCache.LoadCfgFile();

            InfoLog.Instance.I("配置文件读取完毕。");
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            //_splashWnd = new SplashWindow();
            //_splashWnd.Show();

            InfoLog.Instance.I("检测与服务器的连接情况...");
            if (!CheckServerConnection())
            {
                Shutdown();
                return;
            }
            InfoLog.Instance.I("与服务器的连接正常。");

            GetBranchInfoAsync();
            GetBankInfosAsync();
            GetTradeTimeAsync();
            GetOddSettingAsync();
            GetDietSettingAsync();
            GetDinnerWareSettingAsync();

            TaskService.Start(null, CheckTheLastEndWorkProcess, CheckTheLastEndWorkComplete, "检测上次是否结业中...");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                var errMsg = "非WPF窗体线程异常：";
                ErrLog.Instance.E(ex);
                MessageDialog.Warning(errMsg + Environment.NewLine + ex.MyMessage() + Environment.NewLine + ex.StackTrace);
            }
            catch (Exception)
            {
                MessageDialog.Warning("不可恢复的WPF窗体线程异常，应用程序将退出！");
            }
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ErrLog.Instance.E(e.Exception);
            MessageDialog.Warning(e.Exception.MyMessage());
        }

        /// <summary>
        /// 检测与服务器的连接。
        /// </summary>
        /// <returns></returns>
        private bool CheckServerConnection()
        {
            if (!NetwrokHelper.DetectNetworkConnection(SystemConfigCache.JavaServer))
            {
                var msg = string.Format("与后台服务器：\"{0}\"连接失败。{1}请检查服务器是否开机，网络是否正常！", SystemConfigCache.JavaServer, Environment.NewLine);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg);
                return false;
            }
            if (!NetwrokHelper.DetectNetworkConnection(SystemConfigCache.DataServer))
            {
                var msg = string.Format("与DataServer服务器：\"{0}\"连接失败。{1}请检查服务器是否开机，网络是否正常！", SystemConfigCache.DataServer, Environment.NewLine);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 授权开业。
        /// </summary>
        /// <returns>授权成功返回true，否则返回false。</returns>
        private bool AuthorizeOpen()
        {
            var wnd = new RestaurantOpeningWindow();
            if (wnd.ShowDialog() != true)
                return false;

            var authorizationWnd = new AuthorizationWindow(EnumRightType.Opening);
            return authorizationWnd.ShowDialog() == true;
        }

        /// <summary>
        /// 收银员登录。
        /// </summary>
        /// <returns>登录成功返回true，不是收银员返回null，登录失败返回false。</returns>
        private bool? CashierLogin()
        {
            var loginWnd = new UserLoginWindow();//登录
            if (loginWnd.ShowDialog() == true)
            {
                if (!Globals.UserRight.AllowCash)
                    return null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 异步获取结账可选银行集合。
        /// </summary>
        private void GetBankInfosAsync()
        {

            ThreadPool.QueueUserWorkItem(t =>
            {
                InfoLog.Instance.I("异步获取可选银行信息...");
                var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
                if (service == null)
                {
                    ErrLog.Instance.E("创建IRestaurantService服务失败。");
                    return;
                }

                var result = service.GetAllBankInfos();
                InfoLog.Instance.I("异步获取可选银行信息完成。");
                if (!string.IsNullOrEmpty(result.Item1))
                    ErrLog.Instance.E("异步获取可选银行时错误：{0}", result.Item1);
                else
                    Globals.BankInfos = result.Item2;
            });
        }

        /// <summary>
        /// 异步获取营业时间。
        /// </summary>
        private void GetTradeTimeAsync()
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                InfoLog.Instance.I("异步获取营业时间...");
                var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
                if (service == null)
                {
                    ErrLog.Instance.E("创建IRestaurantService服务失败。");
                    return;
                }

                var result = service.GetTradeTime();
                InfoLog.Instance.I("异步获取营业时间完成。");
                if (!string.IsNullOrEmpty(result.Item1))
                    ErrLog.Instance.E("异步获取营业时间时错误：{0}", result.Item1);
                else
                    Globals.TradeTime = result.Item2;
            });
        }

        /// <summary>
        /// 异步获取分店信息。
        /// </summary>
        private void GetBranchInfoAsync()
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                InfoLog.Instance.I("异步获取分店信息...");
                var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
                if (service == null)
                {
                    ErrLog.Instance.E("创建IRestaurantService服务失败。");
                    return;
                }

                var result = service.GetBranchInfo();
                InfoLog.Instance.I("异步获取分店信息完成。");
                if (!string.IsNullOrEmpty(result.Item1))
                    ErrLog.Instance.E("异步获取分店信息时错误：{0}", result.Item1);
                else
                    Globals.BranchInfo = result.Item2;
            });
        }

        /// <summary>
        /// 异步获取抹零设置。
        /// </summary>
        private void GetOddSettingAsync()
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                InfoLog.Instance.I("异步获取抹零设置...");
                var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
                if (service == null)
                {
                    ErrLog.Instance.E("创建IRestaurantService服务失败。");
                    return;
                }

                var result = service.GetSystemSetData(EnumSystemDataType.ROUNDING);
                InfoLog.Instance.I("异步获取抹零设置完成。");
                if (!string.IsNullOrEmpty(result.Item1))
                    ErrLog.Instance.E("异步获取抹零设置信息时错误：{0}", result.Item1);
                else
                {
                    lock (_syncObj)
                    {
                        Globals.SystemSetDatas.AddRange(result.Item2);
                    }
                }

                lock (_syncObj)
                {
                    if (Globals.SystemSetDatas.Any(y => y == null))
                    {
                        MessageDialog.Warning("为空。");
                    }
                }

            });
        }

        /// <summary>
        /// 异步获取忌口设置。
        /// </summary>
        private void GetDietSettingAsync()
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                AllLog.Instance.I("异步获取忌口设置...");
                var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
                if (service == null)
                {
                    AllLog.Instance.E("创建IRestaurantService服务失败。");
                    return;
                }

                var result = service.GetSystemSetData(EnumSystemDataType.JI_KOU_SPECIAL);
                AllLog.Instance.I("异步获取忌口设置完成。");
                if (!string.IsNullOrEmpty(result.Item1))
                    AllLog.Instance.E("异步获取忌口设置信息时错误：{0}", result.Item1);
                else
                {
                    lock (_syncObj)
                    {
                        Globals.SystemSetDatas.AddRange(result.Item2);
                    }
                }
            });
        }
        /// <summary>
        /// 异步获取餐具设置。
        /// </summary>
        private void GetDinnerWareSettingAsync()
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                InfoLog.Instance.I("异步获取餐具设置...");
                var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
                if (service == null)
                {
                    ErrLog.Instance.E("创建IRestaurantService服务失败。");
                    return;
                }

                var result = service.GetSystemSetData(EnumSystemDataType.DISHES);
                InfoLog.Instance.I("异步获取餐具设置完成。");
                if (!string.IsNullOrEmpty(result.Item1))
                    ErrLog.Instance.E("异步获取抹零设置信息时错误：{0}", result.Item1);
                else
                {
                    lock (_syncObj)
                    {
                        Globals.SystemSetDatas.AddRange(result.Item2);
                    }
                }

                lock (_syncObj)
                {
                    if (Globals.SystemSetDatas.Any(y => y == null))
                    {
                        MessageDialog.Warning("为空。");
                    }
                }
            });
        }

        /// <summary>
        /// 检测上次是否结业的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object CheckTheLastEndWorkProcess(object arg)
        {
            InfoLog.Instance.I("检测上次是否结业...");
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, bool>("创建IRestaurantService服务失败。", false);

            return service.CheckWhetherTheLastEndWork();
        }

        /// <summary>
        /// 检测上次是否结业完成时执行。
        /// </summary>
        /// <param name="arg"></param>
        private void CheckTheLastEndWorkComplete(object arg)
        {
            var result = (Tuple<string, bool>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("检测上次是否结业失败。" + result.Item1);
                MessageDialog.Warning(result.Item1);
                Shutdown();
                return;
            }

            if (!result.Item2) //未结业走强制结业流程。
            {
                InfoLog.Instance.I("上次未结业，开始强制结业...");
                var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
                InfoLog.Instance.I("开始获取所有餐桌信息...");
                var allTableResult = service.GetAllTableInfoes();
                if (!string.IsNullOrEmpty(allTableResult.Item1))
                {
                    ErrLog.Instance.E("获取所有餐桌信息失败。" + result.Item1);
                    MessageDialog.Warning(allTableResult.Item1);
                    Shutdown();
                }

                if (allTableResult.Item2.Any(t => t.TableStatus == EnumTableStatus.Dinner)) //还有就餐的餐台，就让收银员登录，结账。
                {
                    InfoLog.Instance.I("上次还有未结餐台，登录收银员强制结账...");
                    MessageDialog.Warning("昨日还有未结账餐台，请先登录收银员账号结账，然后进行清机和结业。");
                    while (true)
                    {
                        var loginResult = CashierLogin();
                        if (!loginResult.HasValue)
                        {
                            MessageDialog.Warning("请登录收银员账号以便结账餐台。");
                            continue;
                        }

                        if (!loginResult.Value) //取消登录。
                        {
                            InfoLog.Instance.I("取消登录，退出程序。");
                            Shutdown();
                        }

                        break;
                    }

                    (new MainWindow(true)).ShowDialog();
                    InfoLog.Instance.I("强制结账完成。");
                    return;
                }

                InfoLog.Instance.I("强制所有POS清机...");
                if (!CommonHelper.ClearAllPos(true))
                {
                    InfoLog.Instance.I("还有POS机未清机。");
                    return;
                }

                InfoLog.Instance.I("强制清机完成，开始强制结业...");
                MessageDialog.Warning("昨日还有未结业，请结业。");
                EndWork();
                InfoLog.Instance.I("强制结业完成，关闭程序。");
                Shutdown();
            }
            else
            {
                InfoLog.Instance.I("上次已结业，开始检测今日是否开业...");
                TaskService.Start(null, CheckIsOpenProcess, CheckIsOpenComplete, "检测今日是否开业...");
            }
        }

        /// <summary>
        /// 检测今日是否开业的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object CheckIsOpenProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
            {
                ErrLog.Instance.E("创建IRestaurantService服务接口失败！");
                return new Tuple<string, bool>("创建IRestaurantOpeningService服务接口失败！", false);
            }

            return service.CheckRestaurantOpened();
        }

        /// <summary>
        /// 检测今日是否开业完成时执行。
        /// </summary>
        /// <param name="arg"></param>
        private void CheckIsOpenComplete(object arg)
        {
            var result = (Tuple<string, bool>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1);
                InfoLog.Instance.I("检测今日是否开业时错误：{0}，退出程序。", result.Item1);
                Shutdown();
                return;
            }

            if (!result.Item2) //没有开业则进行开业授权。
            {
                InfoLog.Instance.I("今日还未开业，进行开业授权...");
                if (!AuthorizeOpen())
                {
                    InfoLog.Instance.I("开业授权失败，退出程序。");
                    Shutdown();
                    return;
                }
                InfoLog.Instance.I("开业授权成功。");
            }
            else
            {
                InfoLog.Instance.I("今日已开业。");
            }

            InfoLog.Instance.I("开始用户登录...");
            if (CashierLogin() != false)
            {
                InfoLog.Instance.I("用户：{0}登录成功，显示主窗口。", Globals.UserInfo.UserName);
                Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
                (new MainWindow(false)).ShowDialog();
            }
            else
            {
                InfoLog.Instance.I("登录失败，退出程序。");
                Shutdown();
            }
        }

        private void EndWork()
        {
            AuthorizationWindow wnd = new AuthorizationWindow(EnumRightType.EndWork);
            if (!WindowHelper.ShowDialog(wnd))
                return;

            var request = new EndWorkRequest { UserId = Globals.UserInfo.UserName };
            TaskService.Start(request, EndWorkProcess, EndWorkComplete, "结业中...");
        }

        /// <summary>
        /// 结业执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object EndWorkProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return "创建IRestaurantService服务失败。";

            return service.EndWork((EndWorkRequest)param);
        }

        /// <summary>
        /// 结业执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void EndWorkComplete(object param)
        {
            var result = (string)param;
            if (!string.IsNullOrEmpty(result))
            {
                MessageDialog.Warning(result);
                return;
            }

            EndWorkSyncData();
        }

        /// <summary>
        /// 结业后异步同步数据。
        /// </summary>
        private void EndWorkSyncData()
        {
            TaskService.Start(null, EndWorkSyncDataProcess, EndWorkSyncDataComplete, "通知结业同步数据...");
        }

        /// <summary>
        /// 结业后异步同步数据的执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object EndWorkSyncDataProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return "创建IRestaurantService服务失败。";

            return service.EndWorkSyncData();
        }

        /// <summary>
        /// 结业后异步同步数据执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void EndWorkSyncDataComplete(object param)
        {
            var result = (string)param;
            if (!string.IsNullOrEmpty(result))
            {
                if (MessageDialog.Quest(result + Environment.NewLine + "上传数据失败，是否重新上传？" + Environment.NewLine + "\"确定\"重新上传，\"取消\"放弃上传。"))
                    EndWorkSyncData();
                return;
            }

            MessageDialog.Warning("结业成功。");
        }

    }
}
