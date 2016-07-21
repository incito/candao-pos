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
        /// <param name="useJsonHeaderFlag">是否使用Json标识Http头。</param>
        /// <returns></returns>
        public static T HttpGet<T>(string uri, Dictionary<string, string> param = null, bool useJsonHeaderFlag = false)
        {
            return HttpOper<T>(uri, param, HttpType.Get, useJsonHeaderFlag);
        }

        /// <summary>
        /// Http Get请求。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="param">参数。</param>
        /// <param name="useJsonHeaderFlag">是否使用Json标识Http头。</param>
        /// <returns></returns>
        public static string HttpGet(string uri, Dictionary<string, string> param = null, bool useJsonHeaderFlag = false)
        {
            return HttpOper(uri, param, HttpType.Get, useJsonHeaderFlag).ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Http Post请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">URL地址。</param>
        /// <param name="data">输入数据。</param>
        /// <param name="useJsonHeaderFlag">是否使用Json标识Http头。</param>
        /// <returns></returns>
        public static T HttpPost<T>(string uri, object data, bool useJsonHeaderFlag = false)
        {
            return HttpOper<T>(uri, data, HttpType.Post, useJsonHeaderFlag);
        }

        /// <summary>
        /// Http Post请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="data">输入数据。</param>
        /// <param name="useJsonHeaderFlag">是否使用Json标识Http头。</param>
        /// <returns></returns>
        public static string HttpPost(string uri, object data, bool useJsonHeaderFlag = false)
        {
            return HttpOper(uri, data, HttpType.Post, useJsonHeaderFlag).ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Http Put请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">URL地址。</param>
        /// <param name="data">输入数据。</param>
        /// <param name="useJsonHeaderFlag">是否使用Json标识Http头。</param>
        /// <returns></returns>
        public static T HttpPut<T>(string uri, object data, bool useJsonHeaderFlag = false)
        {
            return HttpOper<T>(uri, data, HttpType.Put, useJsonHeaderFlag);
        }

        /// <summary>
        /// Http Put请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="data">输入数据。</param>
        /// <param name="useJsonHeaderFlag">是否使用Json标识Http头。</param>
        /// <returns></returns>
        public static string HttpPut(string uri, object data, bool useJsonHeaderFlag = false)
        {
            return HttpOper(uri, data, HttpType.Put, useJsonHeaderFlag).ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Http Delete请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">URL地址。</param>
        /// <param name="useJsonHeaderFlag">是否使用Json标识Http头。</param>
        /// <returns></returns>
        public static T HttpDelete<T>(string uri, bool useJsonHeaderFlag = false)
        {
            return HttpOper<T>(uri, null, HttpType.Delete, useJsonHeaderFlag);
        }

        /// <summary>
        /// Http Delete请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri">URL地址。</param>
        /// <param name="data">输入数据。</param>
        /// <param name="useJsonHeaderFlag">是否使用Json标识Http头。</param>
        /// <returns></returns>
        public static string HttpDelete(string uri, object data, bool useJsonHeaderFlag = false)
        {
            return HttpOper(uri, data, HttpType.Delete, useJsonHeaderFlag).ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Http各项操作。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <param name="useJsonHeaderFlag">是否使用Json标识Http头。</param>
        /// <returns></returns>
        private static T HttpOper<T>(string uri, object data, HttpType type, bool useJsonHeaderFlag)
        {
            InfoLog.Instance.I("URL：{0}。Request ：{1}", uri, data.ToJson());
            var content = HttpOper(uri, data, type, useJsonHeaderFlag);
            InfoLog.Instance.I("URL：{0}。Result：{1}", uri, content.ReadAsStringAsync().Result);
            return content.ReadAsAsync<T>().Result;
        }

        private static HttpContent HttpOper(string uri, object data, HttpType type, bool useJsonHeaderFlag)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            using (var client = new HttpClient())
            {
                if (useJsonHeaderFlag)
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.Timeout = new TimeSpan(0, 0, 0, 10, 0);//超时设置为10秒。
                HttpResponseMessage response;
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