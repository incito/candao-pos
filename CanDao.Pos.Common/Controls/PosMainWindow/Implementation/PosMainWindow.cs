using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanDao.Pos.Common.Controls
{
    public class PosMainWindow : Window
    {
        static PosMainWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PosMainWindow), new FrameworkPropertyMetadata(typeof(PosMainWindow)));
        }

        public CornerRadius WindowCornerRadius
        {
            get { return (CornerRadius)GetValue(WindowCornerRadiusProperty); }
            set { SetValue(WindowCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty WindowCornerRadiusProperty =
            DependencyProperty.Register("WindowCornerRadius", typeof(CornerRadius), typeof(PosMainWindow), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// 标题中间部分内容控件。
        /// </summary>
        public Object TitleContent
        {
            get { return GetValue(TitleContentProperty); }
            set { SetValue(TitleContentProperty, value); }
        }

        public static readonly DependencyProperty TitleContentProperty =
            DependencyProperty.Register("TitleContent", typeof(Object), typeof(PosMainWindow), new PropertyMetadata(null));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var closeBtn = GetTemplateChild("PART_CloseBtn") as Button;
            if (closeBtn != null)
                closeBtn.Click += CloseBtnOnClick;

            var minBtn = GetTemplateChild("PART_Min") as Button;
            if (minBtn != null)
                minBtn.Click += MinBtnOnClick;

            var titleCtrl = GetTemplateChild("PART_Title") as Border;
            if (titleCtrl != null)
            {
                titleCtrl.MouseLeftButtonDown += TitleCtrlOnMouseLeftButtonDown;
                titleCtrl.CornerRadius = new CornerRadius(WindowCornerRadius.TopLeft, WindowCornerRadius.TopRight, 0, 0);
            }
        }

        private void MinBtnOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TitleCtrlOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            DragMove();
        }

        private void CloseBtnOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

    }
}
