using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace Common
{
    public class PayType
    {
        public string payWay { get; set; }

        public float payAmount { get; set; }

        public string memerberCardNo { get; set; }
        public string bankCardNo { get; set; }
        public string couponnum { get; set; }
        public string couponid { get; set; }
        public string coupondetailid { get; set; }

    }
    // using System.Runtime.Serialization.Json; 

    /// <summary> 
    /// 解析JSON，仿Javascript风格 
    /// </summary> 
    public static class JSON
    {

        public static T parse<T>(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }

        public static string stringify(object jsonObject)
        {
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(jsonObject.GetType()).WriteObject(ms, jsonObject);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    } 
}
