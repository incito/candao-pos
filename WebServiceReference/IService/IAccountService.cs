using System;
using Models.Enum;

namespace WebServiceReference.IService
{
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
    }
}