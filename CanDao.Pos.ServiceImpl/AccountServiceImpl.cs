using System;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;

namespace CanDao.Pos.ServiceImpl
{
    /// <summary>
    /// 用户管理服务实现。
    /// </summary>
    public class AccountServiceImpl : IAccountService
    {
        /// <summary>
        /// 授权登录。
        /// </summary>
        /// <param name="userName">登录用户名。</param>
        /// <param name="password">登录密码。</param>
        /// <param name="rightType">授权类型。</param>
        /// <returns>授权成功返回null，否则返回错误信息。</returns>
        public Tuple<string, string> Login(string userName, string password, EnumRightType rightType)
        {
            var addr = ServiceAddrCache.GetServiceAddr("AuthorizeLogin");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E("授权登录服务地址为空。");
                return new Tuple<string, string>("授权登录服务地址为空。", null);
            }

            try
            {
                var encodePwd = MD5Encrypt.Encrypt(password);
                var request = new AuthorizeLoginRequest(userName, encodePwd, MachineManage.GetMachineId(), GetRightCode(rightType));
                var result = HttpHelper.HttpPost<AuthorizeLoginResponse>(addr, request);
                if (!result.IsSuccess)
                    return new Tuple<string, string>(DataHelper.GetNoneNullValueByOrder(result.msg, "用户登录失败。"), null);
                return new Tuple<string, string>(null, result.data.fullname);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("用户登录时发生异常。", ex);
                return new Tuple<string, string>(ex.MyMessage(), null);
            }
        }

        /// <summary>
        /// 获取用户权限。
        /// </summary>
        /// <param name="userName">用户账户。</param>
        /// <returns>Item1是错误信息，正确时为null，Item2是用户权限。</returns>
        public Tuple<string, UserRight> GetUserRight(string userName)
        {
            string addr = ServiceAddrCache.GetServiceAddr("GetUserRight");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E("授权登录服务地址为空。");
                return new Tuple<string, UserRight>("授权登录服务地址为空。", null);
            }

            var request = new GetUserRightRequest { username = userName };
            try
            {
                var jsonResult = HttpHelper.HttpPost(addr, request);
                if (string.IsNullOrEmpty(jsonResult))
                    return new Tuple<string, UserRight>("获取用户权限失败。", null);

                var result = JsonHelper.GetValueFromJson(jsonResult, "result");
                if (string.IsNullOrEmpty(result))
                    return new Tuple<string, UserRight>("获取用户权限失败。", null);

                if (result.Equals("1"))
                {
                    string errMsg = JsonHelper.GetValueFromJson(jsonResult, "msg");
                    return new Tuple<string, UserRight>(DataHelper.GetNoneNullValueByOrder(errMsg, "获取用户权限失败。"), null);
                }

                var rightsJson = JsonHelper.GetValueFromJson(jsonResult, "rights");
                var userRight = ParseRightJson(rightsJson);
                return new Tuple<string, UserRight>(null, userRight);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return new Tuple<string, UserRight>(ex.MyMessage(), null);
            }
        }

        public Tuple<string, bool> VerifyUser(string userId)
        {
            string addr = ServiceAddrCache.GetServiceAddr("VerifyUser");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E("验证用户有效性地址为空。");
                return new Tuple<string, bool>("授权登录服务地址为空。", false);
            }

            var request = new VerifyUserRequest(userId);
            try
            {
                var result = HttpHelper.HttpPost<VerifyUserResponse>(addr, request);
                return new Tuple<string, bool>(result.msg, result.IsSuccess);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return new Tuple<string, bool>(ex.MyMessage(), false);
            }
        }

        /// <summary>
        /// 获取授权码。
        /// </summary>
        /// <param name="rightType"></param>
        /// <returns></returns>
        private string GetRightCode(EnumRightType rightType)
        {
            return string.Format("0{0}", (int)rightType);
        }

        /// <summary>
        /// 解析用户权限Json。因为Json包含数字，不能直接反序列化。
        /// </summary>
        /// <param name="resultJson">包含权限的Json。</param>
        /// <returns></returns>
        private UserRight ParseRightJson(string resultJson)
        {
            var right = new UserRight();
            var rightCode = GetRightCode(EnumRightType.Login);
            right.AllowLogin = JsonHelper.GetValueFromJson(resultJson, rightCode).Equals("1");

            rightCode = GetRightCode(EnumRightType.Opening);
            right.AllowOpening = JsonHelper.GetValueFromJson(resultJson, rightCode).Equals("1");

            rightCode = GetRightCode(EnumRightType.AntiSettlement);
            right.AllowAntiSettlement = JsonHelper.GetValueFromJson(resultJson, rightCode).Equals("1");

            rightCode = GetRightCode(EnumRightType.Cash);
            right.AllowCash = JsonHelper.GetValueFromJson(resultJson, rightCode).Equals("1");

            rightCode = GetRightCode(EnumRightType.Clearner);
            right.AllowClearn = JsonHelper.GetValueFromJson(resultJson, rightCode).Equals("1");

            rightCode = GetRightCode(EnumRightType.EndWork);
            right.AllowEndWork = JsonHelper.GetValueFromJson(resultJson, rightCode).Equals("1");

            return right;
        }
    }
}