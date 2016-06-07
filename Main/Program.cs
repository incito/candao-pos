using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Common;
using DevExpress.Skins;
using DevExpress.UserSkins;
using KYPOS;
using Library;
using Models.Enum;
using ReportsFastReport;
using WebServiceReference;
using WebServiceReference.IService;
using WebServiceReference.ServiceImpl;

/************************************************************************* 
 * 程序说明: 程序入口
 * 注意初始化程序的代码顺序!!! 
 * 作者：
 **************************************************************************/

namespace Main
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Globals.ProductVersion = Application.ProductVersion;
            frmStart.ShowStart();
            frmStart.frm.Update();
            Thread.Sleep(50);
            frmStart.frm.setMsg("加载样式...");
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);//捕获系统所产生的异常。
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            frmStart.frm.setMsg("检测实例...");
            Process instance = RunningInstance();
            if (instance != null)//已经有一个实例在运行
            {
                HandleRunningInstance(instance);
                return;
            }
            frmStart.frm.setMsg("读取配置文件...");
            SystemConfig.ReadSettings(); //读取用户自定义设置

            BonusSkins.Register();//注册Dev酷皮肤
            //OfficeSkins.Register();////注册Office样式的皮肤
            SkinManager.EnableFormSkins();//启用窗体支持换肤特性
            RestClient.GetSoapRemoteAddress();

            var netResult = RestClient.CheckServerConnection();
            if (!string.IsNullOrEmpty(netResult))
            {
                var frm = new FrmRetry(netResult);
                if (frm.ShowDialog() == DialogResult.Cancel)
                    return;
            }

            netResult = RestClient.CheckDataServerConnection();
            if (!string.IsNullOrEmpty(netResult))
            {
                Msg.ShowError(netResult);
                return;
            }

            ReportPrint.Init();
            frmStart.frm.setMsg("获取系统设置...");
            try
            {
                RestClient.getSystemSetData(out Globals.roundinfo);
            }
            catch
            {
                // ignored
            }

            frmStart.frm.setMsg("获取营业时间...");
            IRestaurantService service = new RestaurantServiceImpl();
            try
            {
                var result = service.GetRestaurantTradeTime();
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    frmBase.Warning(result.Item1);
                    return;
                }

                Globals.TradeTime = result.Item2;
            }
            catch (Exception)
            {
                // ignored
            }

            frmStart.frm.setMsg("检查之前是否结业...");
            var endworkResult = service.CheckWhetherTheLastEndWork();
            if (!string.IsNullOrEmpty(endworkResult.Item1))
            {
                frmBase.Warning(endworkResult.Item1);
                return;
            }
            if (!endworkResult.Item2)//未结业走强制结业流程。
            {
                var allTableResult = service.GetAllTableInfoes();
                if (!string.IsNullOrEmpty(allTableResult.Item1))
                {
                    frmBase.Warning(allTableResult.Item1);
                    return;
                }

                if (allTableResult.Item2.Any(t => t.TableStatus == EnumTableStatus.Dinner))//还有就餐的餐台，就让收银员登录，结账。
                {
                    frmBase.Warning("昨日还有未结账餐台，请先登录收银员账号并结账，然后进行清机和结业。");
                    while (true)
                    {
                        var loginResult = CashierLogin();
                        if (!loginResult.HasValue)
                        {
                            frmBase.Warning("请登录收银员账号以便结账餐台。");
                            continue;
                        }

                        if (!loginResult.Value)//取消登录。
                            return;

                        MainForm.Show();
                        MainForm.SetInForcedEndWorkModel();
                        Application.Run();
                        return;
                    }
                }

                if (!CommonHelper.ClearAllMachine(true))
                    return;

                frmBase.Warning("昨日还有未结业，请先结业。");
                CommonHelper.EndWork();
                return;
            }

            //如果还没有开业，提示开业授权
            string reinfo = "";
            frmStart.frm.setMsg("检查是否开业...");
            try
            {
                if (!RestClient.OpenUp("", "", 0, out reinfo))
                {
                    //Thread.Sleep(1000);
                    try
                    { frmStart.frm.Close(); }
                    catch { }
                    if (!frmPermission.ShowPermission())
                    {
                        Application.Exit();
                        return;
                    }
                    else
                    {
                        //经理权限开业
                        if (!frmPermission2.ShowPermission2("开业经理授权", EnumRightType.OpenUp))
                        {
                            Application.Exit();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.ShowException(ex);
                Application.Exit();
                return;
            }
            //注意：先打开登陆窗体,登陆成功后正式运行程序(MDI主窗体)
            frmStart.frm.setMsg("开始登录...");

            if (frmLogin.Login())
            {
                //如果没有收银权限，那不用输入零找金
                if (Globals.userRight.getSyRigth())
                {
                    if (!frmPosMainV3.checkInputTellerCash())
                    {
                        Application.Exit();
                        return;
                    }
                }
                MainForm.Show();
                Application.Run();
            }
            else//登录失败,退出程序
                Application.Exit();
        }

        /// <summary>
        /// 收银员登录，如果登录成功返回true，不是收银员返回null，登录失败返回false。
        /// </summary>
        /// <returns></returns>
        private static bool? CashierLogin()
        {
            if (frmLogin.Login())
            {
                if (!Globals.userRight.getSyRigth())
                    return null;

                return frmPosMainV3.checkInputTellerCash();
            }
            return false;
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {

        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            AllLog.Instance.E(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);
            Msg.ShowException(e.Exception);//处理系统异常
        }

        static Program()
        {
            MainForm = null;
        }

        /// <summary>
        /// MDI主窗体
        /// </summary>        
        public static frmAllTable MainForm { get; set; }

        /// <summary>
        ///检查程序是否运行多实例
        /// </summary>
        private static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表 
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程 
                if (process.Id != current.Id)
                {
                    //保证要打开的进程同已经存在的进程来自同一文件路径
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", @"\") == current.MainModule.FileName)
                    {
                        //返回已经存在的进程
                        return process;
                    }
                }
            }
            return null;
        }
        //3.已经有了就把它激活，并将其窗口放置最前端
        private static void HandleRunningInstance(Process instance)
        {
            //MessageBox.Show("已经在运行！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowWindowAsync(instance.MainWindowHandle, 1);  //调用api函数，正常显示窗口
            SetForegroundWindow(instance.MainWindowHandle); //将窗口放置最前端
        }
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

    }
}