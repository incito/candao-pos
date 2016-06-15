using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;
using CanDao.Pos.SystemConfig.Views;
using CanDaoCD.Pos.Common.Classes.Mvvms;
using Common;
using Models;
using Models.Enum;
using WebServiceReference.ServiceImpl;
using Keyboard = CanDaoCD.Pos.Common.Operates.Keyboard;

namespace CanDao.Pos.SystemConfig.ViewModels
{
    /// <summary>
    /// 打印机状态列表显示控件。
    /// </summary>
    public class UcPrinterListViewModel : ViewModelBase
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
                RefreshCmd = new RelayCommand(Refresh);
                PageUpCmd = new RelayCommand(PageUp);
                PageDownCmd = new RelayCommand(PageDown);
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
        /// 错误的打印机个数。
        /// </summary>
        private int _errorPrintCount;

        /// <summary>
        /// 错误的打印机个数。
        /// </summary>
        public int ErrorPrintCount
        {
            get { return _errorPrintCount; }
            set
            {
                _errorPrintCount = value;
                RaisePropertyChanged(() => ErrorPrintCount);
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
        public RelayCommand RefreshCmd { get; private set; }

        /// <summary>
        /// 上翻按钮。
        /// </summary>
        public RelayCommand PageUpCmd { get; private set; }

        /// <summary>
        /// 下翻按钮。
        /// </summary>
        public RelayCommand PageDownCmd { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 刷新命令的执行方法。
        /// </summary>
        private void Refresh()
        {
            _refreshTimer.Stop();
            var service = new RestaurantServiceImpl();
            var result = service.GetPrinterStatusInfo();
            if (!string.IsNullOrEmpty(result.Item1))
            {
                Msg.ShowError(result.Item1);
                return;
            }

            AllLog.Instance.I("获取打印机状态列表成功。");
            OwnerCtrl.Dispatcher.BeginInvoke((Action)delegate
            {
                PrinterStatusInfos.Clear();
                if (result.Item2 != null)
                    result.Item2.ForEach(PrinterStatusInfos.Add);
                ErrorPrintCount = PrinterStatusInfos.Count(t => t.PrintStatus != EnumPrintStatus.Normal);
                if (ErrorPrintCount == 0)
                    AllLog.Instance.I("打印机全部状态正常。");
                else
                    AllLog.Instance.E("打印机错误状态的个数：{0}", ErrorPrintCount);
            });
            _refreshTimer.Start();
        }

        private void PageUp()
        {
            OwnerCtrl.GcPrinterList.Focus();
            Keyboard.Press(Key.PageUp);
        }

        private void PageDown()
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
            {
                RemainingTimes = RefreshIntervalsSecond - 1;
                Refresh();
            }
            _refreshTimer.Start();
        }

        #endregion
    }
}