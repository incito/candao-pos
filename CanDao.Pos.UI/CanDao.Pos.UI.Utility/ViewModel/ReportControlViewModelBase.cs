using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using DevExpress.Xpf.Grid;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 报表统计控件的ViewModel基类。
    /// </summary>
    public class ReportControlViewModelBase : BaseViewModel
    {
        #region Fields

        /// <summary>
        /// 排序规则。
        /// </summary>
        private GridSortInfo _gridSortInfo;

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

        #endregion

        #region Command Methods

        /// <summary>
        /// 控件加载命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void ControlLoad(object arg)
        {
            StatisticsPeriodsType = EnumStatisticsPeriodsType.Today;
        }

        /// <summary>
        /// 排序命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void EndSorting(object arg)
        {
            _gridSortInfo = ((GridControl)arg).SortInfo.First();
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
            UpdateDataIndex();
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

        #endregion

        #region Protected Methods

        private void InitCommand()
        {
            ControlLoadCmd = CreateDelegateCommand(ControlLoad);
            EndSortingCmd = CreateDelegateCommand(EndSorting);
            PrintCmd = CreateDelegateCommand(Print);
            StatisticCheckedCmd = CreateDelegateCommand(StatisticChecked);
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
            var result = arg as Tuple<string, ReportStatisticInfo>;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取报表统计信息失败：{0}", result.Item1);
                MessageDialog.Warning(result.Item1);
                return null;
            }

            Data = result.Item2;
            UpdateDataIndex();

            DataSource.Clear();
            if (result.Item2 != null)
                result.Item2.DataSource.ForEach(DataSource.Add);

            return null;
        }

        /// <summary>
        /// 更新统计数据的序号。
        /// </summary>
        private void UpdateDataIndex()
        {
            if (_gridSortInfo != null)
            {
                if (_gridSortInfo.SortOrder == ListSortDirection.Descending)
                {
                    switch (_gridSortInfo.FieldName)
                    {
                        case "Name":
                            Data.DataSource = Data.DataSource.OrderByDescending(t => t.Name).ToList();
                            break;
                        case "Count":
                            Data.DataSource = Data.DataSource.OrderByDescending(t => t.Count).ToList();
                            break;
                        case "Amount":
                            Data.DataSource = Data.DataSource.OrderByDescending(t => t.Amount).ToList();
                            break;
                    }
                }
                else
                {
                    switch (_gridSortInfo.FieldName)
                    {
                        case "Name":
                            Data.DataSource = Data.DataSource.OrderBy(t => t.Name).ToList();
                            break;
                        case "Count":
                            Data.DataSource = Data.DataSource.OrderBy(t => t.Count).ToList();
                            break;
                        case "Amount":
                            Data.DataSource = Data.DataSource.OrderBy(t => t.Amount).ToList();
                            break;
                    }
                }
            }

            var idx = 1;
            Data.DataSource.ForEach(t => t.Index = idx++);
        }

        /// <summary>
        /// 打印报表。
        /// </summary>
        protected virtual void PrintReport() { } 

        #endregion
    }
}