using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.UI.Utility.View;
using Timer = System.Timers.Timer;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class SysPrinterListVm : BaseViewModel
    {
        #region Fields

        /// <summary>
        /// 刷新间隔时间。
        /// </summary>
        private const int RefreshIntervalsSecond = 10;

        /// <summary>
        /// 刷新定时器。
        /// </summary>
        private Timer _refreshTimer;

        #endregion

        #region Constructor

        public SysPrinterListVm()
        {
            if (!IsInDesignMode)
            {
                InitRefreshTimer();
                PrinterStatusInfos = new ObservableCollection<PrintStatusInfo>();
                RefreshCmd = CreateDelegateCommand(Refresh);
                GroupCmd = CreateDelegateCommand(Group, CanGroup);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 打印状态信息集合。
        /// </summary>
        public ObservableCollection<PrintStatusInfo> PrinterStatusInfos { get; private set; }

        /// <summary>
        /// 是否有错误的打印机。
        /// </summary>
        private bool _hasErrorPrinter;
        /// <summary>
        /// 是否有错误的打印机。
        /// </summary>
        public bool HasErrorPrinter
        {
            get { return _hasErrorPrinter; }
            private set
            {
                _hasErrorPrinter = value;
                RaisePropertyChanged(() => HasErrorPrinter);
            }
        }

        /// <summary>
        /// 剩余刷新时间。
        /// </summary>
        private int _remainingTimes;

        /// <summary>
        /// 剩余刷新时间。
        /// </summary>
        public int RemainingTimes
        {
            get { return _remainingTimes; }
            set
            {
                _remainingTimes = value;
                RaisePropertyChanged(() => RemainingTimes);
            }
        }

        /// <summary>
        /// 所属控件。
        /// </summary>
        public SysPrinterList OwnerCtrl { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// 刷新命令。
        /// </summary>
        public ICommand RefreshCmd { get; private set; }

        /// <summary>
        /// 分组命令。
        /// </summary>
        public ICommand GroupCmd { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 刷新命令的执行方法。
        /// </summary>
        private void Refresh(object arg)
        {
            RemainingTimes = RefreshIntervalsSecond - 1;
            _refreshTimer.Stop();
            OwnerCtrl.Dispatcher.BeginInvoke((Action)delegate
            {
                TaskService.Start(null, GetPrinterStatusProcess, GetPrinterStatusComplete, "获取打印机状态中...");
            });
        }

        /// <summary>
        /// 分组命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void Group(object arg)
        {
            switch ((string)arg)
            {
                case "PreGroup":
                    OwnerCtrl.GsPrinterList.PreviousGroup();
                    break;
                case "NextGroup":
                    OwnerCtrl.GsPrinterList.NextGroup();
                    break;
            }
        }

        /// <summary>
        /// 分组命令是否可用。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanGroup(object arg)
        {
            switch ((string)arg)
            {
                case "PreGroup":
                    return OwnerCtrl.GsPrinterList.CanPreviousGroup;
                case "NextGroup":
                    return OwnerCtrl.GsPrinterList.CanNextGruop;
                default:
                    return true;
            }
        }


        #endregion

        #region Private Method

        /// <summary>
        /// 初始化刷新定时器。
        /// </summary>
        private void InitRefreshTimer()
        {
            _refreshTimer = new Timer { Interval = 1000 };
            _refreshTimer.Elapsed += RefreshTimerElspsed;
            RemainingTimes = RefreshIntervalsSecond;
            _refreshTimer.Start();
        }

        private void RefreshTimerElspsed(object sender, ElapsedEventArgs e)
        {
            _refreshTimer.Stop();
            if (--RemainingTimes < 0)
                Refresh(null);
            else
                _refreshTimer.Start();
        }

        /// <summary>
        /// 获取打印状态列表执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object GetPrinterStatusProcess(object param)
        {
            InfoLog.Instance.I("开始获取打印机状态列表...");
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, List<PrintStatusInfo>>("创建IRestaurantService服务失败。", null);

            return service.GetPrinterStatusInfo();
        }

        /// <summary>
        /// 获取打印机状态列表执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void GetPrinterStatusComplete(object param)
        {
            var result = (Tuple<string, List<PrintStatusInfo>>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取打印机列表时错误：{0}", result.Item1);
                MessageDialog.Warning(result.Item1);
                _refreshTimer.Start();
                return;
            }

            PrinterStatusInfos.Clear();
            if (result.Item2 != null)
                result.Item2.ForEach(PrinterStatusInfos.Add);

            HasErrorPrinter = PrinterStatusInfos.Any(t => t.PrintStatus != EnumPrintStatus.Normal);
            if (!HasErrorPrinter)
                InfoLog.Instance.I("打印机全部状态正常。");
            else
                InfoLog.Instance.E("有打印机状态异常。");
            _refreshTimer.Start();
        }

        #endregion
    }
}