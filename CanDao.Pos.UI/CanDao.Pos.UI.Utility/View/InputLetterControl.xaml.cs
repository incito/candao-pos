using System.Windows;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 字符条状辅助输入控件。
    /// </summary>
    public partial class InputLetterControl
    {
        public InputLetterControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 元素间距。
        /// </summary>
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(InputLetterControl), new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// 字母大小。
        /// </summary>
        public double ItemSize
        {
            get { return (double)GetValue(ItemSizeProperty); }
            set { SetValue(ItemSizeProperty, value); }
        }

        public static readonly DependencyProperty ItemSizeProperty =
            DependencyProperty.Register("ItemSize", typeof(double), typeof(InputLetterControl), new PropertyMetadata(40d));



        public UIElement FocusElement
        {
            get { return (UIElement)GetValue(FocusElementProperty); }
            set { SetValue(FocusElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FocusElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FocusElementProperty =
            DependencyProperty.Register("FocusElement", typeof(UIElement), typeof(InputLetterControl), new PropertyMetadata(null, FocusElement_PropertyChanged));

        private static void FocusElement_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (InputLetterControl) d;
            ((InputHelperControlVm) ctrl.DataContext).FocusElement = ctrl.FocusElement;
        }
    }
}
