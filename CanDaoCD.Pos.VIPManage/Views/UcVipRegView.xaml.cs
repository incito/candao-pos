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
using CanDaoCD.Pos.Common.Controls.CSystem;

namespace CanDaoCD.Pos.VIPManage.Views
{
    /// <summary>
    /// UcVipReg.xaml 的交互逻辑
    /// </summary>
    public partial class UcVipRegView : UserControlBase
    {
        public UcVipRegView()
        {
            InitializeComponent();
        }

        #region 虚拟键盘

        private void TexTelNum_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.CurrentElement = TexTelNum;
        }

        private void TexCode_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.CurrentElement = TexCode;
        }

        private void TexUserName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.CurrentElement = TexUserName;
        }

        private void TexPsw_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.CurrentElement = TexPsw;
        }

        private void TexPswConfirm_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.CurrentElement = TexPswConfirm;
        }

        #endregion
    }
}
