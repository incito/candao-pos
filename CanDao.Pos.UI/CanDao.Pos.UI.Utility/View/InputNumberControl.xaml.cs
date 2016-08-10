using System.Windows;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 数字输入控件。
    /// </summary>
    public partial class InputNumberControl
    {
        public InputNumberControl()
        {
            InitializeComponent();
        }

        public bool ShowConfirmBtn
        {
            get { return (bool)GetValue(ShowConfirmBtnProperty); }
            set { SetValue(ShowConfirmBtnProperty, value); }
        }

        public static readonly DependencyProperty ShowConfirmBtnProperty =
            DependencyProperty.Register("ShowConfirmBtn", typeof(bool), typeof(InputNumberControl), new PropertyMetadata(false));

        /// <summary>
        /// 是否显示小数点。
        /// </summary>
        public bool AllowDot
        {
            get { return (bool)GetValue(AllowDotProperty); }
            set { SetValue(AllowDotProperty, value); }
        }

        public static readonly DependencyProperty AllowDotProperty =
            DependencyProperty.Register("AllowDot", typeof(bool), typeof(InputNumberControl), new PropertyMetadata(true));


        /// <summary>
        /// 按钮宽度。
        /// </summary>
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(InputNumberControl), new PropertyMetadata(100d));


        /// <summary>
        /// 按钮高度。
        /// </summary>
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(InputNumberControl), new PropertyMetadata(48d));

    }
}
