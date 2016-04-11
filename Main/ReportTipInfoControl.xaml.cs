using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Common;
using DevExpress.Xpf.Core;
using Library;
using Models;
using Models.Enum;
using ReportsFastReport;
using WebServiceReference.IService;
using WebServiceReference.ServiceImpl;
using Keyboard = Library.Keyboard;

namespace KYPOS
{
    /// <summary>
    /// ReportTipInfoControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReportTipInfoControl
    {
        #region Fields

        /// <summary>
        /// 当前选中的周期。
        /// </summary>
        private ToggleButton _curSelectTbBtn;

        /// <summary>
        /// 小费统计信息全。
        /// </summary>
        private TipFullInfo _tipFullInfo;

        #endregion

        #region Constructor

        public ReportTipInfoControl()
        {
            InitializeComponent();
            TipInfos = new ObservableCollection<TipInfo>();

            DataContext = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 小费统计信息集合。
        /// </summary>
        public ObservableCollection<TipInfo> TipInfos { get; private set; }

        #endregion

        #region Event Methods

        private void ButtonPrint_OnClick(object sender, RoutedEventArgs e)
        {
            if (_tipFullInfo == null || _tipFullInfo.TipInfos == null || !_tipFullInfo.TipInfos.Any())
            {
                frmBase.Warning("没有需要打印的小费明细数据。");
                return;
            }

            _tipFullInfo.BranchId = Globals.branch_id;
            _tipFullInfo.CurrentTime = DateTime.Now;
            _tipFullInfo.TotalAmount = _tipFullInfo.TipInfos != null ? _tipFullInfo.TipInfos.Sum(t => t.TipAmount) : 0;
            ReportPrint.PrintTipDetail(_tipFullInfo);
        }

        private void DcTipView_OnEndSorting(object sender, RoutedEventArgs e)
        {
            var sortInfo = DcTipView.SortInfo.First();
            if (sortInfo.SortOrder == ListSortDirection.Descending)
            {
                if (sortInfo.FieldName == "WaiterName")
                    _tipFullInfo.TipInfos = _tipFullInfo.TipInfos.OrderByDescending(t => t.WaiterName).ToList();
                else if (sortInfo.FieldName == "TipAmount")
                    _tipFullInfo.TipInfos = _tipFullInfo.TipInfos.OrderByDescending(t => t.TipAmount).ToList();
                else if (sortInfo.FieldName == "TipCount")
                    _tipFullInfo.TipInfos = _tipFullInfo.TipInfos.OrderByDescending(t => t.TipCount).ToList();
            }
            else
            {
                if (sortInfo.FieldName == "WaiterName")
                    _tipFullInfo.TipInfos = _tipFullInfo.TipInfos.OrderBy(t => t.WaiterName).ToList();
                else if (sortInfo.FieldName == "TipAmount")
                    _tipFullInfo.TipInfos = _tipFullInfo.TipInfos.OrderBy(t => t.TipAmount).ToList();
                else if (sortInfo.FieldName == "TipCount")
                    _tipFullInfo.TipInfos = _tipFullInfo.TipInfos.OrderBy(t => t.TipCount).ToList();
            }

            UpdateTipInfoIndex();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            _curSelectTbBtn = (ToggleButton)sender;
            var enumType = (EnumStatisticsPeriodsType)Enum.Parse(typeof(EnumStatisticsPeriodsType), (string)((ToggleButton)sender).Tag);
            TaskService.Start(enumType, GetTipInfoProcess, GetTipInfoComplete, "获取小费统计数据中...");
        }

        private void TbToday_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var tgBtn = (ToggleButton)sender;
            if (_curSelectTbBtn != null && _curSelectTbBtn.Equals(tgBtn))
            {
                e.Handled = true;
                return;
            }

            if (_curSelectTbBtn != null)
                _curSelectTbBtn.IsChecked = false;
        }

        private void BtnLast_OnClick(object sender, RoutedEventArgs e)
        {
            DcTipView.Focus();
            Keyboard.Press(Key.PageUp);
            Keyboard.Release(Key.PageUp);
        }

        private void BtnNext_OnClick(object sender, RoutedEventArgs e)
        {
            DcTipView.Focus();
            Keyboard.Press(Key.PageDown);
            Keyboard.Release(Key.PageDown);
        }

        private void ReportTipInfoControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_curSelectTbBtn == null)
                TbToday.IsChecked = true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 获取小费统计信息执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object GetTipInfoProcess(object param)
        {
            IRestaurantService service = new RestaurantServiceImpl();
            return service.GetTipInfos((EnumStatisticsPeriodsType)param);
        }

        /// <summary>
        /// 获取小费统计信息执行完成。
        /// </summary>
        /// <param name="arg"></param>
        private void GetTipInfoComplete(object arg)
        {
            try
            {
                var result = (Tuple<string, TipFullInfo>)arg;
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    frmBase.Warning(result.Item1);
                    return;
                }

                _tipFullInfo = result.Item2;
                UpdateTipInfoIndex();
                TipInfos.Clear();
                if (result.Item2 != null)
                    result.Item2.TipInfos.ForEach(TipInfos.Add);
            }
            catch (Exception ex)
            {
                AllLog.Instance.E(ex);
            }
            finally
            {
                if (TipInfos != null)
                {
                    TbTotalAmount.Text = string.Format("金额合计：{0}", TipInfos.Sum(t => t.TipAmount));
                }
            }
        }

        /// <summary>
        /// 更新小费统计序号。
        /// </summary>
        private void UpdateTipInfoIndex()
        {
            var index = 1;
            _tipFullInfo.TipInfos.ForEach(t => t.Index = index++);
        }

        #endregion

    }
}
