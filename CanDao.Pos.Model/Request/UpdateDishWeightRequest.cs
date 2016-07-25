namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 菜品称重请求类。
    /// </summary>
    public class UpdateDishWeightRequest
    {
        public string tableNo { get; set; }

        public string dishid { get; set; }

        public string primarykey { get; set; }

        public string dishnum { get; set; }
    }
}