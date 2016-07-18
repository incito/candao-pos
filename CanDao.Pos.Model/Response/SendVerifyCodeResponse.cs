namespace CanDao.Pos.Model.Response
{
    public class SendVerifyCodeResponse : CanDaoMemberBaseResponse
    {
        /// <summary>
        /// 验证码。
        /// </summary>
        public string valicode { get; set; }
    }
}