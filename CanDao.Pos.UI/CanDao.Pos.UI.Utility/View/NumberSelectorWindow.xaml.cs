using System;
using System.Windows;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 数量选择窗口。
    /// </summary>
    public partial class NumberSelectorWindow
    {
        public NumberSelectorWindow(string title, decimal num, decimal maxNum = 0)
        {
            InitializeComponent();
            DataContext = new NumberSelectorWndVm(title, num, maxNum) { OwnerWindow = this };
        }

        /// <summary>
        /// 输入的数量。
        /// </summary>
        public decimal InputNum
        {
            get { return ((NumberSelectorWndVm)DataContext).InputNum; }
        }

        private void NumberSelectorWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TbNum.Focus();
        }
    }
}
