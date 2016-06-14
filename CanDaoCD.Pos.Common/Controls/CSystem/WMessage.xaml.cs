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
    /// WMessage.xaml 的交互逻辑
    /// </summary>
    public partial class WMessage : Window
    {

        public WMessage()
        {
            InitializeComponent();
        }

        #region 公共方法
        /// <summary>
        /// 初始化信息
        /// </summary>
        /// <param name="message">显示信息</param>
        /// <param name="isShowClose">是否显示取消按钮</param>
        public void Init(string message, bool isShowClose=true)
        {
            TexMessage.Text = message;
            if (!isShowClose)
            {
                BtnClose.Visibility = Visibility.Collapsed;
            }
           
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        #endregion
    }
}