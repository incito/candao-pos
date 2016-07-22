using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using CanDao.Pos.Common.Controls;
using Timer = System.Timers.Timer;

namespace CanDao.Pos.Common
{
    internal class NotifyWindow : PosMsgWindow
    {
        /// <summary>
        /// 正在显示的窗口个数。
        /// </summary>
        private static int _showingCount;

        /// <summary>
        /// 显示信息的控件。
        /// </summary>
        private TextBlock _tbMsg;

        /// <summary>
        /// 定时器。
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// 是否已经关闭。
        /// </summary>
        private bool _isClosed;

        /// <summary>
        /// 显示时间（单位毫秒）
        /// </summary>
        private int ShowTime = 3000;

        /// <summary>
        /// 窗口宽度。
        /// </summary>
        private const int WndWidth = 350;

        /// <summary>
        /// 窗口高度。
        /// </summary>
        private const int WndHeight = 200;

        /// <summary>
        /// 实例化一个提示窗口。
        /// </summary>
        /// <param name="message">提示窗口显示的提示信息。</param>
        /// <param name="ownerWindow">提示窗口所属窗口。</param>
        public NotifyWindow(string message, Window ownerWindow)
        {
            InitWindow(ownerWindow);
            FontSize = 16;
            _tbMsg.Text = message;
        }

        /// <summary>
        /// 初始化窗口。
        /// </summary>
        private void InitWindow(Window ownerWindow)
        {
            // 属性
            Width = WndWidth;
            Height = WndHeight;
            WindowStartupLocation = WindowStartupLocation.Manual;
            Owner = ownerWindow ?? Application.Current.MainWindow;
            CancelBtnText = "";
            CloseBtnText = "";
            Point point = GetMainWindowBottomRight();
            Top = point.Y - 10 - WndHeight * (_showingCount + 1);//多个通知窗口同时显示，则依次往上叠加。
            Left = point.X - 10 - WndWidth;
            ShowInTaskbar = false;
            ShowActivated = false;

            _tbMsg = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = WndWidth - 10,
            };
            Content = _tbMsg;

            // 事件
            MouseEnter += (sender, args) =>
            {
                if (_timer != null && !_isClosed)
                    _timer.Stop();
            };
            MouseLeave += (sender, args) =>
            {
                if (_timer != null && !_isClosed)
                    _timer.Start();
            };
            Loaded += (sender, args) =>
            {
                Interlocked.Increment(ref _showingCount);
                if (_timer != null)
                    _timer.Start();
            };

            // 计时器初始化
            _timer = new Timer { Interval = ShowTime };
            _timer.Elapsed += (sender, args) =>
            {
                Dispatcher.Invoke((Action)Close);
            };
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _isClosed = true;
            Interlocked.Decrement(ref _showingCount);
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
        }

        private Point GetMainWindowBottomRight()
        {
            Window wnd = Application.Current.MainWindow;
            if (wnd.WindowState != WindowState.Maximized)
                return wnd.RestoreBounds.BottomRight;
            else
                return new Point(wnd.ActualWidth - 10, wnd.ActualHeight - 10);
        }
    }
}
