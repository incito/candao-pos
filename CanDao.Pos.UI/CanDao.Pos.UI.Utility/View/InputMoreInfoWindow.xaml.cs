using System.Windows;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// InputMoreInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputMoreInfoWindow
    {
        public InputMoreInfoWindow(string title, string info)
        {
            InitializeComponent();
            DataContext = new InputMoreInfoWndVm(title, info) { OwnerWindow = this };
        }

        public string InputInfo
        {
            get { return ((InputMoreInfoWndVm)DataContext).InputInfo; }
        }

        private void InputMoreInfoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TbMoreInfo.Focus();
        }
    }
}
