namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 获取用户权限请求类。
    /// </summary>
    public class GetUserRightRequest
    {
        /// <summary>
        /// 用户账户。
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 获取用户权限时，密码可以为空。
        /// </summary>
        public string password { get; set; }
    }
}