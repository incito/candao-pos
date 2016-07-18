namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 获取所有菜品信息的请求类。
    /// </summary>
    public class GetAllDishInfoRequest
    {
        /// <summary>
        /// 发起该请求的用户。
        /// </summary>
        public string UserName { get; set; }
 
    }
}