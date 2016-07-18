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
using System.Windows.Shapes;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 餐道会员查询窗口。
    /// </summary>
    public partial class CanDaoMemberQueryWindow
    {
        public CanDaoMemberQueryWindow()
        {
            InitializeComponent();
            DataContext = new CanDaoMemberQueryWndVm { OwnerWindow = this };
        }

        private void CanDaoMemberQueryWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ((CanDaoMemberQueryWndVm)DataContext).QueryMemberAsync();
        }

        private void CanDaoMemberQueryWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TbMobile.Focus();
        }
    }
}
