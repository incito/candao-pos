using System;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common
{
    /// <summary>
    /// Json辅助类。
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为Json格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var output = serializer.Serialize(obj);
                return output;
            }
            catch (Exception ex)
            {
                //AllLog.Instance.E("Json 序列化失败，错误信息：{0}", ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// 将Json格式字符串反序列化为制定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string jsonStr)
        {
            try
            {
                JavaScriptSerializer ds = new JavaScriptSerializer();
                return (T)ds.Deserialize(jsonStr, typeof(T));
            }
            catch (Exception ex)
            {
                //AllLog.Instance.E("Json 反序列化失败，错误信息：{0}", ex.ToString());
                return default(T);
            }
        }

        /// <summary>
        /// 序列化成一个动态对象。
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JObject DeserializeJObject(this string json)
        {
            return (JObject)JsonConvert.DeserializeObject(json);
        }

        /// <summary>
        /// 从一个Json中提取指定名称的值。
        /// </summary>
        /// <param name="json">Json字符串。</param>
        /// <param name="key">指定的名称。</param>
        /// <returns></returns>
        public static string GetValueFromJson(string json, string key)
        {
            if (string.IsNullOrEmpty(json))
                return null;

            var jObj = DeserializeJObject(json);
            var jTokey = jObj[key];
            return jTokey == null ? null : jTokey.ToString();
        }
    }
}