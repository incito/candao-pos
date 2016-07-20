namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 设置优惠券常用或不常用请求类。
    /// </summary>
    public class SetCouponFavorRequest
    {
        /// <summary>
        /// 优惠券ID。
        /// </summary>
        public string preferential { get; set; }

        /// <summary>
        /// 类型。0代表设为常用，1代表设为不常用
        /// </summary>
        public string operationtype { get; set; }
    }
}