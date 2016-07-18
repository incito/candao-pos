using CanDao.Pos.Model.Response;

namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 餐道会员检测手机号是否重复的请求类。
    /// </summary>
    public class CanDaoMemberCheckMobileRepeatRequest : CanDaoMemberBaseRequest
    {
        public string mobile { get; set; }
    }
}