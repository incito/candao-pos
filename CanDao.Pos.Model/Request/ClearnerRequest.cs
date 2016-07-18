namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 清机请求类。
    /// </summary>
    public class ClearnerRequest
    {
        /// <summary>
        /// 收银员编号。
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 收营员姓名。
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 机器标识。
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// Pos机器编号。
        /// </summary>
        public string PosId { get; set; }
        /// <summary>
        /// 授权人。
        /// </summary>
        public string Authorizer { get; set; }
    }
}