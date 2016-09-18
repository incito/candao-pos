namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取店铺营业简易信息的返回类。
    /// </summary>
    public class GetBusinessSimpleInfoResponse : NewHttpBaseResponse<BusinessSimpleInfoResponse>
    {
         
    }

    /// <summary>
    /// 营业简易信息返回类。
    /// </summary>
    public class BusinessSimpleInfoResponse
    {
        /// <summary>
        /// 未结金额。·
        /// </summary>
        public decimal dueamount { get; set; }
        /// <summary>
        /// 累计人数。
        /// </summary>
        public int custnum { get; set; }
        /// <summary>
        /// 已结单数。
        /// </summary>
        public int orderCount { get; set; }
        /// <summary>
        /// 已结金额。
        /// </summary>
        public decimal ssamount { get; set; }
        /// <summary>
        /// 总金额。
        /// </summary>
        public decimal totalAmount { get; set; }
    }
}