using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanDao.Pos.Common.Controls
{

    public class PosBaseWindow : Window
    {

        /// <summary>
        /// 关闭按钮文本。
        /// </summary>
        public string CloseBtnText
        {
            get { return (string)GetValue(CloseBtnTextProperty); }
            set { SetValue(CloseBtnTextProperty, value); }
        }

        public static readonly DependencyProperty CloseBtnTextProperty =
            DependencyProperty.Register("CloseBtnText", typeof(string), typeof(PosBaseWindow), new PropertyMetadata("关闭"));



        public CornerRadius WindowCornerRadius
        {
            get { return (CornerRadius)GetValue(WindowCornerRadiusProperty); }
            set { SetValue(WindowCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty WindowCornerRadiusProperty =
            DependencyProperty.Register("WindowCornerRadius", typeof(CornerRadius), typeof(PosBaseWindow), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// 关闭按钮是否可用。
        /// </summary>
        public bool CloseBtnEnable
        {
            get { return (bool)GetValue(CloseBtnEnableProperty); }
            set { SetValue(CloseBtnEnableProperty, value); }
        }

        public static readonly DependencyProperty CloseBtnEnableProperty =
            DependencyProperty.Register("CloseBtnEnable", typeof(bool), typeof(PosBaseWindow), new PropertyMetadata(true));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var closeBtn = GetTemplateChild("PART_CloseBtn") as Button;
            if (closeBtn != null)
                closeBtn.Click += CloseBtnOnClick;

            var titleCtrl = GetTemplateChild("PART_Title") as Border;
            if (titleCtrl != null)
            {
                titleCtrl.MouseLeftButtonDown += TitleCtrlOnMouseLeftButtonDown;
                titleCtrl.CornerRadius = new CornerRadius(WindowCornerRadius.TopLeft, WindowCornerRadius.TopRight, 0, 0);
            }
        }

        private void TitleCtrlOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            DragMove();
        }

        private void CloseBtnOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        public PosBaseWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        static PosBaseWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PosBaseWindow), new FrameworkPropertyMetadata(typeof(PosBaseWindow)));
        }
    }
}
