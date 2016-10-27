namespace CanDao.Pos.Model.Request
{
    public class UsePreferentialRequest
    {
        /// <summary>
        /// 订单ID。
        /// </summary>
        public string orderid { set; get; }

        /// <summary>
        /// 优惠券ID。
        /// </summary>
        public string preferentialid { set; get; }

        /// <summary>
        /// 折扣率。
        /// </summary>
        public decimal disrate { set; get; }

        /// <summary>
        /// 优惠券类型。
        /// </summary>
        public string type { set; get; }

        /// <summary>
        /// 子类型。
        /// </summary>
        public string sub_type { set; get; }

        /// <summary>
        /// 总优惠。
        /// </summary>
        public string preferentialAmt { set; get; }

        /// <summary>
        /// 雅座优惠券名称。
        /// </summary>
        public string preferentialName { get; set; }

        /// <summary>
        /// 总优免金额。
        /// </summary>
        public string toalFreeAmount { set; get; }

        /// <summary>
        /// 总挂账。
        /// </summary>
        public string toalDebitAmount { set; get; }

        /// <summary>
        /// 总挂账多收。
        /// </summary>
        public string toalDebitAmountMany { set; get; }

        /// <summary>
        /// 优免调整。
        /// </summary>
        public string adjAmout { set; get; }

        /// <summary>
        /// 优惠券使用数量。
        /// </summary>
        public string preferentialNum { set; get; }

        /// <summary>
        /// 手动输入优免金额。
        /// </summary>
        public string preferentialAmout { set; get; }

        /// <summary>
        /// 是否用户自己输入。
        /// </summary>
        public string isCustom { set; get; }

        public string dishid { set; get; }

        public string unit { get; set; }
    }
}
