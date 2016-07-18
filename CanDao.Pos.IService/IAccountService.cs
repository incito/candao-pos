using System;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.IService
{
    /// <summary>
    /// 用户管理服务接口。
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// 授权登录。
        /// </summary>
        /// <param name="userName">登录用户名。</param>
        /// <param name="password">登录密码。</param>
        /// <param name="rightType">授权类型。</param>
        /// <returns>Item1授权成功返回null，否则返回错误信息。Item2为账户全称。</returns>
        Tuple<string, string> Login(string userName, string password, EnumRightType rightType);

        /// <summary>
        /// 获取用户权限。
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Item1处理成功返回null，否则返回错误信息。Item2为用户权限。</returns>
        Tuple<string, UserRight> GetUserRight(string userName);

        /// <summary>
        /// 验证用户是否存在。
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Item1处理成功返回null，否则返回错误信息。Item2为用户是否存在。</returns>
        Tuple<string, bool> VerifyUser(string userId);
    }
}