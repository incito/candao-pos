namespace CanDao.Pos.Model.Request
{
    public class CanDaoMemberRegistRequest : CanDaoMemberModifyPasswordRequest
    {
        public CanDaoMemberRegistRequest()
        {
            channel = "0";
            tenant_id = "";
            updateuser = "";
            cardno = "";
        }

        public string channel { get; set; }

        public string tenant_id { get; set; }

        public string createuser { get; set; }

        public string updateuser { get; set; }
    }
}