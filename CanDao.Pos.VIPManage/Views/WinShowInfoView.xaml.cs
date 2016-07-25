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

namespace CanDao.Pos.VIPManage.Views
{
    /// <summary>
    /// WinShowInfo.xaml 的交互逻辑
    /// </summary>
    public partial class WinShowInfoView : Window
    {
        public WinShowInfoView()
        {
            InitializeComponent();
            this.Loaded += WinShowInfoView_Loaded;
        }

        void WinShowInfoView_Loaded(object sender, RoutedEventArgs e)
        {
            TexCardNum.Focus();
        }

        public void SetTextFocus()
        {
            TexCardNum.IsEnabled = true;
            TexCardNum.Focus();
        }

        public Action TexCardChange;


        private void TexCardNum_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TexCardChange != null)
                {
                    TexCardNum.IsEnabled = false;
                    TexCardChange();
                }
            }
        }
    }
}
