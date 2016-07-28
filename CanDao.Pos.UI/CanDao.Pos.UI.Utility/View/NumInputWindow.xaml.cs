using System;
using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 菜品称重窗口。
    /// </summary>
    public partial class NumInputWindow
    {
        #region Constructor

        /// <summary>
        /// 数字输入窗口。
        /// </summary>
        /// <param name="tipInfo">提示信息。</param>
        /// <param name="numWatermark">数字输入框的水印提示。</param>
        /// <param name="maxNum">最大输入数量，为0时不做判断。</param>
        /// <param name="allowDot">是否允许输入小数点。</param>
        public NumInputWindow(string tipInfo, string numWatermark, decimal maxNum, bool allowDot = true)
        {
            InitializeComponent();
            TipInfo = tipInfo;
            NumWatermark = numWatermark;
            MaxNum = maxNum;
            InputNumCtrl.ShowDot = allowDot;

            DataContext = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 提示文字信息。
        /// </summary>
        public string TipInfo { get; set; }

        /// <summary>
        /// 数字水印文字。
        /// </summary>
        public string NumWatermark { get; set; }

        #region InputNumText

        /// <summary>
        /// 输入的数字文本。
        /// </summary>
        public string InputNumText
        {
            get { return (string)GetValue(InputNumTextProperty); }
            set { SetValue(InputNumTextProperty, value); }
        }

        public static readonly DependencyProperty InputNumTextProperty =
            DependencyProperty.Register("InputNumText", typeof(string), typeof(NumInputWindow), new PropertyMetadata("", InputNumTextPropertyChangedCallback));

        private static void InputNumTextPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var ctrl = (NumInputWindow)sender;
            var isEmptyText = string.IsNullOrEmpty(ctrl.InputNumText) || string.IsNullOrEmpty(ctrl.InputNumText.Trim());
            try
            {
                ctrl.InputNum = !isEmptyText ? Convert.ToDecimal(ctrl.InputNumText) : 0;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("数字输入窗口里转换成数字失败。", ex);
            }
            //ctrl.CloseBtnEnable = ctrl.InputNum > 0;
        }

        #endregion

        #region AllowDot

        /// <summary>
        /// 允许输入小数点。
        /// </summary>
        public bool AllowDot
        {
            get { return (bool)GetValue(AllowDotProperty); }
            set { SetValue(AllowDotProperty, value); }
        }

        public static readonly DependencyProperty AllowDotProperty =
            DependencyProperty.Register("AllowDot", typeof(bool), typeof(NumInputWindow), new PropertyMetadata(true));

        #endregion

        /// <summary>
        /// 输入的数量。
        /// </summary>
        public decimal InputNum { get; set; }

        /// <summary>
        /// 最大输入数量，为0表示不起作用。
        /// </summary>
        public decimal MaxNum { get; set; }

        #endregion

        #region Protected Methods

        protected override void CloseBtnOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (MaxNum > 0 && InputNum > MaxNum)
            {
                MessageDialog.Warning(string.Format("输入数量不能超过：{0}", MaxNum));
                return;
            }
            base.CloseBtnOnClick(sender, routedEventArgs);
        }

        #endregion
    }
}
