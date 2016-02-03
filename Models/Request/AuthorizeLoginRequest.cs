namespace Models.Request
{
    /// <summary>
    /// 授权登录请求类。
    /// </summary>
    public class AuthorizeLoginRequest
    {
        public AuthorizeLoginRequest(string name, string pwd, string rightCode)
        {
            username = name;
            password = pwd;
            loginType = rightCode;
        }
        /// <summary>
        /// 账户名。
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 账户密码。
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 授权登录类型。
        /// </summary>
        public string loginType { get; set; }
    }
}