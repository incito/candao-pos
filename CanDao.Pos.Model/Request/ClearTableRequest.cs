namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 取消账单的请求类。
    /// </summary>
    public class ClearTableRequest
    {
        /// <summary>
        /// 订单号。
        /// </summary>
        public string orderNo { get; set; } 
    }
}