namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 餐道会员修改密码请求类。
    /// </summary>
    public class CanDaoMemberModifyPasswordRequest
    {
        public CanDaoMemberModifyPasswordRequest()
        {
            securityCode = "";
            member_avatar = "";
        }

        public string branch_id { get; set; }

        public string cardno { get; set; }

        public string securityCode { get; set; }

        public string mobile { get; set; }

        public string password { get; set; }

        public string name { get; set; }

        public string gender { get; set; }

        public string birthday { get; set; }

        public string member_avatar { get; set; }
    }
}