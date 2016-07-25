using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanDao.Pos.VIPManage.Operates
{
    public class ButtonSend : Button
    {
        #region 字段

        /// <summary>
        /// 鼠标按下计时器
        /// </summary>
        private Timer _downTimer;

        private int _executeTime = 60;

        public static readonly DependencyProperty IsStartTimeProperty = DependencyProperty.Register(
     "IsStartTime", typeof(bool), typeof(ButtonSend),new PropertyMetadata(defaultValue: false, 
      propertyChangedCallback: null,
      coerceValueCallback: coerceValueCallback));
        /// <summary>
        /// 默认文本（未选中）
        /// </summary>
        public bool IsStartTime
        {
            get { return (bool)GetValue(IsStartTimeProperty); }
            set
            {
                SetValue(IsStartTimeProperty, value);

            }
        }

        private static object coerceValueCallback(DependencyObject d, object baseValue)
        {
            if (baseValue != null)
            {
                var control = d as ButtonSend;
                if (control != null & (bool) baseValue)
                {
                    control.IsEnabled = false;
                    control._downTimer.Start();
                }
            }
            return baseValue;
        }
        #endregion

        public ButtonSend()
        {
            _downTimer = new Timer();
            _downTimer.Interval = 1000;
            _downTimer.Elapsed += timer_Elapsed;
           
        }
       


        #region 私有方法

        /// <summary>
        /// 间隔运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _executeTime--;

            Dispatcher.Invoke(new Action(() =>
            {
                if (_executeTime < 0)
                {
                    _downTimer.Stop();
                    _executeTime = 60;
                    this.Content = "发送";
                    this.IsEnabled = true;
                }
                else
                {
                    this.Content = _executeTime.ToString();
                }

            }));
        }
        #endregion
    }
}
