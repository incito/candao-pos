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

namespace KYPOS.OpenTables
{
    /// <summary>
    /// UCCashboxPsw.xaml 的交互逻辑
    /// </summary>
    public partial class UCCashboxPswView : UserControlBase
    {
        public Action EnterAction { set; get; }
        
        public UCCashboxPswView()
        {
            InitializeComponent();
            this.Loaded += UCCashboxPswView_Loaded;
            
        }

        void UCCashboxPswView_Loaded(object sender, RoutedEventArgs e)
        {
            TexPassWord.HorizontalContentAlignment =HorizontalAlignment.Left;
            TexPassWord.VerticalContentAlignment = VerticalAlignment.Center;
            TexPassWord.Focus();
        }

        private void TexPassWord_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (EnterAction != null)
                {
                    EnterAction();
                }
            }
        }
    }
}
