using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.Common.Controls;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 报表统计控件的ViewModel基类。
    /// </summary>
    public class ReportControlViewModelBase : BaseViewModel
    {
        #region Fields

        #endregion

        #region Constructor

        public ReportControlViewModelBase()
        {
            InitCommand();
            DataSource = new ObservableCollection<ReportDataBase>();
            Data = new ReportStatisticInfo();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 统计数据。
        /// </summary>
        public ReportStatisticInfo Data { get; set; }

        public ObservableCollection<ReportDataBase> DataSource { get; private set; }

        /// <summary>
        /// 项总数。
        /// </summary>
        private decimal _totalItemsCount;
        /// <summary>
        /// 项总数。
        /// </summary>
        public decimal TotalItemsCount
        {
            get { return _totalItemsCount; }
            set
            {
                _totalItemsCount = value;
                RaisePropertyChanged("TotalItemsCount");
            }
        }

        /// <summary>
        /// 数量总数。
        /// </summary>
        private decimal _totalCount;
        /// <summary>
        /// 数量总数。
        /// </summary>
        public decimal TotalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;
                RaisePropertyChanged("TotalCount");
            }
        }

        /// <summary>
        /// 金额总数。
        /// </summary>
        private decimal _totalAmount;
        /// <summary>
        /// 金额总数。
        /// </summary>
        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                _totalAmount = value;
                RaisePropertyChanged("TotalAmount");
            }
        }

        /// <summary>
        /// 统计周期。
        /// </summary>
        private EnumStatisticsPeriodsType _statisticsPeriodsType;

        /// <summary>
        /// 统计周期。
        /// </summary>
        public EnumStatisticsPeriodsType StatisticsPeriodsType
        {
            get { return _statisticsPeriodsType; }
            set
            {
                if (_statisticsPeriodsType == value)
                    return;

                _statisticsPeriodsType = value;
                RaisePropertyChanged("StatisticsPeriodsType");

                GetReportStatisticInfoAsync();
            }
        }

        /// <summary>
        /// 分组控件。
        /// </summary>
        public GroupSelector GsCtrl { get; set; }

        #endregion

        #region Command

        /// <summary>
        /// 控件加载命令。
        /// </summary>
        public ICommand ControlLoadCmd { get; private set; }

        /// <summary>
        /// 排序命令。
        /// </summary>
        public ICommand EndSortingCmd { get; private set; }

        /// <summary>
        /// 打印命令。
        /// </summary>
        public ICommand PrintCmd { get; private set; }

        /// <summary>
        /// 统计周期选择命令。
        /// </summary>
        public ICommand StatisticCheckedCmd { get; private set; }

        /// <summary>
        /// 上一组命令。
        /// </summary>
        public ICommand PreGroupCmd { get; private set; }

        /// <summary>
        /// 下一组命令。
        /// </summary>
        public ICommand NextGroupCmd { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 控件加载命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void ControlLoad(object arg)
        {
            if (StatisticsPeriodsType == EnumStatisticsPeriodsType.None)
                StatisticsPeriodsType = EnumStatisticsPeriodsType.Today;
        }

        /// <summary>
        /// 排序命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void EndSorting(object arg)
        {
            //_gridSortInfo = ((DataGrid)arg).Columns.
        }

        /// <summary>
        /// 打印命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void Print(object arg)
        {
            if (DataSource == null || !DataSource.Any())
            {
                var msg = "没有需要打印的报表数据。";
                InfoLog.Instance.I(msg);
                MessageDialog.Warning(msg);
                return;
            }

            Data.BranchId = Globals.BranchInfo.BranchId;
            Data.CurrentTime = DateTime.Now;
            Data.TotalAmount = Data.DataSource.Sum(t => t.Amount);
            PrintReport();
        }

        /// <summary>
        /// 统计周期选择命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void StatisticChecked(object arg)
        {
            StatisticsPeriodsType = (EnumStatisticsPeriodsType)Enum.Parse(typeof(EnumStatisticsPeriodsType), (string)arg);
        }

        protected virtual void PreGroup(object arg)
        {
            if (GsCtrl != null)
                GsCtrl.PreviousGroup();
        }

        protected virtual bool CanPreGroup(object arg)
        {
            return GsCtrl == null || GsCtrl.CanPreviousGroup;
        }

        protected virtual void NextGroup(object arg)
        {
            if (GsCtrl != null)
                GsCtrl.NextGroup();
        }

        protected virtual bool CanNextGroup(object arg)
        {
            return GsCtrl == null || GsCtrl.CanNextGruop;
        }

        #endregion

        #region Protected Methods

        private void InitCommand()
        {
            ControlLoadCmd = CreateDelegateCommand(ControlLoad);
            EndSortingCmd = CreateDelegateCommand(EndSorting);
            PrintCmd = CreateDelegateCommand(Print);
            StatisticCheckedCmd = CreateDelegateCommand(StatisticChecked);
            PreGroupCmd = CreateDelegateCommand(PreGroup, CanPreGroup);
            NextGroupCmd = CreateDelegateCommand(NextGroup, CanNextGroup);
        }

        /// <summary>
        /// 异步获取报表统计信息。
        /// </summary>
        private void GetReportStatisticInfoAsync()
        {
            WorkFlowService.Start(StatisticsPeriodsType, new WorkFlowInfo(GetReportStatisticInfoProcess, GetReportStatisticInfoComplete, "处理中..."));
        }

        /// <summary>
        /// 获取报表统计信息执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected virtual object GetReportStatisticInfoProcess(object arg)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取报表统计信息执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> GetReportStatisticInfoComplete(object arg)
        {
            var result = (Tuple<string, ReportStatisticInfo>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取报表统计信息失败：{0}", result.Item1);
                MessageDialog.Warning(result.Item1);
                return null;
            }

            Data = result.Item2;

            DataSource.Clear();
            if (result.Item2 != null)
                result.Item2.DataSource.ForEach(DataSource.Add);

            TotalItemsCount = DataSource.Count;
            TotalCount = DataSource.Sum(t => t.Count);
            TotalAmount = DataSource.Sum(t => t.Amount);

            return null;
        }

        /// <summary>
        /// 打印报表。
        /// </summary>
        protected virtual void PrintReport() { }

        #endregion
    }
}