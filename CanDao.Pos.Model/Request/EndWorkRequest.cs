namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 结业请求类。
    /// </summary>
    public class EndWorkRequest
    {
        /// <summary>
        /// 用户ID。
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// IP地址。
        /// </summary>
        public string IpAddress { get; set; }
    }
}