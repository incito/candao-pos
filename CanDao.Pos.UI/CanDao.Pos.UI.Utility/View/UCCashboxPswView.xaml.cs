using System;
using System.Windows;

using System.Windows.Input;
using CanDao.Pos.Common.Controls;
using CanDao.Pos.Common.Controls.CSystem;


namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// UCCashboxPsw.xaml 的交互逻辑
    /// </summary>
    public partial class UCCashboxPswView 
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
