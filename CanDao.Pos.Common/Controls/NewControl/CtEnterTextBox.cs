using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanDao.Pos.Common.Controls.NewControl
{
    /// <summary>
    /// 带回车事件文本框
    /// </summary>
    public class CtEnterTextBox : TextBox
    {
        #region 属性

        public static readonly DependencyProperty EnterActionProperty = DependencyProperty.Register("EnterAction",
            typeof (Action), typeof (CtEnterTextBox));

        /// <summary>
        /// 回车事件
        /// </summary>
        public Action EnterAction
        {
            get { return (Action) GetValue(EnterActionProperty); }
            set { SetValue(EnterActionProperty, value); }
        }

        #endregion

        #region 构造函数

        public CtEnterTextBox()
        {

            this.KeyDown += CtDigitalTextBox_PreviewKeyDown;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.Height = 35;
        }

        #endregion

        #region 私有方法

        private void CtDigitalTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //回车
                if (EnterAction != null)
                {
                    EnterAction();
                }
            }
        }

        #endregion
    }
}
