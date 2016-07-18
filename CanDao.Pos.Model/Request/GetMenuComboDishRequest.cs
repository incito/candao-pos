namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 获取套餐包含菜品的请求类。
    /// </summary>
    public class GetMenuComboDishRequest
    {
        public string dishides { get; set; }

        public string menuid { get; set; }
    }
}