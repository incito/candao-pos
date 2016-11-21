namespace CanDao.Pos.Model.Request
{
    public class BackAllDishRequest
    {
        public BackAllDishRequest()
        {
            discardReason = "";
        }

        public string username { get; set; }
        public string actionType { get; set; }
        public string currenttableid { get; set; }
        public string discardReason { get; set; }
        public string discardUserId { get; set; }
        public string orderNo { get; set; }
    }
}