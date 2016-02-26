using System.Windows;

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
        /// <paramref name="isForcedEndWorkModel">是否是强制结业模式。</paramref>
        public SelectClearMachineStepWindow(bool canEndWork, bool isForcedEndWorkModel)
        {
            InitializeComponent();
            CanEndWork = canEndWork;
            CanNotEndWork = !canEndWork;
            BtnChangeShift.IsEnabled = !isForcedEndWorkModel;//强制结业模式下不允许倒班。
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
