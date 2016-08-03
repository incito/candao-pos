using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.UI.MainView.View;
using CanDao.Pos.UI.Utility;
using CanDao.Pos.UI.Utility.View;
using Timer = System.Timers.Timer;

namespace CanDao.Pos.UI.MainView.ViewModel
{
    public class MainWndVm : NormalWindowViewModel
    {
        #region Filed

        /// <summary>
        /// 刷新时间间隔（秒）
        /// </summary>
        private const int RefreshTimerInterval = 5;

        /// <summary>
        /// 刷新计时器。
        /// </summary>
        private readonly Timer _refreshTimer;

        /// <summary>
        /// 打印机检测间隔（秒）。
        /// </summary>
        private const int PrinterCheckTimerInterval = 10;

        /// <summary>
        /// 打印机检测定时器。
        /// </summary>
        private readonly Timer _printerCheckTimer;

        /// <summary>
        /// 下一次警告间隔时间（分）。
        /// </summary>
        private const int NextMaxWarningInterval = 10;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isForcedEndWorkModel">是否是强制结业模式。</param>
        public MainWndVm(bool isForcedEndWorkModel)
        {
            IsForcedEndWorkModel = isForcedEndWorkModel;
            Tables = new ObservableCollection<TableInfo>();
            CfTables = new ObservableCollection<TableInfo>();
            RefreshRemainSecond = RefreshTimerInterval;

            _refreshTimer = new Timer(1000) { Enabled = true };
            _refreshTimer.Elapsed += RefreshTimer_Elapsed;

            _printerCheckTimer = new Timer(PrinterCheckTimerInterval * 1000);
            _printerCheckTimer.Elapsed += PrinterCheckTimerOnElapsed;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 所有餐台集合。
        /// </summary>
        public ObservableCollection<TableInfo> Tables { get; private set; }

        /// <summary>
        /// 咖啡台集合。
        /// </summary>
        public ObservableCollection<TableInfo> CfTables { get; private set; }

        /// <summary>
        /// 空闲餐台数。
        /// </summary>
        private int _idleCount;
        /// <summary>
        /// 获取或设置空闲餐台数。
        /// </summary>
        public int IdleCount
        {
            get { return _idleCount; }
            set
            {
                _idleCount = value;
                RaisePropertiesChanged("IdleCount");
            }
        }

        /// <summary>
        /// 就餐餐台数。
        /// </summary>
        private int _dinnerCount;
        /// <summary>
        /// 获取或设置就餐餐台数。
        /// </summary>
        public int DinnerCount
        {
            get { return _dinnerCount; }
            set
            {
                _dinnerCount = value;
                RaisePropertiesChanged("DinnerCount");
            }
        }

        /// <summary>
        /// 刷新剩余时间。
        /// </summary>
        private int _refreshRemainSecond;
        /// <summary>
        /// 刷新剩余时间。
        /// </summary>
        public int RefreshRemainSecond
        {
            get { return _refreshRemainSecond; }
            set
            {
                _refreshRemainSecond = value;
                RaisePropertiesChanged("RefreshRemainSecond");
            }
        }

        /// <summary>
        /// 不可用餐台个数。
        /// </summary>
        private int _disableCount;
        /// <summary>
        /// 不可用餐台个数。
        /// </summary>
        public int DisableCount
        {
            get { return _disableCount; }
            set
            {
                _disableCount = value;
                RaisePropertyChanged("DisableCount");
            }
        }

        /// <summary>
        /// 是否是强制结业模式。
        /// </summary>
        public bool IsForcedEndWorkModel { get; set; }

        /// <summary>
        /// 是否有打印机错误。
        /// </summary>
        private bool _hasPrinterError;
        /// <summary>
        /// 是否有打印机错误。
        /// </summary>
        public bool HasPrinterError
        {
            get { return _hasPrinterError; }
            set
            {
                _hasPrinterError = value;
                RaisePropertyChanged("HasPrinterError");
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// 窗口加载时执行命令。
        /// </summary>
        public ICommand WindowLoadCmd { get; private set; }

        /// <summary>
        /// 窗口关闭命令。
        /// </summary>
        public ICommand WindowClosingCmd { get; private set; }

        /// <summary>
        /// 窗口关闭时执行命令。
        /// </summary>
        public ICommand WindowClosedCmd { get; private set; }

        /// <summary>
        /// 选择餐桌命令。
        /// </summary>
        public ICommand SelectTableCmd { get; private set; }

        /// <summary>
        /// 获取所有餐桌信息命令。   
        /// </summary>
        public ICommand GetAllTableInfoCmd { get; private set; }

        /// <summary>
        /// 操作命令。
        /// </summary>
        public ICommand OperCmd { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 窗口加载命令的执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void WindowLoad(object param)
        {
            if (!IsInDesignMode)
            {
                GetAllTableInfoesAsync();
                ThreadPool.QueueUserWorkItem(t => { CheckPrinterStatus(); });

                if (Globals.IsDinnerWareEnable)
                {
                    ThreadPool.QueueUserWorkItem(t =>
                    {
                        InfoLog.Instance.I("启用了餐具收费设置，开始获取餐具信息...");
                        var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
                        if (service == null)
                        {
                            ErrLog.Instance.E("创建IRestaurantService服务失败。");
                            return;
                        }

                        var result2 = service.GetDinnerWareInfo(Globals.UserInfo.UserName);
                        if (!string.IsNullOrEmpty(result2.Item1))
                            ErrLog.Instance.E(result2.Item1);

                        InfoLog.Instance.I("获取餐具信息完成。");
                        Globals.DinnerWareInfo = result2.Item2;
                    });
                }
            }
        }

        /// <summary>
        /// 窗口关闭时执行命令的执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void WindowClosing(object param)
        {
            ExCommandParameter cmdParam = (ExCommandParameter)param;
            if (!MessageDialog.Quest("确定要退出系统吗？"))
                ((CancelEventArgs)cmdParam.EventArgs).Cancel = true;
        }

        /// <summary>
        /// 窗口关闭后命令的执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void WindowClosed(object param)
        {
            if (_refreshTimer != null)//释放刷新定时器。
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            }

            if (_printerCheckTimer != null) //释放打印机状态定时器。
            {
                _printerCheckTimer.Stop();
                _printerCheckTimer.Dispose();
            }

            Application.Current.Shutdown(1);//尝试解决有时候退出主窗口程序依然在运行的问题。
        }

        /// <summary>
        /// 选择某个餐桌命令执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void SelectTable(object param)
        {
            var tableInfo = param as TableInfo;
            if (tableInfo == null)
                return;

            SetRefreshTimerStatus(false);
            if (!Globals.UserRight.AllowCash)
            {
                MessageDialog.Warning("您没有收银权限。", OwnerWindow);
                SetRefreshTimerStatus(true);
                return;
            }

            WindowHelper.ShowDialog(new TableOperWindow(tableInfo) { Owner = OwnerWindow });

            GetAllTableInfoesAsync();
            RefreshRemainSecond = RefreshTimerInterval;
        }

        /// <summary>
        /// 获取所有餐桌命令的执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void GetAllTableInfos(object param)
        {
            SetRefreshTimerStatus(false);
            GetAllTableInfoesAsync();
            RefreshRemainSecond = RefreshTimerInterval;
        }

        /// <summary>
        /// 操作命令的执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void OperMethod(object param)
        {
            SetRefreshTimerStatus(false);
            switch ((string)param)
            {
                case "Takeout"://外卖
                    TakeoutTable();
                    break;
                case "QueryOrderHistory"://账单历史
                    WindowHelper.ShowDialog(new QueryOrderHistoryWindow(), OwnerWindow);
                    break;
                case "Clearner"://清机
                    ClearnMachine();
                    break;
                case "EndWork"://结业
                    if (MessageDialog.Quest("确定要结业吗？"))
                        EndWork();
                    break;
                case "Report"://报表
                    WindowHelper.ShowDialog(new ReportViewWindow(), OwnerWindow);
                    break;
                case "System"://系统
                    WindowHelper.ShowDialog(new SystemSettingWindow(), OwnerWindow);
                    break;
                case "NormalTablesPreGroup":
                    ((MainWindow)OwnerWindow).GsTables.PreviousGroup();
                    break;
                case "NormalTablesNextGroup":
                    ((MainWindow)OwnerWindow).GsTables.NextGroup();
                    break;
                case "CfTablesPreGroup":
                    ((MainWindow)OwnerWindow).GsCfTables.PreviousGroup();
                    break;
                case "CfTablesNextGroup":
                    ((MainWindow)OwnerWindow).GsCfTables.NextGroup();
                    break;
            }
            SetRefreshTimerStatus(true);
        }

        private bool CanOper(object param)
        {
            switch ((string)param)
            {
                case "NormalTablesPreGroup":
                    return ((MainWindow)OwnerWindow).GsTables.CanPreviousGroup;
                case "NormalTablesNextGroup":
                    return ((MainWindow)OwnerWindow).GsTables.CanNextGruop;
                case "CfTablesPreGroup":
                    return ((MainWindow)OwnerWindow).GsCfTables.CanPreviousGroup;
                case "CfTablesNextGroup":
                    return ((MainWindow)OwnerWindow).GsCfTables.CanNextGruop;
                default:
                    return true;
            }
        }

        #endregion

        #region Proptected Methods

        protected override void InitCommand()
        {
            base.InitCommand();
            WindowLoadCmd = CreateDelegateCommand(WindowLoad);
            WindowClosingCmd = CreateDelegateCommand(WindowClosing);
            WindowClosedCmd = CreateDelegateCommand(WindowClosed);
            SelectTableCmd = CreateDelegateCommand(SelectTable);
            GetAllTableInfoCmd = CreateDelegateCommand(GetAllTableInfos);
            OperCmd = CreateDelegateCommand(OperMethod, CanOper);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 点击外卖按钮时处理外卖。
        /// </summary>
        private void TakeoutTable()
        {
            InfoLog.Instance.I("开始外卖...");
            if (!Globals.UserRight.AllowCash)
            {
                InfoLog.Instance.I("当前用户没有收银权限：{0}", Globals.UserInfo.UserName);
                MessageDialog.Warning("您没有收银权限！");
                return;
            }

            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
            {
                ErrLog.Instance.E("创建IRestaurantService服务失败。");
                MessageDialog.Warning("创建IRestaurantService服务失败。");
                return;
            }

            InfoLog.Instance.I("开始获取咖啡外卖台...");
            var result = service.GetTableInfoByType(new List<EnumTableType> { EnumTableType.CFTakeout });//查询咖啡模式外卖台
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("咖啡外卖桌台获取失败：{0}", result.Item1);
                MessageDialog.Warning(result.Item1);
                return;
            }

            if (result.Item2 != null && result.Item2.Any())
            {
                InfoLog.Instance.I("获取咖啡外卖台结束。咖啡外卖台个数：{0}", result.Item2.Count);
                var selectTableWnd = new SelectCoffeeTakeoutTableWindow(result.Item2);
                if (WindowHelper.ShowDialog(selectTableWnd, OwnerWindow))//选择了咖啡外卖就走咖啡外卖桌台，如果没有选择则走配置文件的外卖。
                {
                    InfoLog.Instance.I("选择了\"{0}\"咖啡外卖台", selectTableWnd.SelectedTable.TableName);
                    WindowHelper.ShowDialog(new TableOperWindow(selectTableWnd.SelectedTable), OwnerWindow);
                    return;
                }
            }

            InfoLog.Instance.I("没有选择咖啡外卖台，开始获取普通外卖台...");
            result = service.GetTableInfoByType(new List<EnumTableType> { EnumTableType.Takeout });//查询普通外卖台
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("普通外卖桌台获取失败：{0}", result.Item1);
                MessageDialog.Warning(result.Item1);
                return;
            }

            if (result.Item2 == null || !result.Item2.Any())
            {
                InfoLog.Instance.I("没有获取到普通外卖台，可能后台未配置。");
                MessageDialog.Warning("后台没有配置外卖台。");
                return;
            }

            var takeoutTable = result.Item2.First();
            takeoutTable.OrderId = null;//外卖这里不能有订单号，不然进入结账页面不得开台。
            InfoLog.Instance.I("获取到普通外卖台：{0}", takeoutTable.TableName);
            WindowHelper.ShowDialog(new TableOperWindow(takeoutTable), OwnerWindow);
        }

        /// <summary>
        /// 检测打印机状态。
        /// </summary>
        private void CheckPrinterStatus()
        {
            _printerCheckTimer.Stop();
            InfoLog.Instance.I("开始检测打印机状态...");
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
            {
                ErrLog.Instance.E("创建IRestaurantService服务失败。");
                OwnerWindow.Dispatcher.Invoke((Action)delegate { MessageDialog.Warning("创建IRestaurantService服务失败。"); });
                _printerCheckTimer.Start();
                return;
            }

            var result = service.GetPrinterStatusInfo();
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("检测打印机状态错误：{0}", result.Item1);
                OwnerWindow.Dispatcher.Invoke((Action)delegate { MessageDialog.Warning(result.Item1); });
                _printerCheckTimer.Start();
                return;
            }

            var errPrintCount = result.Item2.Count(t => t.PrintStatus != EnumPrintStatus.Normal);
            HasPrinterError = errPrintCount > 0;
            if (HasPrinterError)
            {
                var errMsg = string.Format("检测到{0}个打印机异常，请到\"系统\">\"打印机列表\"查看并修复。", errPrintCount);
                OwnerWindow.Dispatcher.Invoke((Action)delegate
                {
                    var wnd = new PrinterErrorInfoWindow(errMsg, NextMaxWarningInterval);
                    if (WindowHelper.ShowDialog(wnd, OwnerWindow))
                    {
                        if (wnd.IsCheckedNoWarning)
                            _printerCheckTimer.Interval = NextMaxWarningInterval * 60 * 1000;
                        else
                            _printerCheckTimer.Interval = PrinterCheckTimerInterval * 1000;
                    }
                });
            }

            _printerCheckTimer.Start();
        }

        /// <summary>
        /// 定时刷新执行方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetRefreshTimerStatus(false);
            try
            {
                if (DateTime.Now > Globals.TradeTime.BeginTime)//当前时间大于开业时间，强制结业
                {
                    Globals.TradeTime.BeginTime = Globals.TradeTime.BeginTime.AddDays(1);
                    OwnerWindow.Dispatcher.BeginInvoke((Action)delegate
                    {
                        MessageDialog.Warning("昨天还未结业，请先结业。", OwnerWindow);
                    });

                    IsForcedEndWorkModel = true;
                }
                else if (DateTime.Now > Globals.TradeTime.EndTime.AddSeconds(10))//当前时间大于结业时间，提示结业，与真实的时间提前10秒。
                {
                    Globals.TradeTime.EndTime = Globals.TradeTime.EndTime.AddDays(1);
                    OwnerWindow.Dispatcher.BeginInvoke((Action)delegate
                    {
                        MessageDialog.Warning("结业时间到了，请及时结业。", OwnerWindow);
                    });
                }

                if (RefreshRemainSecond == 0)
                {
                    SetRefreshTimerStatus(false);
                    OwnerWindow.Dispatcher.BeginInvoke((Action)GetAllTableInfoesAsync);
                    RefreshRemainSecond = RefreshTimerInterval;
                }
                else
                {
                    RefreshRemainSecond--;
                    SetRefreshTimerStatus(true);
                }

                //更新餐桌开台持续时间
                Tables.Where(t => t.TableStatus == EnumTableStatus.Dinner).ToList().ForEach(t => t.UpdateDinnerDuration());
                CfTables.Where(t => t.TableStatus == EnumTableStatus.Dinner).ToList().ForEach(t => t.UpdateDinnerDuration());
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
            }
        }

