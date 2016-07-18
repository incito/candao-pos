namespace CanDao.Pos.Model
{
    /// <summary>
    /// 用户信息。
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户Id。登录用的是这个Id。
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户姓名。
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 账户密码。
        /// </summary>
        public string Password { get; set; }
    }
}