using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 账单付款方式信息。
    /// </summary>
    public class BillPayInfo
    {
        private decimal p;
        private EnumBillPayType enumBillPayType;

        public BillPayInfo(decimal payAmount, EnumBillPayType payType, string bankCardNo = "", string memberCardNo = "")
        {
            PayAmount = payAmount;
            PayType = payType;
            CouponDetailId = "";
            CouponId = "";
            MemberCardNo = memberCardNo ?? "";
            BankCardNo = bankCardNo ?? "";
            CouponNum = 0;
        }

        /// <summary>
        /// 账单付款方式。
        /// </summary>
        public EnumBillPayType PayType { get; set; }

        /// <summary>
        /// 付款金额。
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 会员卡号。
        /// </summary>
        public string MemberCardNo { get; set; }

        /// <summary>
        /// 提示类标识信息。
        /// </summary>
        public string BankCardNo { get; set; }

        public int CouponNum { get; set; }

        /// <summary>
        /// 优惠券分类ID.
        /// </summary>
        public string CouponId { get; set; }

        /// <summary>
        /// 优惠券ID。
        /// </summary>
        public string CouponDetailId { get; set; }
    }
}