        /// <summary>
        /// 打印机检测定时器触发时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void PrinterCheckTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            CheckPrinterStatus();
        }

        /// <summary>
        /// 异步获取所有餐台信息。
        /// </summary>
        private void GetAllTableInfoesAsync()
        {
            var info = Tables.Any() ? "" : "加载所有餐桌信息...";
            TaskService.Start(null, GetAllTableInfoProcess, GetAllTableInfoComplete, info);
        }

        /// <summary>
        /// 获取所有餐台信息执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object GetAllTableInfoProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, List<TableInfo>>("创建IRestaurantService服务失败。", null);

            var request = new List<EnumTableType>
            {
                EnumTableType.Room,
                EnumTableType.Outside,
                EnumTableType.CFTable
            };
            return service.GetTableInfoByType(request);
        }

        /// <summary>
        /// 获取所有餐台信息执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void GetAllTableInfoComplete(object param)
        {
            var result = (Tuple<string, List<TableInfo>>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1, OwnerWindow);
                SetRefreshTimerStatus(true);
                return;
            }

            SetRefreshTimerStatus(true);
            if (result.Item2 != null && result.Item2.Any())
            {
                IdleCount = result.Item2.Count(t => !IsForcedEndWorkModel && t.TableStatus == EnumTableStatus.Idle);
                DinnerCount = result.Item2.Count(t => t.TableStatus == EnumTableStatus.Dinner);
                DisableCount = result.Item2.Count(t => IsForcedEndWorkModel && t.TableStatus == EnumTableStatus.Idle);

                var normalTables = result.Item2.Where(t => t.TableType == EnumTableType.Room || t.TableType == EnumTableType.Outside).ToList();
                UpdateNormalTables(normalTables);

                var cfTables = result.Item2.Where(t => t.TableType == EnumTableType.CFTable).ToList();
                UpdateCfTables(cfTables);
            }
        }

        /// <summary>
        /// 更新标准餐台。
        /// </summary>
        /// <param name="tableInfos">餐台集合。</param>
        private void UpdateNormalTables(List<TableInfo> tableInfos)
        {
            foreach (var tableInfo in tableInfos)
            {
                var item = Tables.FirstOrDefault(t => t.TableId == tableInfo.TableId);
                if (item == null)
                    AddTableInfo(tableInfo);
                else
                    item.CloneData(tableInfo);
            }

            var removedTables = Tables.Where(t => tableInfos.All(y => y.TableId != t.TableId)).ToList();
            if (removedTables.Any())
                removedTables.ForEach(t => Tables.Remove(t));

            foreach (var table in Tables)
            {
                table.UpdateDinnerDuration();
            }
        }

        /// <summary>
        /// 更新咖啡餐台信息。
        /// </summary>
        /// <param name="tableInfos"></param>
        private void UpdateCfTables(List<TableInfo> tableInfos)
        {
            foreach (var tableInfo in tableInfos)
            {
                var item = CfTables.FirstOrDefault(t => t.TableId == tableInfo.TableId);
                if (item == null)
                    AddTableInfo(tableInfo);
                else
                    item.CloneData(tableInfo);
            }

            var removedTables = CfTables.Where(t => tableInfos.All(y => y.TableId != t.TableId)).ToList();
            if (removedTables.Any())
                removedTables.ForEach(t => Tables.Remove(t));

            foreach (var cfTable in CfTables)
            {
                cfTable.UpdateDinnerDuration();
            }
        }

        /// <summary>
        /// 添加餐台。
        /// </summary>
        /// <param name="item"></param>
        private void AddTableInfo(TableInfo item)
        {
            if (item.TableType == EnumTableType.CFTable)
            {
                item.TableEnable = item.TableStatus == EnumTableStatus.Dinner;//咖啡台不允许开台，所以只有当就餐时才允许点击。
                CfTables.Add(item);
            }
            else
            {
                //餐台的可用状态：只有当是强制结业模式且餐台空闲时不可用，其他时候都可用。
                item.TableEnable = !IsForcedEndWorkModel || (item.TableStatus == EnumTableStatus.Dinner);
                Tables.Add(item);
            }
        }

        /// <summary>
        /// 清机。
        /// </summary>
        private void ClearnMachine()
        {
            InfoLog.Instance.I("点击清机按钮，选择清机或结业...");
            var hasDinnerTable = Tables.Any(t => t.TableStatus == EnumTableStatus.Dinner);
            var wnd = new SelectClearnStepWindow(!hasDinnerTable, IsForcedEndWorkModel);
            if (!WindowHelper.ShowDialog(wnd, OwnerWindow))
                return;

            if (wnd.IsEndWork)
            {
                InfoLog.Instance.I("选择的是结业，开始判断所有机器是否都清机。 ");
                if (!CommonHelper.ClearAllPos(false))
                    return;

                InfoLog.Instance.I("所有机器都清机完成，开始结业...");
                EndWork();
            }
            else
            {
                if (CommonHelper.ClearPos(Globals.UserInfo.UserName))
                    CommonHelper.ForcedLogin();
            }
        }

        /// <summary>
        /// 结业。
        /// </summary>
        private void EndWork()
        {
            InfoLog.Instance.I("结业授权...");
            var wnd = new AuthorizationWindow(EnumRightType.EndWork);
            if (!WindowHelper.ShowDialog(wnd, OwnerWindow))
            {
                InfoLog.Instance.I("结业授权失败，终止结业。");
                return;
            }

            InfoLog.Instance.I("结业授权成功，开始调用结业接口...");
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
                SetRefreshTimerStatus(true);
                ErrLog.Instance.E("结业接口调用失败：{0}", result);
                MessageDialog.Warning(result, OwnerWindow);
                return;
            }

            InfoLog.Instance.I("结业成功，开始调用结业上传数据接口...");
            EndWorkSyncData();
        }

        /// <summary>
        /// 结业后JDE同步数据方法的异步执行。
        /// </summary>
        private void EndWorkSyncData()
        {
            TaskService.Start(null, EndWorkSyncDataProcess, EndWorkSyncDataComplete, "通知结业同步数据...");
        }

        /// <summary>
        /// 结业后JDE同步数据的执行方法。
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
        /// 结业后JDE同步数据的执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void EndWorkSyncDataComplete(object param)
        {
            SetRefreshTimerStatus(true);
            var result = (string)param;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E("结业后上传数据接口调用失败：{0}", result);
                if (
                    MessageDialog.Quest(result + Environment.NewLine + "上传数据失败，是否重新上传？" + Environment.NewLine +
                                        "\"确定\"重新上传，\"取消\"放弃上传。"))
                {
                    InfoLog.Instance.I("选择了重新上传结业数据...");
                    EndWorkSyncData();
                }

                InfoLog.Instance.I("选择取消重新上传结业数据。");
                return;
            }

            InfoLog.Instance.I("结业数据上传成功。");
            MessageDialog.Warning("结业成功。", OwnerWindow);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 设置刷新定时器的状态。
        /// </summary>
        /// <param name="enable">启动定时器时为true，停止则为false。</param>
        private void SetRefreshTimerStatus(bool enable)
        {
            _refreshTimer.Enabled = enable;
        }

        #endregion
    }
}