using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.UI.MainView.View;
using CanDao.Pos.UI.Utility;
using CanDao.Pos.UI.Utility.View;
using DevExpress.Xpf.Editors.Helpers;
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
            RefreshRemainSecond = RefreshTimerInterval;

            _refreshTimer = new Timer(1000) { Enabled = true };
            _refreshTimer.Elapsed += RefreshTimer_Elapsed;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 所有餐台集合。
        /// </summary>
        public ObservableCollection<TableInfo> Tables { get; private set; }

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
        /// 是否是强制结业模式。
        /// </summary>
        public bool IsForcedEndWorkModel { get; set; }

        #endregion

        #region Command

        /// <summary>
        /// 加载命令。
        /// </summary>
        public ICommand LoadCmd { get; private set; }

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
        /// 加载命令的执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void Load(object param)
        {
            if (!IsInDesignMode)
            {
                GetAllTableInfoesAsync();

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
                MessageDialog.Warning("没有收银权限。", OwnerWindow);
                return;
            }

            WindowHelper.ShowDialog(new TableOperWindow(tableInfo) { Owner = OwnerWindow });

            GetAllTableInfoesAsync();
            RefreshRemainSecond = RefreshTimerInterval;
            SetRefreshTimerStatus(true);
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
                    WindowHelper.ShowDialog(new TableOperWindow(SystemConfigCache.TakeoutTableName), OwnerWindow);
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
            }
            SetRefreshTimerStatus(true);
        }

        #endregion

        #region Proptected Methods

        protected override void InitCommand()
        {
            base.InitCommand();
            LoadCmd = CreateDelegateCommand(Load);
            SelectTableCmd = CreateDelegateCommand(SelectTable);
            GetAllTableInfoCmd = CreateDelegateCommand(GetAllTableInfos);
            OperCmd = CreateDelegateCommand(OperMethod);
        }

        #endregion

        #region Private Methods

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
                }

                //更新餐桌开台持续时间
                Tables.ForEach(t => t.UpdateDinnerDuration());
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
            }
            finally
            {
                SetRefreshTimerStatus(true);
            }
        }

        /// <summary>
        /// 异步获取所有餐台信息。
        /// </summary>
        private void GetAllTableInfoesAsync()
        {
            TaskService.Start(null, GetAllTableInfoProcess, GetAllTableInfoComplete, "加载所有餐桌信息...");
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

            return service.GetAllTableInfoes();
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
                return;
            }

            SetRefreshTimerStatus(true);
            if (result.Item2 != null && result.Item2.Any())
            {
                UpdateTables(result.Item2);
            }
            IdleCount = Tables.Count(t => !IsForcedEndWorkModel && t.TableStatus == EnumTableStatus.Idle);
            DinnerCount = Tables.Count(t => t.TableStatus == EnumTableStatus.Dinner);
            DisableCount = Tables.Count(t => IsForcedEndWorkModel && t.TableStatus == EnumTableStatus.Idle);
        }

        private void UpdateTables(List<TableInfo> tableInfos)
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

            Tables.ForEach(t => t.UpdateDinnerDuration());
        }

        /// <summary>
        /// 添加餐台。
        /// </summary>
        /// <param name="item"></param>
        private void AddTableInfo(TableInfo item)
        {
            //餐台的可用状态：只有当是强制结业模式且餐台空闲时不可用，其他时候都可用。
            item.TableEnable = !IsForcedEndWorkModel || (item.TableStatus == EnumTableStatus.Dinner);
            Tables.Add(item);
        }

        /// <summary>
        /// 清机。
        /// </summary>
        private void ClearnMachine()
        {
            var hasDinnerTable = Tables.Any(t => t.TableStatus == EnumTableStatus.Dinner);
            var wnd = new SelectClearnStepWindow(!hasDinnerTable, IsForcedEndWorkModel);
            if (!WindowHelper.ShowDialog(wnd, OwnerWindow))
                return;

            if (wnd.IsEndWork)
            {
                if (!CommonHelper.ClearAllPos(false))
                    return;

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
            AuthorizationWindow wnd = new AuthorizationWindow(EnumRightType.EndWork);
            if (!WindowHelper.ShowDialog(wnd, OwnerWindow))
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
                SetRefreshTimerStatus(true);
                MessageDialog.Warning(result, OwnerWindow);
                return;
            }

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
                if (MessageDialog.Quest(result + Environment.NewLine + "上传数据失败，是否重新上传？" + Environment.NewLine + "\"确定\"重新上传，\"取消\"放弃上传。"))
                    EndWorkSyncData();
                return;
            }

            MessageDialog.Warning("结业成功。", OwnerWindow);
            CommonHelper.ForcedLogin();
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