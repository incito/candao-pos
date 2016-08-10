using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanDao.Pos.Common.Controls
{
    /// <summary>
    /// 带水印的密码框。
    /// </summary>
    public class WatermarkPasswordBox : WatermarkTextBox
    {
        private bool _isResponseChange;

        static WatermarkPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkPasswordBox), new FrameworkPropertyMetadata(typeof(WatermarkPasswordBox)));
        }

        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register("PasswordChar", typeof(char), typeof(WatermarkPasswordBox), new PropertyMetadata('●'));



        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(WatermarkPasswordBox), new PropertyMetadata(""));


        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (!_isResponseChange)
                return;

            var lastOffset = Text.Length;
            foreach (var change in e.Changes)
            {
                var str = Password.Remove(change.Offset, change.RemovedLength);
                str = str.Insert(change.Offset, Text.Substring(change.Offset, change.AddedLength));
                lastOffset = change.Offset;
                this.SetValue(PasswordProperty, str);
            }
            _isResponseChange = false;
            Text = ConvertToPasswordChar(Password.Length);
            SelectionStart = lastOffset + 1;
            _isResponseChange = true;
        }

        private string ConvertToPasswordChar(int passwordLength)
        {
            return "".PadRight(passwordLength, PasswordChar);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Text = string.IsNullOrEmpty(Password) ? "" : ConvertToPasswordChar(Password.Length);
            _isResponseChange = true;
        }
    }
}
