using System;
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
    /// 品项明细报表控件。
    /// </summary>
    public partial class ReportDishInfoControl
    {
        #region Fields

        /// <summary>
        /// 当前选中的周期。
        /// </summary>
        private ToggleButton _curSelectTbBtn;

        /// <summary>
        /// 品项全信息。
        /// </summary>
        private DishSaleFullInfo _dishSaleFullInfo;

        /// <summary>
        /// 当前视图序号。
        /// </summary>
        private int _curViewIndex;

        #endregion

        #region Constructor

        public ReportDishInfoControl()
        {
            InitializeComponent();
            DishSaleInfos = new ObservableCollection<DishSaleInfo>();

            DataContext = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 品项销售信息集合。
        /// </summary>
        public ObservableCollection<DishSaleInfo> DishSaleInfos { get; private set; }

        #endregion

        #region Event Methods

        private void ButtonPrint_OnClick(object sender, RoutedEventArgs e)
        {
            if (_dishSaleFullInfo == null || _dishSaleFullInfo.DishSaleInfos == null || !_dishSaleFullInfo.DishSaleInfos.Any())
            {
                frmBase.Warning("没有需要打印的品项明细数据。");
                return;
            }

            _dishSaleFullInfo.BranchId = Globals.branch_id;
            _dishSaleFullInfo.CurrentTime = DateTime.Now;
            _dishSaleFullInfo.TotalAmount = _dishSaleFullInfo.DishSaleInfos != null ? _dishSaleFullInfo.DishSaleInfos.Sum(t => t.SalesAmount) : 0;
            ReportPrint.PrintDishSaleDetail(_dishSaleFullInfo);
        }

        private void DcDishView_OnEndSorting(object sender, RoutedEventArgs e)
        {
            var sortInfo = DcDishView.SortInfo.First();
            if (sortInfo.SortOrder == ListSortDirection.Descending)
            {
                if (sortInfo.FieldName == "Name")
                    _dishSaleFullInfo.DishSaleInfos = _dishSaleFullInfo.DishSaleInfos.OrderByDescending(t => t.Name).ToList();
                else if (sortInfo.FieldName == "SalesAmount")
                    _dishSaleFullInfo.DishSaleInfos = _dishSaleFullInfo.DishSaleInfos.OrderByDescending(t => t.SalesAmount).ToList();
                else if (sortInfo.FieldName == "SalesCount")
                    _dishSaleFullInfo.DishSaleInfos = _dishSaleFullInfo.DishSaleInfos.OrderByDescending(t => t.SalesCount).ToList();
            }
            else
            {
                if (sortInfo.FieldName == "Name")
                    _dishSaleFullInfo.DishSaleInfos = _dishSaleFullInfo.DishSaleInfos.OrderBy(t => t.Name).ToList();
                else if (sortInfo.FieldName == "SalesAmount")
                    _dishSaleFullInfo.DishSaleInfos = _dishSaleFullInfo.DishSaleInfos.OrderBy(t => t.SalesAmount).ToList();
                else if (sortInfo.FieldName == "SalesCount")
                    _dishSaleFullInfo.DishSaleInfos = _dishSaleFullInfo.DishSaleInfos.OrderBy(t => t.SalesCount).ToList();
            }

            UpdateDishIndex();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            _curSelectTbBtn = (ToggleButton)sender;
            var enumType = (EnumStatisticsPeriodsType)Enum.Parse(typeof(EnumStatisticsPeriodsType), (string)((ToggleButton)sender).Tag);
            TaskService.Start(enumType, GetDishSaleDataProcess, GetDishSaleDataComplete, "获取品项明细数据中...");
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
            DcDishView.Focus();
            Keyboard.Press(Key.PageUp);
            Keyboard.Release(Key.PageUp);
        }

        private void BtnNext_OnClick(object sender, RoutedEventArgs e)
        {
            DcDishView.Focus();
            Keyboard.Press(Key.PageDown);
            Keyboard.Release(Key.PageDown);
        }

        private void ReportDishInfoControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!this.IsInDesignTool())
                TbToday.IsChecked = true;
        }

        #endregion

        #region Private Methods

        private object GetDishSaleDataProcess(object param)
        {
            IRestaurantService service = new RestaurantServiceImpl();
            return service.GetDishSaleInfo((EnumStatisticsPeriodsType)param);
        }

        private void GetDishSaleDataComplete(object arg)
        {
            try
            {
                var result = (Tuple<string, DishSaleFullInfo>)arg;
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    frmBase.Warning(result.Item1);
                    return;
                }

                _dishSaleFullInfo = result.Item2;
                UpdateDishIndex();
                DishSaleInfos.Clear();
                if (result.Item2 != null)
                    result.Item2.DishSaleInfos.ForEach(DishSaleInfos.Add);
            }
            catch (Exception ex)
            {
                AllLog.Instance.E(ex);
            }
            finally
            {
                if (DishSaleInfos != null)
                {
                    TbTotalDish.Text = string.Format("品项个数：{0}", DishSaleInfos.Count);
                    TbTotalCount.Text = string.Format("数量总计：{0}", DishSaleInfos.Sum(t => t.SalesCount));
                    TbTotalAmount.Text = string.Format("金额合计：{0}", DishSaleInfos.Sum(t => t.SalesAmount));
                }
            }
        }

        /// <summary>
        /// 更新菜品序号。
        /// </summary>
        private void UpdateDishIndex()
        {
            var index = 1;
            _dishSaleFullInfo.DishSaleInfos.ForEach(t => t.Index = index++);
        }

        #endregion

    }
}
