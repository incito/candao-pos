using Models.Request;

namespace WebServiceReference.IService
{
    public interface IRestaurantService
    {

        /// <summary>
        /// 清机。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="userName">用户姓名。</param>
        /// <returns>清机成功返回null，否则返回错误信息。</returns>
        string Clearner(string userId, string userName);
 
    }
}