using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using Common;
using Models;
using Models.Request;
using Models.Response;
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

        public Tuple<string, List<NoClearMachineInfo>> GetUnclearnPosInfo()
        {
            var addr = string.Format("http://{0}/{1}/padinterface/findUncleanPosList.json", RestClient.server, RestClient.apiPath);
            try
            {
                var result = HttpHelper.HttpGet<GetUnclearnPosInfoResponse>(addr);
                if (!result.result.Equals("0"))
                    return new Tuple<string, List<NoClearMachineInfo>>("服务器内部错误，获取未清机列表失败。", null);

                var list = result.detail.Select(DataConverter.ToNoClearMachineInfo).ToList();
                return new Tuple<string, List<NoClearMachineInfo>>(null, list);
            }
            catch (Exception ex)
            {
                return new Tuple<string, List<NoClearMachineInfo>>(ex.Message, null);
            }
        }
    }
}