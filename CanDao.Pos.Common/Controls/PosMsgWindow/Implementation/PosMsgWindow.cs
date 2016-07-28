using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanDao.Pos.Common.Controls
{
    /// <summary>
    /// 消息提示窗口。
    /// </summary>
    public class PosMsgWindow : Window
    {
        protected Button CloseBtn;
        protected Button CancelBtn;

        static PosMsgWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PosMsgWindow), new FrameworkPropertyMetadata(typeof(PosMsgWindow)));
        }

        public CornerRadius WindowCornerRadius
        {
            get { return (CornerRadius)GetValue(WindowCornerRadiusProperty); }
            set { SetValue(WindowCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty WindowCornerRadiusProperty =
            DependencyProperty.Register("WindowCornerRadius", typeof(CornerRadius), typeof(PosMsgWindow), new PropertyMetadata(new CornerRadius(0)));

        public bool CloseBtnEnable
        {
            get { return (bool)GetValue(CloseBtnEnableProperty); }
            set { SetValue(CloseBtnEnableProperty, value); }
        }

        public static readonly DependencyProperty CloseBtnEnableProperty =
            DependencyProperty.Register("CloseBtnEnable", typeof(bool), typeof(PosMsgWindow), new PropertyMetadata(true));



        public bool ShowCloseBtn
        {
            get { return (bool)GetValue(ShowCloseBtnProperty); }
            set { SetValue(ShowCloseBtnProperty, value); }
        }

        public static readonly DependencyProperty ShowCloseBtnProperty =
            DependencyProperty.Register("ShowCloseBtn", typeof(bool), typeof(PosMsgWindow), new PropertyMetadata(true));

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.Key)
            {
                case Key.Enter:
                    DialogResult = true;
                    break;
                case Key.Escape:
                    DialogResult = false;
                    break;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CloseBtn = GetTemplateChild("PART_CloseBtn") as Button;
            if (CloseBtn != null)
                CloseBtn.Click += CloseBtnOnClick;

            CancelBtn = GetTemplateChild("PART_CancelBtn") as Button;
            if (CancelBtn != null)
                CancelBtn.Click += CancelBtnOnClick;

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

        protected  virtual void CancelBtnOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            DialogResult = false;
        }

        protected virtual void CloseBtnOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            DialogResult = true;
        }
    }
}
