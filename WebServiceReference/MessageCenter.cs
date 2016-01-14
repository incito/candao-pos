using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common;
using Models;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace WebServiceReference
{
    public class MessageCenter
    {
        public static bool InsertTinvoice(string memberNo, string deviceId, string invoiceTitle, string orderid)
        {
            bool ret =false;
            string address = String.Format("http://{0}/" + RestClient.apiPath + "/padinterface/InsertTinvoice.json", RestClient.server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("memberNo");
            writer.WriteValue(memberNo);
            writer.WritePropertyName("deviceId");
            writer.WriteValue(deviceId);
            writer.WritePropertyName("invoiceTitle");
            writer.WriteValue(invoiceTitle);
            writer.WritePropertyName("orderid");
            writer.WriteValue(orderid);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = RestClient.Post_Rest(address, sw);
            JObject ja = null;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret = false; return ret; }
            ret = true;
            return ret;
        }
        public static bool findInvoiceByOrderid(string orderid,ref string invoice_title)
        {
            bool ret = false;
            string address = String.Format("http://{0}/" + RestClient.apiPath + "/padinterface/findInvoiceByOrderid.json", RestClient.server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("orderid");
            writer.WriteValue(orderid);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = RestClient.Post_Rest(address, sw);
            JObject ja = null;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret = false; return ret; }
            ret = ja["result"].ToString().Equals("0");
            //invoice_title = "上海餐道互联网金融信息服务有限公司";
            //return true;
            try
            {
                if (ret)
                {
                    JArray jr = (JArray)JsonConvert.DeserializeObject(ja["data"].ToString());
                    ja = (JObject)jr[0];
                }
                invoice_title = ja["invoice_title"].ToString();
            }
            catch { ret = false; return ret; }
            return ret;
        }
        public static bool updateInvoice(string orderid, decimal invoice_amount, string cardno)
        {
            bool ret = false;
            string address = String.Format("http://{0}/" + RestClient.apiPath + "/padinterface/updateInvoice.json", RestClient.server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("orderid");
            writer.WriteValue(orderid);
            writer.WritePropertyName("invoice_amount");
            writer.WriteValue(invoice_amount);
            writer.WritePropertyName("cardno");
            writer.WriteValue(cardno);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = RestClient.Post_Rest(address, sw);
            JObject ja = null;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret = false; return ret; }
            ret = ja["result"].ToString().Equals("0");
            return ret;
        }
    }
}
/*
 1.PAD开发票 
http://local/newspicyway/padinterface/InsertTinvoice.json
memberNo：会员号
deviceId：设备号
invoiceTitle：发票名称
orderid：订单号
{"memberNo":"123","deviceId":"123","invoiceTitle":"祁昆","orderid":"123"}


2.PAD根据会员号查询发票信息
http://local/newspicyway/padinterface/FindTinvoice.json
{"memberNo":"123"}

3.PAD根据订单号查询发票信息
http://local/newspicyway/padinterface/findInvoiceByOrderid.json
{"orderid":"123"}
{
result :"0",
data:{[
    "cardno":"",
	"invoice_title":"",
	"orderid":""
	]
  }
}

4. pos 打印发票后更新发票状态
http://local/newspicyway/padinterface/updateInvoice.json
invoice_amount 发票金额
orderid   订单号
{"orderid":"1213","invoice_amount":"123"}

*/