namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 保存支付方式请求类。
    /// </summary>
    public class SavePayWayInfoRequest
    {
        /// <summary>
        /// 支付方式ID。
        /// </summary>
        public int itemId { get; set; }
        /// <summary>
        /// 0隐藏/1显示。
        /// </summary>
        public int status { get; set; }
    }
}