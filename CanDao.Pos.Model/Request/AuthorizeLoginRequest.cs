namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 授权登录请求类。
    /// </summary>
    public class AuthorizeLoginRequest
    {
        public AuthorizeLoginRequest(string name, string pwd, string macAddr, string rightCode)
        {
            username = name;
            password = pwd;
            macAddress = macAddr;
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

        /// <summary>
        /// 本机标识。
        /// </summary>
        public string macAddress { get; set; }
    }
}