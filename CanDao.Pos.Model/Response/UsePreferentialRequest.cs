namespace CanDao.Pos.Model.Response
{
    public class UsePreferentialResponse : NewHttpBaseResponse
    {
        public PreferentialInfoResponse data { set; get; }
    }

    public class DelePreferentialResponse : NewHttpBaseResponse
    {
        public PreferentialInfo data { set; get; }
    }
    public class PreferentialInfo
    {
        public PreferentialInfoResponse preferentialInfo { set; get; }
    }
}
