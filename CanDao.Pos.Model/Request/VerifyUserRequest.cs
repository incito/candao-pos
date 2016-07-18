namespace CanDao.Pos.Model.Request
{
    public class VerifyUserRequest
    {
        public VerifyUserRequest(string userName)
        {
            username = userName;
            loginType = "030101";//这里是固定值。
        }

        public string username { get; set; }

        public string loginType { get; set; }
    }
}