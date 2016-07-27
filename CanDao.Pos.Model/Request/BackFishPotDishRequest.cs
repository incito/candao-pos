namespace CanDao.Pos.Model.Request
{
    public class BackFishPotDishRequest : BackDishRequest
    {
        public string potdishid { get; set; }

        /// <summary>
        /// 退鱼锅的鱼时为0，锅底为1。
        /// </summary>
        public string hotflag { get; set; }
    }
}