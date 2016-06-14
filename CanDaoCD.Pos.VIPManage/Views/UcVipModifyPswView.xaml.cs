using CanDaoCD.Pos.Common.Controls.CSystem;
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

namespace CanDaoCD.Pos.VIPManage.Views
{
    /// <summary>
    /// UcVipLogOff.xaml 的交互逻辑
    /// </summary>
    public partial class UcVipModifyPswView : UserControlBase
    {
        public UcVipModifyPswView()
        {
            InitializeComponent();
        }

        private void TexCode_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.CurrentElement = TexTelNum;
        }

        private void TexPsw_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.CurrentElement = TexPsw;
        }

        private void TexPswConfirm_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.CurrentElement = TexPswConfirm;
        }
    }
}
