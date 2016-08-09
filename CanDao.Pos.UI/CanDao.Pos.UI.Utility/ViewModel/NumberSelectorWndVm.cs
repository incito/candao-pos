using CanDao.Pos.Common;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 数量选择控件窗口的VM。
    /// </summary>
    public class NumberSelectorWndVm : NormalWindowViewModel
    {
        public NumberSelectorWndVm(string title, decimal num, decimal maxNum)
        {
            Title = title;
            InputNum = num;
            MaxNum = maxNum;
        }

        /// <summary>
        /// 显示的标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 输入的数量。
        /// </summary>
        public decimal InputNum { get; set; }

        /// <summary>
        /// 最大数量。
        /// </summary>
        private decimal MaxNum { get; set; }

        protected override void Confirm(object param)
        {
            if (MaxNum > 0 && InputNum > MaxNum)
            {
                MessageDialog.Warning(string.Format("数量不能超过：{0}", MaxNum));
                return;
            }

            base.Confirm(param);
        }

        protected override bool CanConfirm(object param)
        {
            return InputNum > 0;
        }
    }
}