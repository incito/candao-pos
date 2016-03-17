using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common
{
    public class DataServerResultHelper
    {
        public static string GetDataServerReturnData(string jsonStrng)
        {
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonStrng);
            var resultStr = ja["result"] != null ? ja["result"].ToString() : "";
            JArray jr = (JArray) JsonConvert.DeserializeObject(resultStr);
            return jr[0].ToString();
        } 
    }
}