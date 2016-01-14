using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common;
using Models;

namespace WebServiceReference
{
    public class WebServiceReference
    {
        private static string _report_title="";
        private static string _report_tele = "";
        private static string _report_address = "";
        private static string _report_web = "";
        private static bool _isprint_invoice = false;
        private static string _report_membertitle = "";
        private static string _candaomemberserver ="";
        private static string _reorderreason1 = "";
        private static string _reorderreason2 = "";
        private static string _reorderreason3 = "";
        private static string _reorderreason4 = "";
        private static string _reorderreason5 = "";
        private static int _paytype = 0;//收费模式 0 正常先吃饭再结帐 1 先付款再吃饭

        public static int Paytype
        {
            get { return getPayType(); }
            set { WebServiceReference._paytype = value; }
        }


        public static string Candaomemberserver
        {
            get
            {
                if (WebServiceReference._candaomemberserver.Equals(""))
                    WebServiceReference._candaomemberserver = WebServiceReference.getgetCandaomemberserver();
                return WebServiceReference._candaomemberserver;
            }
            set { WebServiceReference._candaomemberserver = value; }
        }

        public static string Report_membertitle
        {
            get
            {
                if (WebServiceReference._report_membertitle.Equals(""))
                    WebServiceReference._report_membertitle = WebServiceReference.getreport_membertitle();
                return WebServiceReference._report_membertitle;
            }
            set { WebServiceReference._report_membertitle = value; }
        }

        public static string Report_title
        {
            get { 
                if(WebServiceReference._report_title.Equals(""))
                    WebServiceReference._report_title=WebServiceReference.getReport_Title();
                return WebServiceReference._report_title;
                }
            set { WebServiceReference._report_title = value; }
        }
        public static string Report_tele
        {
            get
            {
                if (WebServiceReference._report_tele.Equals(""))
                    WebServiceReference._report_tele = WebServiceReference.getReport_Tele();
                return WebServiceReference._report_tele;
            }
            set { WebServiceReference._report_tele = value; }
        }
        public static string Report_address
        {
            get
            {
                if (WebServiceReference._report_address.Equals(""))
                    WebServiceReference._report_address = WebServiceReference.getReport_Address();
                return WebServiceReference._report_address;
            }
            set { WebServiceReference._report_address = value; }
        }
        public static string Report_web
        {
            get
            {
                if (WebServiceReference._report_web.Equals(""))
                    WebServiceReference._report_web = WebServiceReference.getReport_Web();
                return WebServiceReference._report_web;
            }
            set { WebServiceReference._report_web = value; }
        }
        private static string getnodeValue(string nodeName)
        {
            string ret = "";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(RestClient.ConfigFile);
                string xpath = "configuration/client/" + nodeName;
                XmlNode node = xml.SelectSingleNode(xpath);
                ret = node.Attributes["value"].Value;
            }
            catch { }
            return ret;
        }
        private static int getnodeIntValue(string nodeName)
        {
            int ret = 0;
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(RestClient.ConfigFile);
                string xpath = "configuration/client/" + nodeName;
                XmlNode node = xml.SelectSingleNode(xpath);
                ret = int.Parse( node.Attributes["value"].Value);
            }
            catch { }
            return ret;
        }
        public static bool isPrint_invoice
        {
            get { return getisprint_invoice().Equals("1"); }
            set { WebServiceReference._isprint_invoice = value; }
        }
        /// <summary>
        /// 报表标题第一行
        /// </summary>
        /// <returns></returns>
        private static string getReport_Title()
        {
            string ret = getnodeValue("report_title");
            return ret;
        }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
        private static string getReport_Tele()
        {
            string ret = getnodeValue("report_tele");
            return ret;
        }
        /// <summary>
        /// 地址
        /// </summary>
        /// <returns></returns>
        private static string getReport_Address()
        {
            string ret = getnodeValue("report_address");
            return ret;
        }
        /// <summary>
        /// 网址
        /// </summary>
        /// <returns></returns>
        private static string getReport_Web()
        {
            string ret = getnodeValue("report_web");
            return ret;
        }
        private static int getPayType()
        {
            int ret = getnodeIntValue("paytype");
            _paytype = ret;
            return ret;
        }
        private static string getisprint_invoice()
        {
            string ret = getnodeValue("isprint_invoice");
            return ret;
        }
        private static string getreport_membertitle()
        {
            string ret = getnodeValue("report_membertitle");
            return ret;
        }
        private static string getgetCandaomemberserver()
        {
            string ret = getnodeValue("CanDaoMemberServer");
            return ret;
        }
        public static string getreorderreason1()
        {
            string ret = getnodeValue("reorderreason1");
            return ret;
        }
        public static string getreorderreason2()
        {
            string ret = getnodeValue("reorderreason2");
            return ret;
        }
        public static string getreorderreason3()
        {
            string ret = getnodeValue("reorderreason3");
            return ret;
        }
        public static string getreorderreason4()
        {
            string ret = getnodeValue("reorderreason4");
            return ret;
        }
        public static string getreorderreason5()
        {
            string ret = getnodeValue("reorderreason5");
            return ret;
        }
    }
}
