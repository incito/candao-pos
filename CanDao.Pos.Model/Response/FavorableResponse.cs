namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 第二杯半价和第二份起半价优惠的返回类。
    /// </summary>
    public class FavorableResponse
    {
        public double? orderprice { get; set; }

        /// <summary>
        /// 优惠劵名称。
        /// </summary>
        public string couponname { get; set; }

        public string decamount { get; set; }

        public int? decdishnum { get; set; }

        public string decorderprice { get; set; }
    }
}