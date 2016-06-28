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
using KYPOS.Reports;

namespace KYPOS.Reports
{
    /// <summary>
    /// CtTextChange.xaml 的交互逻辑
    /// </summary>
    public partial class CtTextChange : UserControl
    {

        #region 属性

        private CtDigitalTextBox _currentTextBox;

        #endregion

        #region 构造函数

        public CtTextChange()
        {
            InitializeComponent();
            TexHour.GotFocus += TexHour_GotFocus;
            TexMinute.GotFocus += TexMinute_GotFocus;
            TexSecond.GotFocus += TexSecond_GotFocus;


        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        public void Init(int hour, int minute, int second)
        {
            TexHour.Text = hour.ToString();
            TexMinute.Text = minute.ToString();
            TexSecond.Text = second.ToString();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public List<int> GetData()
        {
            var time=new List<int>();
            time.Add(int.Parse(TexHour.Text));
            time.Add(int.Parse(TexMinute.Text));
            time.Add(int.Parse(TexSecond.Text));
            return time;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 秒
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TexSecond_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentTextBox = TexSecond;
        }

        /// <summary>
        /// 分钟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TexMinute_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentTextBox = TexMinute;
        }

        /// <summary>
        /// 小时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TexHour_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentTextBox = TexHour;
        }

        /// <summary>
        /// 向上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUp_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentTextBox != null)
            {
                var oldValue = int.Parse(_currentTextBox.Text);

                if (oldValue == _currentTextBox.MaxValue)
                {
                    _currentTextBox.Text = _currentTextBox.MinValue.ToString();
                }
                else
                {
                    _currentTextBox.Text = (oldValue + 1).ToString();
                }
            }

        }

        /// <summary>
        /// 向下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDown_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentTextBox != null)
            {
                var oldValue = int.Parse(_currentTextBox.Text);

                if (oldValue == _currentTextBox.MinValue)
                {
                    _currentTextBox.Text = _currentTextBox.MaxValue.ToString();
                }
                else
                {
                    _currentTextBox.Text = (oldValue - 1).ToString();
                }
            }
        }

        #endregion

    }
}
