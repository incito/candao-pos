using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Library.View;
using Common;
using Models;
using Models.Enum;
using WebServiceReference.ServiceImpl;
using AllLog = Common.AllLog;
using Keyboard = CanDao.Pos.Common.Keyboard;
using Timer = System.Timers.Timer;

namespace CanDao.Pos.UI.Library.ViewModel
{
    /// <summary>
    /// 打印机状态列表显示控件。
    /// </summary>
    public class UcPrinterListViewModel : BaseViewModel
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

        public UcPrinterListViewModel()
        {
            if (!IsInDesignMode)
            {
                InitRefreshTimer();
                PrinterStatusInfos = new ObservableCollection<PrintStatusInfo>();
                RefreshCmd = CreateDelegateCommand(Refresh);
                PageUpCmd = CreateDelegateCommand(PageUp);
                PageDownCmd = CreateDelegateCommand(PageDown);
                _refreshTimer.Start();
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
        public UcPrinterListView OwnerCtrl { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// 刷新命令。
        /// </summary>
        public ICommand RefreshCmd { get; private set; }

        /// <summary>
        /// 上翻按钮。
        /// </summary>
        public ICommand PageUpCmd { get; private set; }

        /// <summary>
        /// 下翻按钮。
        /// </summary>
        public ICommand PageDownCmd { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 刷新命令的执行方法。
        /// </summary>
        private void Refresh(object arg)
        {
            RemainingTimes = RefreshIntervalsSecond - 1;
            _refreshTimer.Stop();
            ThreadPool.QueueUserWorkItem(y =>
            {
                var service = new RestaurantServiceImpl();
                var result = service.GetPrinterStatusInfo();
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    OwnerCtrl.Dispatcher.BeginInvoke((Action)delegate { MessageDialog.Warning(result.Item1); });
                    return;
                }

                AllLog.Instance.I("获取打印机状态列表成功。");
                OwnerCtrl.Dispatcher.BeginInvoke((Action)delegate
                {
                    PrinterStatusInfos.Clear();
                    if (result.Item2 != null)
                        result.Item2.ForEach(PrinterStatusInfos.Add);

                    HasErrorPrinter = PrinterStatusInfos.Any(t => t.PrintStatus != EnumPrintStatus.Normal);
                    if (!HasErrorPrinter)
                        AllLog.Instance.I("打印机全部状态正常。");
                    else
                        AllLog.Instance.E("有打印机状态异常。");
                });
                _refreshTimer.Start();
            });
        }

        private void PageUp(object arg)
        {
            OwnerCtrl.GcPrinterList.Focus();
            Keyboard.Press(Key.PageUp);
        }

        private void PageDown(object arg)
        {
            OwnerCtrl.GcPrinterList.Focus();
            Keyboard.Press(Key.PageDown);
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
        }

        private void RefreshTimerElspsed(object sender, ElapsedEventArgs e)
        {
            _refreshTimer.Stop();
            if (--RemainingTimes < 0)
                Refresh(null);
            else
                _refreshTimer.Start();
        }

        #endregion
    }
}