namespace CanDao.Pos.Model.Request
{
    public class SaveCouponInfoRequest
    {
        /// <summary>
        /// 优惠名称。
        /// </summary>
        public string yhname { get; set; }

        /// <summary>
        /// 合作单位，对应公司名称。
        /// </summary>
        public string partnername { get; set; }

        /// <summary>
        /// 折扣率。
        /// </summary>
        public decimal couponrate { get; set; }

    }
}