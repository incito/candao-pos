using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common
{
    public class DataServerResultHelper
    {
        public static string GetDataServerReturnData(string jsonStrng)
        {
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonStrng);
            return ja["result"] != null ? ja["result"].ToString() : "";
        } 
    }
}