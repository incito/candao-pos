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
        public bool ShowDot
        {
            get { return (bool)GetValue(ShowDotProperty); }
            set { SetValue(ShowDotProperty, value); }
        }

        public static readonly DependencyProperty ShowDotProperty =
            DependencyProperty.Register("ShowDot", typeof(bool), typeof(InputNumberControl), new PropertyMetadata(true));

    }
}
