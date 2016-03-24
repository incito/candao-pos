using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebServiceReference.ServiceImpl;

namespace Library
{
    /// <summary>
    /// 选择小熊窗口。
    /// </summary>
    public partial class SelectAnimalWindow
    {
        /// <summary>
        /// 玩偶集合缓存。
        /// </summary>
        private static List<string> AnimalsCache;

        public SelectAnimalWindow()
        {
            InitializeComponent();
            Animals = new ObservableCollection<string>();

            DataContext = this;
        }

        public ObservableCollection<string> Animals { get; private set; }

        public string SelectedAnimal { get; set; }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void SelectAnimalWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (AnimalsCache == null || !AnimalsCache.Any())
                TaskService.Start(null, GetAllAnimalProcess, GetAllAnimalComplete);
            else
                AnimalsCache.ForEach(Animals.Add);
        }

        private object GetAllAnimalProcess(object param)
        {
            var service = new RestaurantServiceImpl();
            return service.GetAllAnimals();
        }

        private void GetAllAnimalComplete(object arg)
        {
            var result = (Tuple<string, List<string>>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                frmBase.Warning(result.Item1);
                return;
            }

            AnimalsCache = result.Item2;
            if (AnimalsCache != null && AnimalsCache.Any())
                AnimalsCache.ForEach(Animals.Add);
        }
    }
}
