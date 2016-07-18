namespace CanDao.Pos.Model.Request
{
    public class SendVerifyCodeRequest
    {
        public string branch_id { get; set; }

        public string securityCode { get; set; }

        public string mobile { get; set; }
    }
}