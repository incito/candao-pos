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

        /// <summary>
        /// 确认按钮文本。
        /// </summary>
        public string CloseBtnText
        {
            get { return (string)GetValue(CloseBtnTextProperty); }
            set { SetValue(CloseBtnTextProperty, value); }
        }

        public static readonly DependencyProperty CloseBtnTextProperty =
            DependencyProperty.Register("CloseBtnText", typeof(string), typeof(PosMsgWindow), new PropertyMetadata("确认"));

        /// <summary>
        /// 关闭按钮文本。
        /// </summary>
        public string CancelBtnText
        {
            get { return (string)GetValue(CancelBtnTextProperty); }
            set { SetValue(CancelBtnTextProperty, value); }
        }

        public static readonly DependencyProperty CancelBtnTextProperty =
            DependencyProperty.Register("CancelBtnText", typeof(string), typeof(PosMsgWindow), new PropertyMetadata("关闭"));

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
