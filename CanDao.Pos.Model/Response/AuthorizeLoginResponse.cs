namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 授权登录返回类型。
    /// </summary>
    public class AuthorizeLoginResponse : NewHttpBaseResponse
    {
        /// <summary>
        /// 登录时间。
        /// </summary>
        public string loginTime { get; set; }

        /// <summary>
        /// 登录消息。
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 用户名称。非登录账户。
        /// </summary>
        public string fullname { get; set; }

    }
}