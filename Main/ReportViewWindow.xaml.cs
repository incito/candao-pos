using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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
        private DishSaleFullInfo _dishSaleFullInfo;

        public ReportViewWindow()
        {
            InitializeComponent();
            DishSaleInfos = new ObservableCollection<DishSaleInfo>();

            DataContext = this;
        }

        /// <summary>
        /// 品项销售信息集合。
        /// </summary>
        public ObservableCollection<DishSaleInfo> DishSaleInfos { get; private set; }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ButtonPeriod_OnClick(object sender, RoutedEventArgs e)
        {
            var typeString = ((Button)sender).Tag as string;
            var enumType = (EnumDishSalePeriodsType)Enum.Parse(typeof(EnumDishSalePeriodsType), typeString);

            TaskService.Start(enumType, GetDishSaleDataProcess, GetDishSaleDataComplete, "获取品项明细数据中...");
        }

        private object GetDishSaleDataProcess(object param)
        {
            IRestaurantService service = new RestaurantServiceImpl();
            return service.GetDishSaleInfo((EnumDishSalePeriodsType)param);
        }

        private void GetDishSaleDataComplete(object arg)
        {
            var result = (Tuple<string, DishSaleFullInfo>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                frmBase.Warning(result.Item1);
                return;
            }

            _dishSaleFullInfo = result.Item2;
            DishSaleInfos.Clear();
            if (result.Item2 != null)
            {
                Dispatcher.BeginInvoke((Action)delegate
                {
                    try
                    {
                        result.Item2.DishSaleInfos.ForEach(DishSaleInfos.Add);
                    }
                    catch (Exception ex)
                    {
                        AllLog.Instance.E(ex.Message);
                    }
                });
            }
        }

        private void ReportViewWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ButtonPeriod_OnClick(BtToday, null);//默认获取今天的数据。
        }

        private void ButtonPrint_OnClick(object sender, RoutedEventArgs e)
        {
            if(_dishSaleFullInfo == null)
                return;

            ReportPrint.PrintDishSaleDetail(_dishSaleFullInfo);
        }

        private void DcDishView_OnEndSorting(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
