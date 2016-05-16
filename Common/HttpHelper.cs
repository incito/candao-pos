using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms.VisualStyles;

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
            //return HttpOper<T>(uri, param, HttpType.Get);
            var json = HttpOper(uri, param, HttpType.Get);
            return json.FromJson<T>();
        }

        public static string HttpGet(string uri, Dictionary<string, string> param = null)
        {
            //return HttpOper(uri, param, HttpType.Get).ReadAsStringAsync().Result;
            return HttpOper(uri, param, HttpType.Get);
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
            var json = HttpOper(uri, data, HttpType.Post);
            return json.FromJson<T>();
        }

        /// <summary>
        /// Http Post请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HttpPost(string uri, object data)
        {
            //return HttpOper(uri, data, HttpType.Post).ReadAsStringAsync().Result;
            return HttpOper(uri, data, HttpType.Post);
        }

        /// <summary>
        /// Http Put请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        //public static T HttpPut<T>(string uri, object data)
        //{
        //    return HttpOper<T>(uri, data, HttpType.Put);
        //}

        /// <summary>
        /// Http Put请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        //public static string HttpPut(string uri, object data)
        //{
        //    return HttpOper(uri, data, HttpType.Put).ReadAsStringAsync().Result;
        //}

        /// <summary>
        /// Http Delete请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        //public static T HttpDelete<T>(string uri)
        //{
        //    return HttpOper<T>(uri, null, HttpType.Delete);
        //}

        /// <summary>
        /// Http Delete请求。返回JSON字符串。
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        //public static string HttpDelete(string uri, object data)
        //{
        //    return HttpOper(uri, data, HttpType.Delete).ReadAsStringAsync().Result;
        //}

        /// <summary>
        /// Http各项操作。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //private static T HttpOper<T>(string uri, object data, HttpType type)
        //{
        //    var content = HttpOper(uri, data, type);
        //    return content.ReadAsAsync<T>().Result;
        //}

        private static string HttpOper(string uri, object data, HttpType type)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (type == HttpType.Get && data != null)
            {
                if (!(data is Dictionary<string, string>))
                    throw new NotSupportedException("HttpGet param must be Dictionary<string, string>");

                var param = (Dictionary<string, string>)data;
                var keyValues = param.Keys.Select(t => string.Format("{0}={1}", t, param[t]));
                var appendUri = string.Format("?{0}", string.Join("&", keyValues));
                uri += appendUri;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = type.ToString("G").ToUpper();
            request.ContentType = "application/json; charset=utf-8";
            request.Timeout = 30 * 1000;
            if (type != HttpType.Get)
            {
                var dataString = data != null ? data.ToJson() : "";
                if (!uri.Contains("querytables.json"))
                    AllLog.Instance.I(string.Format("uri:{0}, type:{1}, request:{2}", uri, type, dataString));
                var dataArray = Encoding.UTF8.GetBytes(dataString);
                request.ContentLength = dataArray.Length;
                using (Stream sr = request.GetRequestStream())
                {
                    sr.Write(dataArray, 0, dataArray.Length);
                }
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader str = new StreamReader(stream, Encoding.UTF8))
                {
                    var result = str.ReadToEnd();
                    if (!uri.Contains("querytables.json"))
                        AllLog.Instance.I(string.Format("uri:{0}, type:{1}, result:{2}", uri, type, result));
                    return result;
                }
            }


            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response;
            //    switch (type)
            //    {
            //        case HttpType.Get:
            //            if (data != null)
            //            {
            //                if (!(data is Dictionary<string, string>))
            //                    throw new NotSupportedException("HttpGet param must be Dictionary<string, string>");

            //                var param = (Dictionary<string, string>)data;
            //                var keyValues = param.Keys.Select(t => string.Format("{0}={1}", t, param[t]));
            //                var appendUri = string.Format("?{0}", string.Join("&", keyValues));
            //                uri += appendUri;
            //            }

            //            response = client.GetAsync(uri).Result;
            //            break;
            //        case HttpType.Post:
            //            response = client.PostAsJsonAsync(uri, data).Result;
            //            break;
            //        case HttpType.Put:
            //            response = client.PutAsJsonAsync(uri, data).Result;
            //            break;
            //        case HttpType.Delete:
            //            response = client.DeleteAsync(uri).Result;
            //            break;
            //        default:
            //            throw new ArgumentOutOfRangeException("type", type, null);
            //    }

            //    if (response.IsSuccessStatusCode)
            //        return response.Content;
            //    throw new HttpRequestException(response.StatusCode.ToString());
            //}
        }

        private static string GetObjJsonString(object data)
        {
            if (data == null)
                return null;

            return data.ToJson();
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