namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 餐道会员查询请求类。
    /// </summary>
    public class CanDaoMemberQueryRequest : CanDaoMemberBaseRequest
    {
        public CanDaoMemberQueryRequest()
        {
            password = "";
        }

        public string cardno { get; set; }

        public string password { get; set; }
    }
}