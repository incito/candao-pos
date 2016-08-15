using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CanDao.Pos.Common
{
    /// <summary>
    /// Http辅助类。
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// Http Get请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">URL地址。</param>
        /// <param name="param">参数。</param>
        /// <returns></returns>
        public static T HttpGet<T>(string uri, Dictionary<string, string> param = null)
        {
            return HttpOper<T>(uri, param, HttpType.Get);
        }

        /// <summary>
        /// Http Get请求。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="param">参数。</param>
        /// <returns></returns>
        public static string HttpGet(string uri, Dictionary<string, string> param = null)
        {
            var result = HttpOper(uri, param, HttpType.Get).ReadAsStringAsync().Result;
            HttpLog.Instance.D("URL：{0}。 Result：{1}", uri, result);
            return result;
        }

        /// <summary>
        /// Http Post请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">URL地址。</param>
        /// <param name="data">输入数据。</param>
        /// <param name="timeoutSecond">超时时间，默认30秒。</param>
        /// <returns></returns>
        public static T HttpPost<T>(string uri, object data, int timeoutSecond = 30)
        {
            return HttpOper<T>(uri, data, HttpType.Post, timeoutSecond);
        }

        /// <summary>
        /// Http Post请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="data">输入数据。</param>
        /// <param name="timeoutSecond">超时时间，默认30秒。</param>
        /// <returns></returns>
        public static string HttpPost(string uri, object data, int timeoutSecond = 30)
        {
            var result = HttpOper(uri, data, HttpType.Post, timeoutSecond).ReadAsStringAsync().Result;
            HttpLog.Instance.D("URL：{0}。 Result：{1}", uri, result);
            return result;
        }

        /// <summary>
        /// Http Put请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">URL地址。</param>
        /// <param name="data">输入数据。</param>
        /// <returns></returns>
        public static T HttpPut<T>(string uri, object data)
        {
            return HttpOper<T>(uri, data, HttpType.Put);
        }

        /// <summary>
        /// Http Put请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="data">输入数据。</param>
        /// <returns></returns>
        public static string HttpPut(string uri, object data)
        {
            var result = HttpOper(uri, data, HttpType.Put).ReadAsStringAsync().Result;
            HttpLog.Instance.D("URL：{0}。 Result：{1}", uri, result);
            return result;
        }

        /// <summary>
        /// Http Delete请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">URL地址。</param>
        /// <returns></returns>
        public static T HttpDelete<T>(string uri)
        {
            return HttpOper<T>(uri, null, HttpType.Delete);
        }

        /// <summary>
        /// Http Delete请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="data">输入数据。</param>
        /// <returns></returns>
        public static string HttpDelete(string uri, object data)
        {
            var result = HttpOper(uri, data, HttpType.Delete).ReadAsStringAsync().Result;
            HttpLog.Instance.D("URL：{0}。 Result：{1}", uri, result);
            return result;
        }

        /// <summary>
        /// Http各项操作。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <param name="timeoutSecond">超时时间，默认30秒。</param>
        /// <returns></returns>
        private static T HttpOper<T>(string uri, object data, HttpType type, int timeoutSecond = 30)
        {
            var content = HttpOper(uri, data, type, timeoutSecond);
            HttpLog.Instance.D("URL：{0}。 Result：{1}", uri, content.ReadAsStringAsync().Result);
            return content.ReadAsAsync<T>().Result;
        }

        /// <summary>
        /// Http请求执行类。
        /// </summary>
        /// <param name="uri">地址。</param>
        /// <param name="data">数据。</param>
        /// <param name="type">Http操作类型。</param>
        /// <param name="timeoutSecond">超时时间，默认30秒。</param>
        /// <returns></returns>
        private static HttpContent HttpOper(string uri, object data, HttpType type, int timeoutSecond = 30)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = new TimeSpan(0, 0, 0, timeoutSecond, 0);//超时设置。
                HttpResponseMessage response;
                HttpLog.Instance.D("URL：{0}。 Request ：{1}", uri, data.ToJson());
                switch (type)
                {
                    case HttpType.Get:
                        if (data != null)
                        {
                            if (!(data is Dictionary<string, string>))
                                throw new NotSupportedException("HttpGet param must be Dictionary<string, string>");

                            var param = (Dictionary<string, string>)data;
                            var keyValues = param.Keys.Select(t => string.Format("{0}={1}", t, param[t]));
                            var appendUri = string.Format("?{0}", string.Join("&", keyValues));
                            uri += appendUri;
                        }

                        response = client.GetAsync(uri).Result;
                        break;
                    case HttpType.Post:
                        response = client.PostAsJsonAsync(uri, data).Result;
                        break;
                    case HttpType.Put:
                        response = client.PutAsJsonAsync(uri, data).Result;
                        break;
                    case HttpType.Delete:
                        response = client.DeleteAsync(uri).Result;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("type", type, null);
                }

                if (response.IsSuccessStatusCode)
                    return response.Content;

                throw new HttpRequestException(response.StatusCode.ToString());
            }
        }
    }
}