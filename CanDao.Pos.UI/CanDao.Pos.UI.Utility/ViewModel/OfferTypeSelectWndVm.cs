using System;
using CanDao.Pos.Common;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 优惠类型选择窗口的VM。
    /// </summary>
    public class OfferTypeSelectWndVm : NormalWindowViewModel
    {
        #region Constructor

        public OfferTypeSelectWndVm(EnumOfferType allowOfferType)
        {
            AllowOfferType = allowOfferType;
            SelectedOfferType = AllowOfferType == EnumOfferType.Amount ? EnumOfferType.Amount : EnumOfferType.Discount;
            switch (AllowOfferType)
            {
                case EnumOfferType.Amount:
                    WndTitle = "优惠金额设置窗口";
                    break;
                case EnumOfferType.Discount:
                    WndTitle = "折扣率设置窗口";
                    break;
                case EnumOfferType.Both:
                    WndTitle = "优免金额或折扣率设置窗口";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Properties

        public string WndTitle { get; set; }

        /// <summary>
        /// 允许的优惠类型。
        /// </summary>
        public EnumOfferType AllowOfferType { get; private set; }

        /// <summary>
        /// 选中的优惠类型。
        /// </summary>
        public EnumOfferType SelectedOfferType { get; set; }

        /// <summary>
        /// 优免金额字符串。（绑定到UI，不直接绑定Amount的原因是decimal类型的话会直接显示0，导致选中时很容易点在0的后面。）
        /// </summary>
        public string AmountString { get; set; }

        /// <summary>
        /// 折扣率字符串。
        /// </summary>
        public string DiscountString { get; set; }

        /// <summary>
        /// 优免金额。
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 折扣率。
        /// </summary>
        public decimal Discount { get; set; }

        #endregion

        #region Protected Methods

        protected override void Confirm(object param)
        {
            var msg = CheckInputValid();
            if (!string.IsNullOrEmpty(msg))
            {
                MessageDialog.Warning(msg, OwnerWindow);
                return;
            }

            CloseWindow(true);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 检测输入有效性。
        /// </summary>
        /// <returns>输入数据有误则返回错误信息，全部正确则返回null。</returns>
        private string CheckInputValid()
        {
            Amount = Convert.ToDecimal(AmountString);
            Discount = Convert.ToDecimal(DiscountString);

            if (SelectedOfferType == EnumOfferType.Amount)
            {
                if (Amount == 0)
                    return "优免金额必须大于0。";
            }
            else if (SelectedOfferType == EnumOfferType.Discount)
            {
                if (Discount == 0)
                    return "折扣率必须大于0。";
                if (Discount > 100)
                    return "折扣率不能大于100";
            }
            return null;
        }

        #endregion

    }
}