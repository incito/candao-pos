using System;
using System.Collections.Generic;
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

namespace Library
{
    /// <summary>
    /// SelectClearMachineStepWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectClearMachineStepWindow
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canEndWork">允许结业。</param>
        public SelectClearMachineStepWindow(bool canEndWork)
        {
            InitializeComponent();
            CanEndWork = canEndWork;
            CanNotEndWork = !canEndWork;
            DataContext = this;
        }

        /// <summary>
        /// 是否允许结业。
        /// </summary>
        public bool CanEndWork { get; set; }

        /// <summary>
        /// 是否不允许结业。
        /// </summary>
        public bool CanNotEndWork { get; set; }

        /// <summary>
        /// 是否选择结业操作。
        /// </summary>
        public bool DoEndWork { get; set; }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnChangeShift_OnClick(object sender, RoutedEventArgs e)
        {
            DoEndWork = false;
            DialogResult = true;
            Close();
        }

        private void BtnEndWork_OnClick(object sender, RoutedEventArgs e)
        {
            DoEndWork = true;
            DialogResult = true;
            Close();
        }
    }
}
