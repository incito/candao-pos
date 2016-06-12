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

namespace CanDaoCD.Pos.ResourceService.Controls
{
    /// <summary>
    /// CtlDigitalKeyboard.xaml 的交互逻辑
    /// </summary>
    public partial class CtlDigitalKeyboard : UserControl
    {

#region 字段

        private Dictionary<string, TextBox> _dictionary;
#endregion

#region 构造函数
        public CtlDigitalKeyboard()
        {
            InitializeComponent();
            _dictionary = new Dictionary<string, TextBox>();
        }
#endregion
        #region 属性
        /// <summary>
        /// 当前关联控件
        /// </summary>
        public UIElement CurrentElement {
            get
            {
                return (UIElement)GetValue(CurrentElementProperty);
            }
            set
            {
                SetValue(CurrentElementProperty, value);

                if (CurrentElement is TextBox)
                {
                    var textBox = CurrentElement as TextBox;
                    if (!_dictionary.ContainsKey(textBox.Name))
                    {
                        _dictionary.Add(textBox.Name, textBox);
                        textBox.PreviewKeyDown += CtlDigitalKeyboard_PreviewKeyDown;
                    }
                }
            }
        }

        public static readonly DependencyProperty CurrentElementProperty = DependencyProperty.Register(
           "CurrentElement", typeof(UIElement), typeof(CtlDigitalKeyboard));

      

        public static readonly DependencyProperty MaxNumProperty = DependencyProperty.Register(
        "MaxNum", typeof(int), typeof(CtlDigitalKeyboard), new PropertyMetadata(0));
        public int MaxNum
        {
            get { return (int)GetValue(MaxNumProperty); }
            set { SetValue(MaxNumProperty, value); }
        }

        public static readonly DependencyProperty MinNumProperty = DependencyProperty.Register(
        "MinNum", typeof(int), typeof(CtlDigitalKeyboard), new PropertyMetadata(0));
        public int MinNum
        {
            get { return (int)GetValue(MinNumProperty); }
            set { SetValue(MinNumProperty, value); }
        }
        #endregion

        #region 事件
        public Action SureAction
        {
            get
            {
                return (Action)GetValue(SureActionProperty);
            }
            set
            {
                SetValue(SureActionProperty, value);
            }
        }

        public static readonly DependencyProperty SureActionProperty = DependencyProperty.Register(
            "SureAction", typeof(Action), typeof(CtlDigitalKeyboard));

        public Action<TextBox> TextEnterAction
        {
            get
            {
                return (Action<TextBox>)GetValue(TextEnterProperty);
            }
            set
            {
                SetValue(TextEnterProperty, value);
            }
        }

        public static readonly DependencyProperty TextEnterProperty = DependencyProperty.Register(
            "TextEnterAction", typeof(Action<TextBox>), typeof(CtlDigitalKeyboard));
   
        #endregion

        #region 私有方法
        /// <summary>
        /// 输入Enter键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CtlDigitalKeyboard_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TextEnterAction != null)
                {
                    TextEnterAction(sender as TextBox);
                }
            }
        }

        /// <summary>
        /// 数字键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDigital_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentElement != null)
                {
                  
                    string appText = (sender as Button).Content.ToString();
                    var curText = (TextBox) CurrentElement;
                    curText.AppendText(appText);
                    TextChange(curText);
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 退格操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDele_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentElement != null)
                {
                    var curText = (TextBox)CurrentElement;
                    if (curText.Text.Length > 0)
                    {
                        curText.Text = curText.Text.Remove(curText.Text.Length - 1);
                        TextChange(curText);
                    }
                    else
                    {
                        curText.Focus();
                    }
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 小数点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPoint_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentElement != null)
                {
                    var curText = (TextBox)CurrentElement;
                    if (!curText.Text.Contains("."))
                    {
                        curText.AppendText(".");
                        TextChange(curText);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSure_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SureAction!= null)
                {
                    SureAction();
                }
       
            }
            catch
            {
                
              
            }
           
        }
        /// <summary>
        /// 输入文本框焦点
        /// </summary>
        /// <param name="textBox"></param>
        private void TextChange(TextBox textBox)
        {
            //判断输入范围
            CheckNum(textBox);

            textBox.Select(textBox.Text.Length, 0);
            textBox.Focus();
           
        }
        /// <summary>
        /// 检查文字范围
        /// </summary>
        /// <param name="textBox"></param>
        private void CheckNum(TextBox textBox)
        {
            if (MaxNum > 0)
            {
                float textNum;
                if (float.TryParse(textBox.Text, out textNum))
                {
                    if (textNum < MinNum || textNum > MaxNum)//输入信息小于最小值或大于最大值
                    {
                        textBox.Text = "";
                    }
                }
                else
                {
                    textBox.Text = "";
                }
            }
        }
        #endregion

    }
}