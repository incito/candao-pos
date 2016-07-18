namespace CanDao.Pos.Model.Request
{
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