using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Common;
using Library;
using Models;
using Models.Enum;
using ReportsFastReport;
using WebServiceReference.IService;
using WebServiceReference.ServiceImpl;

namespace KYPOS
{
    /// <summary>
    /// 报表显示窗口。
    /// </summary>
    public partial class ReportViewWindow
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

        private const int PageSize = 11;

        #endregion

        #region Constructor

        public ReportViewWindow()
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

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ReportViewWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TbToday.IsChecked = true;
        }

        private void ButtonPrint_OnClick(object sender, RoutedEventArgs e)
        {
            if (_dishSaleFullInfo == null || _dishSaleFullInfo.DishSaleInfos == null || !_dishSaleFullInfo.DishSaleInfos.Any())
            {
                frmBase.Warning("没有需要打印的品项明细数据。");
                return;
            }

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
            var index = 1;
            _dishSaleFullInfo.DishSaleInfos.ForEach(t => t.Index = index++);
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            _curSelectTbBtn = (ToggleButton)sender;
            var enumType = (EnumDishSalePeriodsType)Enum.Parse(typeof(EnumDishSalePeriodsType), (string)((ToggleButton)sender).Tag);
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
            BtnNext.IsEnabled = true;
            var newIndex = --_curViewIndex * PageSize;
            if (newIndex < 0)
            {
                newIndex = 0;
                BtnLast.IsEnabled = false;
            }

            DcDishView.View.ScrollIntoView(newIndex);
        }

        private void BtnNext_OnClick(object sender, RoutedEventArgs e)
        {
            BtnLast.IsEnabled = true;
            var newIndex = ++_curViewIndex*PageSize;
            if (newIndex >= DishSaleInfos.Count)
            {
                newIndex = DishSaleInfos.Count - 1;
                BtnNext.IsEnabled = false;
            }

            DcDishView.View.ScrollIntoView(newIndex);
        }
        #endregion

        #region Prvate Methods

        private object GetDishSaleDataProcess(object param)
        {
            IRestaurantService service = new RestaurantServiceImpl();
            return service.GetDishSaleInfo((EnumDishSalePeriodsType)param);
        }

        private void GetDishSaleDataComplete(object arg)
        {
            try
            {
                var result = (Tuple<string, DishSaleFullInfo>) arg;
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    frmBase.Warning(result.Item1);
                    return;
                }

                _dishSaleFullInfo = result.Item2;
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
                TbTotalCount.Text = string.Format("总数：{0}", DishSaleInfos.Count);
            }
        }

        #endregion

    }
}
