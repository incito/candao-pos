namespace CanDao.Pos.Model.Response
{
    public class SyncDataResponse
    {
        public string code { get; set; }

        public string message { get; set; }

        public bool IsSuccess
        {
            get { return code.Equals("0000"); }
        }
    }
}