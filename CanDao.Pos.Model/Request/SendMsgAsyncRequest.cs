namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 异步发送消息请求类。
    /// </summary>
    public class SendMsgAsyncRequest
    {
        /// <summary>
        /// 消息类型。
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 订单号。
        /// </summary>
        public string orderId { get; set; }
    }
}