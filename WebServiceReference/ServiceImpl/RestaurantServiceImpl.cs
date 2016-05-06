using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using CanDao.Pos.Model.Response;
using Common;
using Models;
using Models.Enum;
using Models.Request;
using Models.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebServiceReference.IService;

namespace WebServiceReference.ServiceImpl
{
    public class RestaurantServiceImpl : IRestaurantService
    {
        public Tuple<string, BranchInfo> GetBranchInfo()
        {
            try
            {
                var addr = string.Format("http://{0}/{1}/padinterface/getbranchinfo.json", RestClient.JavaServer, RestClient.ApiPath);
                var result = HttpHelper.HttpGet<GetBranchInfoResponse>(addr);
                if (result.IsSuccess)//这个接口1是成功，0是失败。
                    return new Tuple<string, BranchInfo>(result.msg ?? "获取分店信息失败。", null);

                var data = DataConverter.ToBranchInfo(result.data);
                return new Tuple<string, BranchInfo>(null, data);
            }
            catch (Exception ex)
            {
                return new Tuple<string, BranchInfo>(ex.Message, null);
            }
        }

        public string Clearner(string userId, string userName)
        {
            var paramList = new List<string>
            {
                userId,
                userName,
                RestClient.GetMacAddr(),
                RestClient.PosId,
                Globals.authorizer
            };
            var addr = string.Format("http://{0}/datasnap/rest/TServerMethods1/clearMachine/{1}", RestClient.DataServer, string.Join("//", paramList));

            try
            {
                var result = HttpHelper.HttpGet(addr);
                JObject ja = (JObject)JsonConvert.DeserializeObject(DataServerResultHelper.GetDataServerReturnData(result));
                if (!ja["Data"].ToString().Equals("1"))
                {
                    var msg = ja["Info"] != null ? ja["Info"].ToString() : "清机失败。";
                    return string.IsNullOrEmpty(msg) ? "清机失败。" : msg;
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
            var addr = string.Format("http://{0}/{1}/padinterface/findUncleanPosList.json", RestClient.JavaServer, RestClient.ApiPath);
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

        public Tuple<string, List<TableInfo>> GetAllTableInfoes()
        {
            try
            {
                var addr = string.Format("http://{0}/{1}/padinterface/querytables.json", RestClient.JavaServer, RestClient.ApiPath);
                var result = HttpHelper.HttpPost<List<TableInfoResponse>>(addr, null);
                var dataList = new List<TableInfo>();
                if (result != null && result.Any())
                    dataList = result.Select(DataConverter.ToTableInfo).ToList();
                return new Tuple<string, List<TableInfo>>(null, dataList);
            }
            catch (Exception ex)
            {
                //AllLog.Instance.E(ex);
                return new Tuple<string, List<TableInfo>>(ex.Message, null);
            }
        }

        public Tuple<string, bool> CheckWhetherTheLastEndWork()
        {
            try
            {
                var addr = string.Format("http://{0}/{1}/padinterface/isYesterdayEndWork.json", RestClient.JavaServer, RestClient.ApiPath);
                var result = HttpHelper.HttpGet<CheckWhetherEndWorkResponse>(addr);
                if (!result.IsSuccess)
                    return new Tuple<string, bool>(result.msg ?? "检测是否结业失败。", false);

                return new Tuple<string, bool>(null, result.detail);
            }
            catch (Exception ex)
            {
                return new Tuple<string, bool>(ex.Message, false);
            }
        }

        public Tuple<string, RestaurantTradeTime> GetRestaurantTradeTime()
        {
            try
            {
                var addr = string.Format("http://{0}/{1}/padinterface/getOpenEndTime.json", RestClient.JavaServer, RestClient.ApiPath);
                var response = HttpHelper.HttpGet<GetRestaurantTradeTimeResponse>(addr);
                if (!response.IsSuccess)
                    return new Tuple<string, RestaurantTradeTime>(response.info ?? "获取店铺营业时间失败。", null);

                var result = new RestaurantTradeTime(response.detail.begintime, response.detail.endtime);
                return new Tuple<string, RestaurantTradeTime>(null, result);
            }
            catch (Exception ex)
            {
                return new Tuple<string, RestaurantTradeTime>(ex.Message, null);
            }
        }

        public Tuple<string, DishSaleFullInfo> GetDishSaleInfo(EnumStatisticsPeriodsType periodsType)
        {
            try
            {
                var addr = string.Format("http://{0}/{1}/padinterface/getItemSellDetail.json", RestClient.JavaServer, RestClient.ApiPath);
                var request = new Dictionary<string, string> { { "flag", ((int)periodsType).ToString() } };
                var response = HttpHelper.HttpGet<GetDishSaleInfoResponse>(addr, request);
                if (!response.IsSuccess)
                    return new Tuple<string, DishSaleFullInfo>(response.msg ?? "获取品项销售明细失败。", null);

                return new Tuple<string, DishSaleFullInfo>(null, DataConverter.ToDishSaleFullInfo(response));
            }
            catch (Exception ex)
            {
                return new Tuple<string, DishSaleFullInfo>(ex.Message, null);
            }
        }

        public string BillingTip(string orderId, float tipAmount)
        {
            try
            {
                var addr = string.Format("http://{0}/{1}/tip/tipBilling.json", RestClient.JavaServer, RestClient.ApiPath);
                var request = new BillingTipRequest { orderid = orderId, paid = tipAmount };
                var response = HttpHelper.HttpPost<JavaResponse1>(addr, request);
                if (!response.IsSuccess)
                    return string.IsNullOrEmpty(response.msg) ? "小费结算失败。" : response.msg;
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Tuple<string, TipFullInfo> GetTipInfos(EnumStatisticsPeriodsType periodsType)
        {
            try
            {
                var addr = string.Format("http://{0}/{1}/tip/tipList.json", RestClient.JavaServer, RestClient.ApiPath);
                var request = new Dictionary<string, string> { { "flag", ((int)periodsType).ToString() } };
                var response = HttpHelper.HttpGet<GetTipInfoResponse>(addr, request);
                if (response == null)
                    return new Tuple<string, TipFullInfo>("获取小费统计信息失败。", null);

                if (!response.IsSuccess)
                {
                    var msg = !string.IsNullOrEmpty(response.msg) ? response.msg : "获取小费统计信息失败。";
                    AllLog.Instance.E(msg);
                    return new Tuple<string, TipFullInfo>(msg, null);
                }

                return new Tuple<string, TipFullInfo>(null, DataConverter.ToTipFullInfo(response));
            }
            catch (Exception ex)
            {
                return new Tuple<string, TipFullInfo>(ex.Message, null);
            }
        }
    }
}