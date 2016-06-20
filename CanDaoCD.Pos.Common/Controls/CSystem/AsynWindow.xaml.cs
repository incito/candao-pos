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

namespace CanDaoCD.Pos.Common.Controls.CSystem
{
    /// <summary>
    /// AsynWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AsynWindow : Window
    {
        #region 事件
        public Action ActionCancel;
        #endregion

        #region 构造函数
        public AsynWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if(ActionCancel!=null)
            {
                ActionCancel();
            }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="showText"></param>
        public void SetShowText(string showText)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                TbShowText.Text = showText;
            }));
         
        }
        #endregion
    }
}
