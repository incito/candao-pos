using System;
using Common;
using Models.Enum;
using Models.Request;
using Models.Response;
using WebServiceReference.IService;

namespace WebServiceReference.ServiceImpl
{
    public class AccountServiceImpl : IAccountService
    {
        public Tuple<string, string> Login(string userName, string password, EnumRightType rightType)
        {
            string addr = "http://" + RestClient.server + "/" + RestClient.apiPath + "/padinterface/login.json";
            var encodePwd = CEncoder.GenerateMD5Hash(password);
            var request = new AuthorizeLoginRequest(userName, encodePwd, GetRightCode(rightType));
            try
            {
                var result = HttpHelper.HttpPost<AuthorizeLoginResponse>(addr, request);
                return result.IsSuccess ? new Tuple<string, string>(null, result.fullname) : new Tuple<string, string>(result.msg, null);
            }
            catch (Exception ex)
            {
                //AllLog.Instance.E(ex);
                return new Tuple<string, string>(ex.Message, null);
            }
        }

        /// <summary>
        /// 获取授权码。
        /// </summary>
        /// <param name="rightType"></param>
        /// <returns></returns>
        private string GetRightCode(EnumRightType rightType)
        {
            return string.Format("03020{0}", (int)rightType);
        }

    }
}