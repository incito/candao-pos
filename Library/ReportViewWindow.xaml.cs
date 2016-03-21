using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common;
using Models;
using Models.Enum;
using WebServiceReference.IService;
using WebServiceReference.ServiceImpl;

namespace Library
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
                Dispatcher.BeginInvoke((Action) delegate
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
    }
}
