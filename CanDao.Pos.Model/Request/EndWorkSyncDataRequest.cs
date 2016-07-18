namespace CanDao.Pos.Model.Request
{
    public class EndWorkSyncDataRequest
    {
        public EndWorkSyncDataRequest()
        {
            synkey = "candaosynkey";
        }

        public string synkey { get; set; } 
    }
}