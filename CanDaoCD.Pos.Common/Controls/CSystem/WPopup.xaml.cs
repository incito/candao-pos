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
    /// WPopup.xaml 的交互逻辑
    /// </summary>
    public partial class WPopup : Window
    {
        public WPopup()
        {
            InitializeComponent();
        }

        #region 属性
        /// <summary>
        /// 是否模态窗台
        /// </summary>
        public bool IsDialog
        {
            set;
            get;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="showUc"></param>
        public void SetShowUc(UserControl showUc)
        {
            CtlShowFm.Content = showUc;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (IsDialog)
            {
                this.DialogResult = false;
            }
            else
            {
                this.Close();
            }
        }
        #endregion
    }
}
