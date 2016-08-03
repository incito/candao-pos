using System;
using System.Windows;
using CanDao.Pos.Model;
using CanDao.Pos.UI.MainView.ViewModel;

namespace CanDao.Pos.UI.MainView.View
{
    /// <summary>
    /// 开台窗口。 
    /// </summary>
    public partial class OpenTableWindow
    {
        public OpenTableWindow(TableInfo tableInfo)
        {
            InitializeComponent();
            DataContext = new OpenTableWndVm(tableInfo) { OwnerWindow = this };
        }

        /// <summary>
        /// 开台设置的顾客数量。（下单餐具数量使用）。
        /// </summary>
        public int CustomerNumber
        {
            get { return Convert.ToInt32(((OpenTableWndVm)DataContext).CustomerNumber); }
        }

        /// <summary>
        /// 窗口加载时设置服务员编号控件为焦点。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenTableWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            WtbWaiterId.Focus();
        }
    }
}
