using System.Windows;
using System.Windows.Controls;

namespace CanDao.Pos.Common.Controls
{
    /// <summary>
    /// 标识控件。
    /// </summary>
    public class FlagControl : ContentControl
    {
        static FlagControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlagControl), new FrameworkPropertyMetadata(typeof(FlagControl)));
        }


        /// <summary>
        /// 标记宽度。
        /// </summary>
        public double FlagWidth
        {
            get { return (double)GetValue(FlagWidthProperty); }
            set { SetValue(FlagWidthProperty, value); }
        }

        public static readonly DependencyProperty FlagWidthProperty =
            DependencyProperty.Register("FlagWidth", typeof(double), typeof(FlagControl), new PropertyMetadata(2d));


    }
}
