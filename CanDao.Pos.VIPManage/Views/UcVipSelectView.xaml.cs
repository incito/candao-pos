using CanDao.Pos.Common.Controls.CSystem;
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

namespace CanDao.Pos.VIPManage.Views
{
    /// <summary>
    /// UcVipSelect.xaml 的交互逻辑
    /// </summary>
    public partial class UcVipSelectView : UserControlBase
    {

        public UcVipSelectView()
        {
            InitializeComponent();
            this.Loaded += UcVipSelectView_Loaded;
           
        }

        void UcVipSelectView_Loaded(object sender, RoutedEventArgs e)
        {
            TexSelectNum.Focus();
        }
    }
}
