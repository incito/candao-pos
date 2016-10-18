using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CanDao.Pos.Common
{
    /// <summary>
    /// Rest风格Http请求辅助类，Delephi接口。
    /// </summary>
    public class RestHttpHelper
    {
        /// <summary>
        /// Get请求。
        /// </summary>
        /// <typeparam name="T">返回的类型。</typeparam>
        /// <param name="uri">地址。</param>
        /// <param name="param">参数集合，注意顺序会影响拼接后的地址。</param>
        /// <returns></returns>
        public static Tuple<string, T> HttpGet<T>(string uri, List<string> param = null) where T : class , new()
        {
            return HttpOper<T>(uri, param, HttpType.Get);
        }

        /// <summary>
        /// Get请求。
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Tuple<string, string> HttpGet(string uri, List<string> param = null)
        {
            return HttpOper(uri, param, HttpType.Get);
        }

        /// <summary>
        /// Post请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Tuple<string, T> HttpPost<T>(string uri, object data) where T : class , new()
        {
            return HttpOper<T>(uri, data, HttpType.Post);
        }

        /// <summary>
        /// Post请求。
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Tuple<string, string> HttpPost(string uri, object data)
        {
            return HttpOper(uri, data, HttpType.Post);
        }

        /// <summary>
        /// Put请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Tuple<string, T> HttpPut<T>(string uri, object data) where T : class , new()
        {
            return HttpOper<T>(uri, data, HttpType.Put);
        }

        /// <summary>
        /// Put请求。
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Tuple<string, string> HttpPut(string uri, object data)
        {
            return HttpOper(uri, data, HttpType.Put);
        }

        /// <summary>
        /// Delete请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Tuple<string, T> HttpDelete<T>(string uri, object data) where T : class , new()
        {
            return HttpOper<T>(uri, data, HttpType.Delete);
        }

        /// <summary>
        /// Delete请求。
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Tuple<string, string> HttpDelete(string uri, object data)
        {
            return HttpOper(uri, data, HttpType.Delete);
        }

        private static Tuple<string, T> HttpOper<T>(string uri, object data, HttpType type) where T : class , new()
        {
            try
            {
                var result = HttpOper(uri, data, type);
                if (!string.IsNullOrEmpty(result.Item1))
                    return new Tuple<string, T>(result.Item1, default(T));

                var tag = ParseRestJson<T>(result.Item2);
                return new Tuple<string, T>(null, tag);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return new Tuple<string, T>(ex.MyMessage(), default(T));
            }
        }

        private static Tuple<string, string> HttpOper(string uri, object data, HttpType type)
        {
            try
            {
                if (string.IsNullOrEmpty(uri))
                    throw new ArgumentNullException("uri");

                string jsonString = string.Empty;
                switch (type)
                {
                    case HttpType.Get:
                        if (data != null)
                        {
                            if (!(data is List<string>))
                                throw new NotSupportedException("HttpGet param must be List<string>");

                            var param = (List<string>)data;
                            uri += string.Join("/", param);
                        }
                        if (!uri.EndsWith("/"))
                            uri += "/";
                        jsonString = HttpHelper.HttpGet(uri);
                        break;
                    case HttpType.Post:
                        jsonString = HttpHelper.HttpPost(uri, data);
                        break;
                    case HttpType.Put:
                        jsonString = HttpHelper.HttpPut(uri, data);
                        break;
                    case HttpType.Delete:
                        jsonString = HttpHelper.HttpDelete(uri, data);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("type", type, null);
                }
                if (string.IsNullOrEmpty(jsonString))
                    return new Tuple<string, string>("后台接口返回数据为空", null);

                return new Tuple<string, string>(null, jsonString);
            }
            catch (HttpRequestException ex)
            {
                HttpStatusCode statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), ex.MyMessage());
                string errMsg = GetHttpStatusCodeString(statusCode);
                return new Tuple<string, string>(errMsg, null);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return new Tuple<string, string>(ex.MyMessage(), null);
            }
        }

        private static string GetHttpStatusCodeString(HttpStatusCode statusCode)
        {
            string parseString = statusCode.ToString();
            switch (statusCode)
            {
                case HttpStatusCode.Continue:
                    parseString = "客户端可能继续其他请求。";
                    break;
                case HttpStatusCode.SwitchingProtocols:
                    parseString = "正在更改协议版本或协议。";
                    break;
                case HttpStatusCode.OK:
                    parseString = "请求成功。";
                    break;
                case HttpStatusCode.Created:
                    parseString = "请求导致在响应被发送前创建新资源。";
                    break;
                case HttpStatusCode.Accepted:
                    parseString = "请求已被接受做进一步处理。";
                    break;
                case HttpStatusCode.NonAuthoritativeInformation:
                    parseString = "返回的元信息来自缓存副本而不是原始服务器，因此可能不正确。";
                    break;
                case HttpStatusCode.NoContent:
                    parseString = "已成功处理请求并且响应已被设定为无内容。";
                    break;
                case HttpStatusCode.ResetContent:
                    parseString = "客户端应重置（或重新加载）当前资源。";
                    break;
                case HttpStatusCode.PartialContent:
                    parseString = "响应是包括字节范围的 GET 请求所请求的部分响应。";
                    break;
                case HttpStatusCode.MultipleChoices:
                    parseString = "请求的信息有多种表示形式。";
                    break;
                case HttpStatusCode.MovedPermanently:
                    parseString = "请求的信息已移到 Location 头中指定的 URI 处。";
                    break;
                case HttpStatusCode.Found:
                    parseString = "请求的信息位于 Location 头中指定的 URI 处。";
                    break;
                case HttpStatusCode.SeeOther:
                    parseString = "作为 POST 的结果，SeeOther 将客户端自动重定向到 Location 头中指定的 URI。用 GET 生成对 Location 标头所指定的资源的请求。";
                    break;
                case HttpStatusCode.NotModified:
                    parseString = "客户端的缓存副本是最新的。未传输此资源的内容。";
                    break;
                case HttpStatusCode.UseProxy:
                    parseString = "请求应使用位于 Location 头中指定的 URI 的代理服务器。";
                    break;
                case HttpStatusCode.Unused:
                    parseString = "未完全指定的 HTTP/1.1 规范的建议扩展。";
                    break;
                case HttpStatusCode.TemporaryRedirect:
                    parseString = "请求信息位于 Location 头中指定的 URI 处。";
                    break;
                case HttpStatusCode.BadRequest:
                    parseString = "服务器未能识别请求。";
                    break;
                case HttpStatusCode.Unauthorized:
                    parseString = "请求的资源要求身份验证。";
                    break;
                case HttpStatusCode.PaymentRequired:
                    parseString = "保留 PaymentRequired 以供将来使用。";
                    break;
                case HttpStatusCode.Forbidden:
                    parseString = "服务器拒绝满足请求。";
                    break;
                case HttpStatusCode.NotFound:
                    parseString = "请求的资源不在服务器上。";
                    break;
                case HttpStatusCode.MethodNotAllowed:
                    parseString = "请求的资源上不允许请求方法（POST 或 GET）。";
                    break;
                case HttpStatusCode.NotAcceptable:
                    parseString = "客户端已用 Accept 头指示将不接受资源的任何可用表示形式。";
                    break;
                case HttpStatusCode.ProxyAuthenticationRequired:
                    parseString = "请求的代理要求身份验证。";
                    break;
                case HttpStatusCode.RequestTimeout:
                    parseString = "客户端没有在服务器期望请求的时间内发送请求。";
                    break;
                case HttpStatusCode.Conflict:
                    parseString = "由于服务器上的冲突而未能执行请求。";
                    break;
                case HttpStatusCode.Gone:
                    parseString = "请求的资源不再可用。";
                    break;
                case HttpStatusCode.LengthRequired:
                    parseString = "缺少必需的 Content-length 头。";
                    break;
                case HttpStatusCode.PreconditionFailed:
                    parseString = "为此请求设置的条件失败，且无法执行此请求。";
                    break;
                case HttpStatusCode.RequestEntityTooLarge:
                    parseString = "请求太大，服务器无法处理。";
                    break;
                case HttpStatusCode.RequestUriTooLong:
                    parseString = "URI 太长。";
                    break;
                case HttpStatusCode.UnsupportedMediaType:
                    parseString = "请求是不支持的类型。";
                    break;
                case HttpStatusCode.RequestedRangeNotSatisfiable:
                    parseString = "无法返回从资源请求的数据范围，因为范围的开头在资源的开头之前，或因为范围的结尾在资源的结尾之后。";
                    break;
                case HttpStatusCode.ExpectationFailed:
                    parseString = "服务器未能符合 Expect 头中给定的预期值。";
                    break;
                case HttpStatusCode.InternalServerError:
                    parseString = "服务器上发生了一般错误。";
                    break;
                case HttpStatusCode.NotImplemented:
                    parseString = "服务器不支持请求的函数。";
                    break;
                case HttpStatusCode.BadGateway:
                    parseString = "中间代理服务器从另一代理或原始服务器接收到错误响应。";
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    parseString = "服务器暂时不可用，通常是由于过多加载或维护。";
                    break;
                case HttpStatusCode.GatewayTimeout:
                    parseString = "中间代理服务器在等待来自另一个代理或原始服务器的响应时已超时。";
                    break;
                case HttpStatusCode.HttpVersionNotSupported:
                    parseString = "服务器不支持请求的 HTTP 版本。";
                    break;
                default:
                    parseString = "其他未知错误。";
                    break;
            }
            return parseString;
        }

        private static T ParseRestJson<T>(string json) where T : class , new()
        {
            //var jObj = (JObject)JsonConvert.DeserializeObject(json);
            //var resultStr = jObj["result"] != null ? jObj["result"].ToString() : "";
            var headerLength = "{\"result\":[\"".Length;
            var resultStr = json.StartsWith("{\"result\":[\"") ? json.Substring(headerLength, json.Length - headerLength - 3) : json;
            if (string.IsNullOrEmpty(resultStr))
                return default(T);

            //var jArray = (JArray)JsonConvert.DeserializeObject(resultStr);
            //if (!jArray.Any())
            //    return default(T);

            var result = ParseJson2Object<T>(resultStr.FromUnicodeString());

            return result;
        }

        private static T ParseJson2Object<T>(string jsonString) where T : class , new()
        {
            var ppys = typeof(T).GetProperties();
            var jObj = (JObject)JsonConvert.DeserializeObject(jsonString);
            T item = default(T);
            foreach (var ppy in ppys)
            {
                var jToken = jObj[ppy.Name];
                if (jToken == null)
                    continue;

                if (ppy.PropertyType.IsValueType || ppy.PropertyType == typeof(string))
                {
                    if (item == default(T))
                        item = new T();
                    ppy.SetValue(item, GetValue(ppy.PropertyType, jToken.ToString()), null);
                }
                else if (ppy.PropertyType.IsClass)
                {
                    //var jsonStr = jToken.ToString().Replace("|", "\"");//有些从DataServer返回的Json做了替换处理
                    var jsonStr = jToken.ToString().Replace("&quot", "\"");
                    if (!IsValidData(jsonStr))
                        continue;

                    try
                    {
                        JavaScriptSerializer ds = new JavaScriptSerializer();
                        var data = ds.Deserialize(jsonStr, ppy.PropertyType);
                        if (item == default(T))
                            item = new T();
                        ppy.SetValue(item, data, null);
                    }
                    catch (Exception ex)
                    {
                        ErrLog.Instance.E(ex);
                    }
                }
            }
            return item;
        }

        private static bool IsValidData(string jsonStr)
        {
            try
            {
                var jObj = (JObject)JsonConvert.DeserializeObject(jsonStr);
                var str = jObj["Data"].ToString();
                return !string.IsNullOrEmpty(str) && !str.Equals("0") && !str.Equals("[]");
            }
            catch (Exception)
            {
                return true;
            }
        }

        private static object GetValue(Type type, string valueString)
        {
            if (type == typeof(int))
                return Convert.ToInt32(valueString);
            if (type == typeof(double) || type == typeof(float))
                return Convert.ToDouble(valueString);
            if (type == typeof(decimal))
                return Convert.ToDecimal(valueString);
            return valueString;
        }
    }

    /// <summary>
    /// Http类型
    /// </summary>
    internal enum HttpType
    {
        Get,
        Post,
        Put,
        Delete
    }

    /// <summary>
    /// Rest风格的返回类型，以result封装结果。
    /// </summary>
    internal class RestResponse
    {
        public List<string> result { get; set; }
    }
}