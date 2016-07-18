namespace CanDao.Pos.Model.Response
{
    public class VerifyUserResponse
    {
        public string result { get; set; }

        public bool IsVerifySuccess
        {
            get { return !string.IsNullOrEmpty(result) && result.Equals("0"); }
        }
    }
}