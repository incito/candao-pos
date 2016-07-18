namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 餐道会员接口请求类基类。
    /// </summary>
    public class CanDaoMemberBaseRequest
    {
        public CanDaoMemberBaseRequest()
        {
            securityCode = "";
        }

        public string branch_id { get; set; }

        public string securityCode { get; set; }
    }
}