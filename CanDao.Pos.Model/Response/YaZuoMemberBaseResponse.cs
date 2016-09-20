namespace CanDao.Pos.Model.Response
{
    public class YaZuoMemberBaseResponse
    {
        public string Data { get; set; }

        public string Info { get; set; }

        public bool IsSuccess
        {
            get { return Data == "1"; }
        }
    }
}