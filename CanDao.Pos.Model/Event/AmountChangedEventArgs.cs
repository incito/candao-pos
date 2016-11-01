using System;

namespace CanDao.Pos.Model.Event
{
    /// <summary>
    /// 金额改变事件的参数类。
    /// </summary>
    public class AmountChangedEventArgs : EventArgs
    {
        public AmountChangedEventArgs(bool isCashAmountChanged)
        {
            IsCashAmountChanged = isCashAmountChanged;
        }

        /// <summary>
        /// 是否是现金金额改变。
        /// </summary>
        public bool IsCashAmountChanged { get; set; }
    }
}