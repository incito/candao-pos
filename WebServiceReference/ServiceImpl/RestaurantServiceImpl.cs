using System;
using System.Collections.Generic;
using Common;
using Models.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebServiceReference.IService;

namespace WebServiceReference.ServiceImpl
{
    public class RestaurantServiceImpl : IRestaurantService
    {
        public string Clearner(string userId, string userName)
        {
            var paramList = new List<string>
            {
                userId,
                userName,
                RestClient.GetMacAddr(),
                RestClient.getPosID(),
                Globals.authorizer
            };
            var addr = string.Format("http://{0}/datasnap/rest/TServerMethods1/clearMachine/{1}", RestClient.Server3, string.Join("//", paramList));

            try
            {
                var result = HttpHelper.HttpGet(addr);
                JObject ja = (JObject)JsonConvert.DeserializeObject(DataServerResultHelper.GetDataServerReturnData(result));
                if (!ja["Data"].ToString().Equals("1"))
                {
                    return ja["Info"] != null ? ja["Info"].ToString() : "清机失败。";
                }
                return null;
            }
            catch (Exception ex)
            {
                //AllLog.Instance.E(ex);
                return ex.Message;
            }
        }
    }
}