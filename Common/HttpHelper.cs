using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Common
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
        /// <param name="uri"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T HttpGet<T>(string uri, Dictionary<string, string> param = null)
        {
            return HttpOper<T>(uri, param, HttpType.Get);
        }

        public static string HttpGet(string uri, Dictionary<string, string> param = null)
        {
            return HttpOper(uri, param, HttpType.Get).ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Http Post请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T HttpPost<T>(string uri, object data)
        {
            return HttpOper<T>(uri, data, HttpType.Post);
        }

        /// <summary>
        /// Http Post请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HttpPost(string uri, object data)
        {
            return HttpOper(uri, data, HttpType.Post).ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Http Put请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T HttpPut<T>(string uri, object data)
        {
            return HttpOper<T>(uri, data, HttpType.Put);
        }

        /// <summary>
        /// Http Put请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HttpPut(string uri, object data)
        {
            return HttpOper(uri, data, HttpType.Put).ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Http Delete请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static T HttpDelete<T>(string uri)
        {
            return HttpOper<T>(uri, null, HttpType.Delete);
        }

        /// <summary>
        /// Http Delete请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HttpDelete(string uri, object data)
        {
            return HttpOper(uri, data, HttpType.Delete).ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Http各项操作。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static T HttpOper<T>(string uri, object data, HttpType type)
        {
            var content = HttpOper(uri, data, type);
            return content.ReadAsAsync<T>().Result;
        }

        private static HttpContent HttpOper(string uri, object data, HttpType type)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

    /// <summary>
    /// Http类型
    /// </summary>
    public enum HttpType
    {
        Get,
        Post,
        Put,
        Delete
    }
}