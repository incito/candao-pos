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
    /// UcVipSelect.xaml 的交互逻辑
    /// </summary>
    public partial class UcVipSelectView : UserControlBase
    {
#region 属性

        public int SelectModel { set; get; }

        public Action<string> EntAction { set; get; }
#endregion
        public UcVipSelectView()
        {
            InitializeComponent();
            SelectModel = 0;

        }

        #region UI键盘事件
        /// <summary>
        /// 电话号码绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TexTelNum_OnGotFocus(object sender, RoutedEventArgs e)
        {
            //Keyboard.CurrentElement = TexTelNum;

            SelectModel = 1;
        }
        /// <summary>
        /// 实体卡号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TexCardNum_OnGotFocus(object sender, RoutedEventArgs e)
        {
          
            SelectModel = 2;
        }

        #endregion

        private void TexTelNum_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (EntAction != null)
            {
                if (e.Key == Key.Enter)
                {
                    EntAction(TexTelNum.Text);
                }
            }
        }

        private void TexCardNum_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (EntAction != null)
            {
                if (e.Key == Key.Enter)
                {
                    EntAction(TexCardNum.Text);
                }
            }
        }
    }
}
