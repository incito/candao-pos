namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 计算折扣类金额请求类。
    /// </summary>
    public class CalcDiscountAmountRequest
    {
        /// <summary>
        /// 机器编号。IP地址或者MAC地址。
        /// </summary>
        public string machineno { get; set; }

        /// <summary>
        /// 当前用户ID。
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 订单号。
        /// </summary>
        public string orderid { get; set; }

        /// <summary>
        /// 优惠券主键ID。（输入自定义折扣时，这里为空）
        /// </summary>
        public string preferentialid { get; set; }

        /// <summary>
        /// 优惠折扣率。（如果使用优惠券则这里传0，如果自定义折扣计算时传自定义的值）
        /// </summary>
        public string disrate { get; set; }

        /// <summary>
        /// 优惠券类型。
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 优惠券子类型。
        /// </summary>
        public string sub_type { get; set; }

        /// <summary>
        /// 记录所有已选的挂帐和优免金额 ，传给后台计算的时候去掉优惠。
        /// </summary>
        public string preferentialAmt { get; set; }
    }
}