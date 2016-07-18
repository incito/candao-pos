using System.Windows;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 数字条状输入辅助控件。
    /// </summary>
    public partial class InputHelperStripControl
    {
        public InputHelperStripControl()
        {
            InitializeComponent();
        }

        public double ItemSize
        {
            get { return (double)GetValue(ItemSizeProperty); }
            set { SetValue(ItemSizeProperty, value); }
        }

        public static readonly DependencyProperty ItemSizeProperty =
            DependencyProperty.Register("ItemSize", typeof(double), typeof(InputHelperStripControl), new PropertyMetadata(40d));


        /// <summary>
        /// 是否允许输入小数点。
        /// </summary>
        public bool AllowDot
        {
            get { return (bool)GetValue(AllowDotProperty); }
            set { SetValue(AllowDotProperty, value); }
        }

        public static readonly DependencyProperty AllowDotProperty =
            DependencyProperty.Register("AllowDot", typeof(bool), typeof(InputHelperStripControl), new PropertyMetadata(true));


    }
}
