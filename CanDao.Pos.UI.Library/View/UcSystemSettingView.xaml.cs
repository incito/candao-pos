using System;
using System.Windows;
using CanDao.Pos.UI.Library.ViewModel;

namespace CanDao.Pos.SystemConfig.Views
{
    /// <summary>
    /// 系统设置控件。
    /// </summary>
    public partial class UcSystemSettingView
    {
        public UcSystemSettingView()
        {
            InitializeComponent();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UcSystemSettingView_OnClosed(object sender, EventArgs e)
        {
            ((UcPrinterListViewModel)PrinterListView.DataContext).StopRefresh();
        }
    }
}
