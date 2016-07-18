namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 餐道会员登入登出请求类。
    /// </summary>
    public class CanDaoMemberLoginLogoutRequest
    {
        public string orderid { get; set; }

        public string mobile { get; set; }
    }
}