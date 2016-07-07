using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanDaoCD.Pos.Common.Controls
{
    /// <summary>
    /// 数字输入框
    /// </summary>
    public class CtDigitalTextBox : TextBox
    {
        #region 属性

        //定义依赖属性
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue",
            typeof(int), typeof(CtDigitalTextBox));

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue",
            typeof(int), typeof(CtDigitalTextBox));

        public static readonly DependencyProperty EnterActionProperty = DependencyProperty.Register("EnterAction",
          typeof(Action), typeof(CtDigitalTextBox));

        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        /// <summary>
        /// 回车事件
        /// </summary>
        public Action EnterAction
        {
            get { return (Action)GetValue(EnterActionProperty); }
            set { SetValue(EnterActionProperty, value); }
        }

        #endregion

        #region 构造函数

        public CtDigitalTextBox()
        {
            this.TextChanged += CtDigitalTextBox_TextChanged;
            this.KeyDown += CtDigitalTextBox_PreviewKeyDown;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
        }

      

        #endregion

        #region 私有方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CtDigitalTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
        /// <summary>
        /// 输入检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CtDigitalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //屏蔽中文输入和非法字符粘贴输入
                var textBox = sender as TextBox;
                TextChange[] change = new TextChange[e.Changes.Count];
                e.Changes.CopyTo(change, 0);

                int offset = change[0].Offset;
                if (change[0].AddedLength > 0)
                {
                    int num = 0;
                    if (!int.TryParse(textBox.Text, out num))
                    {
                        textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                        textBox.Select(offset, 0);
                    }
                    else if (MaxValue != 0 & num > MaxValue)
                    {
                        textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                        textBox.Select(offset, 0);
                    }
                    else if (MinValue != 0 & num < MinValue)
                    {
                        textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                        textBox.Select(offset, 0);
                    }
                }
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = MinValue.ToString();
                    textBox.SelectAll();
                }
            }
            catch
            {
            }

        }

        #endregion
    }
}
