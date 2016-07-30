namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 授权登录返回类型。
    /// </summary>
    public class AuthorizeLoginResponse : NewHttpBaseResponse
    {
        public LoginInfoResponse data { set; get; }

    }
}