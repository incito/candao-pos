using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Common;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace WebServiceReference
{
    public class RestClient
    {
        public static string server = "";
        public static string server2 = "";
        public static string Server3 = "";
        public static string apiPath = "";
        public static SerialClass sc;
        public static string portname = "";
        public enum ShowWindowCommands : int
        {

            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,    //用最近的大小和位置显示，激活
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_MAX = 10
        }
        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpszOp,
            string lpszFile,
            string lpszParams,
            string lpszDir,
            ShowWindowCommands FsShowCmd
            );
        public static string GetLocalIp()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            IPAddress ip = null;
            foreach (IPAddress ipa in ips)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = ipa;
                    break;
                }
            }
            string ipaddress = "";
            try
            { ipaddress = ip.ToString(); }
            catch { ipaddress = ""; }
            return ipaddress;
        }
        /// <summary>
        /// 获取MAC地址。
        /// </summary>
        /// <returns></returns>
        private static string GetMacAddr()
        {
            string mac = GetMacByWMI();
            if (string.IsNullOrEmpty(mac))
                mac = GetMacByNetworkInterface();
            return mac;
        }

        /// <summary>
        /// 获取MAC地址方法1。依赖WMI的系统服务，该服务一般不会被关闭；但如果系统服务缺失或者出现问题，该方法无法取得MAC地址。
        /// </summary>
        /// <returns></returns>
        private static string GetMacByWMI()
        {
            try
            {
                string macAddress = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var o in moc)
                {
                    var mo = (ManagementObject)o;
                    if ((bool)mo["IPEnabled"])
                        macAddress = mo["MacAddress"].ToString();
                    mo.Dispose();
                }
                return macAddress;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 获取MAC地址方法2。
        /// </summary>
        /// <returns></returns>
        private static string GetMacByNetworkInterface()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            if (interfaces.Length > 0)
            {
                return interfaces[0].GetPhysicalAddress().ToString();
            }
            return "00-00-00-00-00-00";
        }
        public static string ToUnicodeString(string str)
        {
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    strResult.Append("\\u");
                    strResult.Append(((int)str[i]).ToString("x"));
                }
            }
            return strResult.ToString();
        }
        public static string ConfigFile
        {
            get { return Application.StartupPath + @"\WebServiceReference.dll.config"; }
        }

        /// <summary>
        /// 从config文件获取WebService的连接地址
        /// </summary>
        /// <param name="endPointName">SOAP endPointName</param>
        /// <returns></returns>
        public static string GetSoapRemoteAddress()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(ConfigFile);
            string xpath = "configuration/client/Server";
            XmlNode node = xml.SelectSingleNode(xpath);
            server = node.Attributes["address"].Value;

            xpath = "configuration/client/Server2";
            node = xml.SelectSingleNode(xpath);
            server2 = node.Attributes["address"].Value;

            xpath = "configuration/client/Server3";
            node = xml.SelectSingleNode(xpath);
            Server3 = node.Attributes["address"].Value;

            apiPath = "newspicyway";
            try
            {
                xpath = "configuration/client/ApiPath";
                node = xml.SelectSingleNode(xpath);
                apiPath = node.Attributes["value"].Value;
            }
            catch { apiPath = "newspicyway"; }

            return server;

        }
        public static bool getShowReport()
        {
            string PrintDesign = "0";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/PrintDesign";
                XmlNode node = xml.SelectSingleNode(xpath);
                PrintDesign = node.Attributes["value"].Value;
            }
            catch { }
            return PrintDesign == "1";
        }
        public static int getMemberSystem()
        {
            int MemberSystem = 0;
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/MemberSystem";
                XmlNode node = xml.SelectSingleNode(xpath);
                MemberSystem = int.Parse(node.Attributes["value"].Value.ToString());
            }
            catch { }
            return MemberSystem;
        }
        public static string getPortName()
        {
            string _portname = "";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/PortName";
                XmlNode node = xml.SelectSingleNode(xpath);
                _portname = node.Attributes["value"].Value;
            }
            catch { _portname = ""; }
            return _portname;
        }
        public static string getbranch_id()
        {
            string _branch_id = "586313";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/branch_id";
                XmlNode node = xml.SelectSingleNode(xpath);
                _branch_id = node.Attributes["value"].Value;
            }
            catch { _branch_id = ""; }
            return _branch_id;
        }
        /// <summary>
        /// 返回第二扎半价ID号
        /// </summary>
        /// <returns></returns>
        public static string getYhID()
        {
            string ret = "f4d14744f95b4febb18385f7659199ee";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/yhid";
                XmlNode node = xml.SelectSingleNode(xpath);
                ret = node.Attributes["value"].Value;
            }
            catch { }
            return ret;
        }
        public static string getDoubleDishTicket()
        {
            string ret = "14027c4ed33a4bd19e9fd42f3cd09d7e";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/doubledishticket";
                XmlNode node = xml.SelectSingleNode(xpath);
                ret = node.Attributes["value"].Value;
            }
            catch { }
            return ret;
        }

        public static bool isRound()
        {
            bool ret = true; ;
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/Round";
                XmlNode node = xml.SelectSingleNode(xpath);
                ret = node.Attributes["value"].Value.Equals("1");
            }
            catch { }
            return ret;
        }

        public static string getTakeOutTable()
        {
            string TakeOutTable = "60";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/TakeOutTable";
                XmlNode node = xml.SelectSingleNode(xpath);
                TakeOutTable = node.Attributes["value"].Value;
            }
            catch { }
            return TakeOutTable;
        }
        public static bool isClearCoupon()
        {
            bool ret = true;
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/ClearCoupon";
                XmlNode node = xml.SelectSingleNode(xpath);
                ret = node.Attributes["value"].Value.Equals("1");
            }
            catch { }
            return ret;
        }
        public static string getTakeOutTableID()
        {
            string TakeOutTableID = "220";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/TakeOutTableID";
                XmlNode node = xml.SelectSingleNode(xpath);
                TakeOutTableID = node.Attributes["value"].Value;
            }
            catch { TakeOutTableID = "220"; }
            return TakeOutTableID;

        }
        public static string getRightCode(string rightid)
        {
            string PrintDesign = rightid;
            String rightdir = rightid.ToString();
            if (rightid.Length <= 1)
                rightdir = "03020" + rightid.ToString();//R
            return rightdir;
            /*try
            {
                String rightdir="R03020"+rightid.ToString();
                return rightdir;
                XmlDocument xml = new XmlDocument();
                xml.Load(RestClient.ConfigFile);
                string xpath = "configuration/client/"+rightdir;
                XmlNode node = xml.SelectSingleNode(xpath);
                PrintDesign = node.Attributes["value"].Value;
            }
            catch { }
            return PrintDesign;*/
        }
        public static int getAutoClose()
        {
            int AutoClose = 0;
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/AutoClose";
                XmlNode node = xml.SelectSingleNode(xpath);
                AutoClose = int.Parse(node.Attributes["value"].Value);
            }
            catch { }
            return AutoClose;
        }
        public static string getOpenCashIP()
        {
            string ip = "192.168.2.113";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/OpenCash";
                XmlNode node = xml.SelectSingleNode(xpath);
                ip = node.Attributes["value"].Value;
                if (ip.Trim().ToString().Length <= 0)
                    ip = "192.168.2.113";
            }
            catch { }
            return ip;
        }
        public static string getPosID()
        {
            string posid = "001";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/POSID";
                XmlNode node = xml.SelectSingleNode(xpath);
                posid = node.Attributes["value"].Value;
            }
            catch { }
            return posid;

        }
        public static string getManagerPwd()
        {
            string posid = "001";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/MANAGERPWD";
                XmlNode node = xml.SelectSingleNode(xpath);
                posid = node.Attributes["value"].Value;
            }
            catch { }
            return posid;

        }

        public static bool getPrintDesign()
        {
            string PrintDesign = "0";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ConfigFile);
                string xpath = "configuration/client/PrintDesign";
                XmlNode node = xml.SelectSingleNode(xpath);
                PrintDesign = node.Attributes["value"].Value;
            }
            catch { }
            return PrintDesign == "2";
        }

        public static string FromUnicodeString(string str)
        {
            //最直接的方法Regex.Unescape(str);
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        int charCode = Convert.ToInt32(strlist[i], 16);
                        strResult.Append((char)charCode);
                    }
                }
                catch (FormatException ex)
                {
                    return Regex.Unescape(str);
                }
            }
            return strResult.ToString();
        }
        private static string Request_Rest(string url)
        {
            HttpWebRequest request;
            HttpWebResponse response = null;
            StreamReader reader;
            StringBuilder sbSource;
            string address = url;
            if (address == null) { throw new ArgumentNullException("address"); }
            try
            {
                request = WebRequest.Create(address) as HttpWebRequest;
                request.UserAgent = ".NET Sample";
                request.Method = "GET";
                request.KeepAlive = false;
                request.Timeout = 15 * 1000;
                response = request.GetResponse() as HttpWebResponse;
                if (request.HaveResponse == true && response != null)
                {
                    reader = new StreamReader(response.GetResponseStream());
                    sbSource = new StringBuilder(reader.ReadToEnd());
                    string returnStr = FromUnicodeString(sbSource.ToString());
                    returnStr = returnStr.Replace("{\"result\":[\"", "");
                    returnStr = returnStr.Replace("\"]}", "");
                    return returnStr;
                    //return sbSource.ToString();
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        return errorResponse.StatusDescription;
                    }
                }
            }
            finally
            {
                if (response != null) { response.Close(); }
            }
            return "0";
        }
        private static string Request_Rest60(string url)
        {
            HttpWebRequest request;
            HttpWebResponse response = null;
            StreamReader reader;
            StringBuilder sbSource;
            string address = url;
            if (address == null) { throw new ArgumentNullException("address"); }
            try
            {
                request = WebRequest.Create(address) as HttpWebRequest;
                request.UserAgent = ".NET Sample";
                request.Method = "GET";
                request.KeepAlive = false;
                request.Timeout = 60 * 1000;
                response = request.GetResponse() as HttpWebResponse;
                if (request.HaveResponse == true && response != null)
                {
                    reader = new StreamReader(response.GetResponseStream());
                    sbSource = new StringBuilder(reader.ReadToEnd());
                    string returnStr = FromUnicodeString(sbSource.ToString());
                    returnStr = returnStr.Replace("{\"result\":[\"", "");
                    returnStr = returnStr.Replace("\"]}", "");
                    return returnStr;
                    //return sbSource.ToString();
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        return errorResponse.StatusDescription;
                    }
                }
            }
            finally
            {
                if (response != null) { response.Close(); }
            }
            return "0";
        }

        public static String Post_Rest(string url, StringWriter sw)
        {
            HttpWebRequest request;
            HttpWebResponse response = null;
            StreamReader reader;
            StringBuilder sbSource;
            string address = url;
            string ret = string.Empty;
            if (address == null) { throw new ArgumentNullException("address"); }
            try
            {
                request = WebRequest.Create(address) as HttpWebRequest;
                request.UserAgent = ".NET POS";
                request.Method = "POST";
                request.KeepAlive = false;
                //webReq.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json; charset=utf-8";
                //request.Timeout = 15 * 1000;
                if (sw != null)
                {
                    string jsonText = sw.GetStringBuilder().ToString();
                    //Console.WriteLine(jsonText);
                    byte[] byteArray = Encoding.UTF8.GetBytes(jsonText); //转化
                    request.ContentLength = byteArray.Length;
                    Stream newStream = request.GetRequestStream();
                    newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                    newStream.Close();
                }
                response = (HttpWebResponse)request.GetResponse();
                if (request.HaveResponse == true && response != null)
                {
                    reader = new StreamReader(response.GetResponseStream());
                    sbSource = new StringBuilder(reader.ReadToEnd());
                    string returnStr = FromUnicodeString(sbSource.ToString());
                    if (returnStr.Trim().ToString().Length <= 0)
                    {
                        returnStr = sbSource.ToString();
                    }
                    returnStr = returnStr.Replace("{\"result\":[\"", "");
                    returnStr = returnStr.Replace("\"]}", "");
                    return returnStr;
                    //return sbSource.ToString();
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        return errorResponse.StatusDescription;
                    }
                }
            }
            finally
            {
                if (response != null) { response.Close(); }
            }
            return "0";
        }


        private static byte[] Request_RestByte(string url)
        {
            HttpWebRequest request;
            HttpWebResponse response = null;
            StreamReader reader;
            StringBuilder sbSource;
            string address = url;
            if (address == null) { throw new ArgumentNullException("address"); }
            try
            {
                request = WebRequest.Create(address) as HttpWebRequest;
                request.UserAgent = ".NET Sample";
                request.Method = "GET";
                request.KeepAlive = false;
                request.Timeout = 15 * 1000;
                response = request.GetResponse() as HttpWebResponse;
                if (request.HaveResponse == true && response != null)
                {
                    reader = new StreamReader(response.GetResponseStream());
                    sbSource = new StringBuilder(reader.ReadToEnd());
                    string returnStr = FromUnicodeString(sbSource.ToString());
                    returnStr = returnStr.Replace("{\"result\":[\"", "");
                    returnStr = returnStr.Replace("\"]}", "");
                    return Encoding.Default.GetBytes(returnStr);
                    //return sbSource.ToString();
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        return null;
                    }
                }
            }
            finally
            {
                if (response != null) { response.Close(); }
            }
            return null;
        }
        /// <summary>
        ///登录
        /// </summary>
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// http://192.168.102.7/ladaotu/padinterface/login.json
        public static string Login(string userid, string password, string loginType)
        {
            string newloginType = getRightCode(loginType.ToString());
            string address = "http://" + server + "/" + apiPath + "/padinterface/login.json";
            StringWriter sw = new StringWriter();  //right1
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("username");
            writer.WriteValue(userid);
            writer.WritePropertyName("password");
            writer.WriteValue(password);
            writer.WritePropertyName("loginType");
            writer.WriteValue(newloginType);
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = sw.GetStringBuilder().ToString();
            //Msg.ShowError(jsonText);
            Console.WriteLine(jsonText);
            //return wmsRestClient.Request_Rest(address);
            String jsonResult = Post_Rest(address, sw);
            //Msg.ShowError(address);
            //Msg.ShowError(jsonResult);
            if (jsonResult == "0")
            {
                return "";
            }
            Globals.UserInfo.msg = jsonResult;
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            Globals.UserInfo.msg = "";
            //将反序列化的JSON字符串转换成对象  
            string result = ja["result"].ToString();
            //Msg.ShowError(result);
            string username = "";
            try
            {
                username = ja["fullname"].ToString();
            }
            catch { }
            Globals.UserInfo.msg = "";
            if (loginType.Equals("1"))
            {
                Globals.UserInfo.UserName = username;
                Globals.UserInfo.PassWord = password;
                Globals.UserInfo.UserID = userid;
            }
            else
            {
                Globals.authorizer = username;
            }
            try
            {
                Globals.UserInfo.msg = ja["msg"].ToString();
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 查询桌台
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetTableInfo(string TableName, string UserID)
        {
            string address = "http://" + Server3 + "/datasnap/rest/TServerMethods1/GetTableInfo";
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("ID");
            writer.WriteValue(1);
            writer.WritePropertyName("TableName");
            writer.WriteValue(TableName);
            writer.WritePropertyName("UserID");
            writer.WriteValue(UserID);
            writer.WriteEndObject();
            writer.Flush();

            string jsonText = sw.GetStringBuilder().ToString();
            Console.WriteLine(jsonText);

            //return wmsRestClient.Request_Rest(address);
            String jsonResult = Post_Rest(address, sw);
            if (jsonResult == "0")
            {
                return "";
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["UserID"].ToString();
            return result;
        }
        public static string GetServerTableInfo(string TableName, string UserID)
        {
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/GetServerTableInfo/{0}/{1} "
                                          , TableName
                                          , UserID
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return "";
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            if (!result.Equals("0"))
            {
                JArray jr = (JArray)JsonConvert.DeserializeObject(result);
                ja = (JObject)jr[0];//(JObject)JsonConvert.DeserializeObject(result);
                //将反序列化的JSON字符串转换成对象  
                Globals.CurrTableInfo.tableid = ja["tableid"].ToString();
                Globals.CurrTableInfo.tableName = ja["tableName"].ToString();
                Globals.CurrTableInfo.tableNo = ja["tableNo"].ToString();
                Globals.CurrTableInfo.tabletype = ja["tabletype"].ToString();
                Globals.CurrTableInfo.personNum = int.Parse(ja["personNum"].ToString());

                Globals.CurrOrderInfo.orderstatus = int.Parse(ja["orderstatus"].ToString());
                try
                {
                    Globals.CurrTableInfo.amount = float.Parse(ja["dueamount"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.begintime = DateTime.Parse(ja["begintime"].ToString());
                }
                catch
                { }
                Globals.CurrOrderInfo.orderid = ja["orderid"].ToString();
                try
                {
                    Globals.CurrOrderInfo.childNum = Int32.Parse(ja["childNum"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.womanNum = Int32.Parse(ja["womanNum"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.mannum = Int32.Parse(ja["mannum"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.custnum = Int32.Parse(ja["custnum"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.fulldiscountrate = float.Parse(ja["fulldiscountrate"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.memberno = ja["memberno"].ToString();
                }
                catch
                { }
                try
                {
                    Globals.CurrTableInfo.status = int.Parse(ja["status"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.userid = ja["userid"].ToString();
                }
                catch
                { }

            }
            return result;


        }
        public static string GetOrder(string TableName, string UserID)
        {
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/GetOrder/{0}/{1} "
                                          , TableName
                                          , UserID
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return "";
            }
            JObject jaAll = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = jaAll["Data"].ToString();
            if (!result.Equals("0"))
            {
                string tableinfojson = jaAll["OrderJson"].ToString();
                JObject ja = (JObject)JsonConvert.DeserializeObject(tableinfojson);
                result = ja["Data"].ToString();
                JArray jr = (JArray)JsonConvert.DeserializeObject(result);
                ja = (JObject)jr[0];//(JObject)JsonConvert.DeserializeObject(result);
                //将反序列化的JSON字符串转换成对象  
                Globals.CurrTableInfo.tableid = ja["tableid"].ToString();
                Globals.CurrTableInfo.tableName = ja["tableName"].ToString();
                Globals.CurrTableInfo.tableNo = ja["tableNo"].ToString();
                Globals.CurrTableInfo.tabletype = ja["tabletype"].ToString();
                Globals.CurrTableInfo.personNum = int.Parse(ja["personNum"].ToString());

                Globals.CurrOrderInfo.orderstatus = int.Parse(ja["orderstatus"].ToString());
                try
                {
                    Globals.CurrTableInfo.amount = float.Parse(ja["dueamount"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.begintime = DateTime.Parse(ja["begintime"].ToString());
                }
                catch
                { }
                Globals.CurrOrderInfo.orderid = ja["orderid"].ToString();
                try
                {
                    Globals.CurrOrderInfo.childNum = Int32.Parse(ja["childNum"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.womanNum = Int32.Parse(ja["womanNum"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.mannum = Int32.Parse(ja["mannum"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.custnum = Int32.Parse(ja["custnum"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.fulldiscountrate = float.Parse(ja["fulldiscountrate"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.memberno = ja["memberno"].ToString();
                }
                catch
                { }
                try
                {
                    Globals.CurrTableInfo.status = int.Parse(ja["status"].ToString());
                }
                catch
                { }
                try
                {
                    Globals.CurrOrderInfo.userid = ja["userid"].ToString();
                }
                catch
                { }
                //tablelist
                string tablelistjson = jaAll["JSJson"].ToString();
                //JObject jaList = (JObject)JsonConvert.DeserializeObject(tablelistjson);
                if (tablelistjson.Length > 30)
                {
                    DataTableConverter dtc = new DataTableConverter();
                    JsonReader jread = new JsonTextReader(new StringReader(tablelistjson));
                    DataTable dt = new DataTable();
                    dt.TableName = "tb_data";
                    dt.Clear();
                    dtc.ReadJson(jread, typeof(DataTable), dt, new JsonSerializer());
                    Globals.OrderTable.Clear();
                    Globals.OrderTable = null;
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        if (dr[0].ToString().Equals(""))
                        {
                            dt.Rows.RemoveAt(0);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        if (dr[0].ToString().Equals(""))
                        {
                            dt.Rows.RemoveAt(0);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[dt.Rows.Count - 1];
                        if (dr[0].ToString().Equals(""))
                        {
                            dt.Rows.RemoveAt(dt.Rows.Count - 1);
                        }
                    }
                    Globals.OrderTable = dt;
                }
            }
            return result;


        }

        /// <summary>
        /// 全单折扣
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="UserID"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        public static string fullDiscount(string OrderID, string UserID, double discount, string couponid, string partnername)
        {
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/fullDiscount/{0}/{1}/{2}/{3}/{4}/ "
                                          , OrderID
                                          , UserID
                                          , discount  //折扣率
                                          , couponid  //优惠编号
                                          , partnername  //优惠合作单位
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return "";
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            return result;


        }
        /// <summary>
        /// 使用鱼券
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="UserID"></param>
        /// <param name="cardno"></param>
        /// <param name="orderprice"></param>
        /// <returns></returns>
        public static string usefishcard(string OrderID, string UserID, string cardno, double orderprice)
        {
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/usefishcard/{0}/{1}/{2}/{3} "
                                          , OrderID
                                          , UserID
                                          , cardno
                                          , orderprice
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return "";
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            return result;


        }
        public static bool setMemberPrice(string UserID, string OrderID, string memberno)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/setMemberPrice/{0}/{1}/{2}/{3}/"
                                          , UserID
                                          , OrderID
                                          , ipaddress
                                          , memberno
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            return result == "1";


        }
        public static bool setMemberPrice2(string UserID, string OrderID)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/setMemberPrice2/{0}/{1}/{2}/"
                                          , UserID
                                          , OrderID
                                          , ipaddress
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            return result == "1";


        }
        public static bool cancelOrder(string UserID, string OrderID, string tableno)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/cancelOrder/{0}/{1}/{2}/"
                                          , UserID
                                          , OrderID
                                          , tableno
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            return result == "1";


        }

        public static bool setMemberPrice3(string UserID, string OrderID)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/setMemberPrice3/{0}/{1}/{2}/"
                                          , UserID
                                          , OrderID
                                          , ipaddress
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            return result == "1";


        }
        public static bool rebackorder(string UserID, string OrderID, ref String errStr)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/rebackorder/{0}/{1}/"
                                          , UserID
                                          , OrderID
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            errStr = ja["Info"].ToString();
            string result = ja["Data"].ToString();
            return result == "1";


        }
        public static bool accountsorder(string UserID, string OrderID, ref String errStr)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/accountsorder/{0}/{1}/"
                                          , UserID
                                          , OrderID
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            errStr = ja["Info"].ToString();
            string result = ja["Data"].ToString();
            return result == "1";


        }

        /// <summary>
        /// 获取帐单列表
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static string GetServerTableList(string OrderID, string UserID)
        {
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/GetServerTableList/{0}/{1} "
                                          , OrderID
                                          , UserID
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return "";
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            try
            { Globals.OrderTable.Clear(); }
            catch { }
            if (!result.Equals("0"))
            {
                ///JArray jr = (JArray)JsonConvert.DeserializeObject(result);
                //把JSON转为DataSet
                DataTableConverter dtc = new DataTableConverter();
                JsonReader jread = new JsonTextReader(new StringReader(result));
                DataTable dt = new DataTable();
                dt.TableName = "tb_data";
                dt.Clear();
                dtc.ReadJson(jread, typeof(DataTable), dt, new JsonSerializer());
                Globals.OrderTable.Clear();
                Globals.OrderTable = null;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    if (dr[0].ToString().Equals(""))
                    {
                        dt.Rows.RemoveAt(0);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[dt.Rows.Count - 1];
                    if (dr[0].ToString().Equals(""))
                    {
                        dt.Rows.RemoveAt(dt.Rows.Count - 1);
                    }
                }
                Globals.OrderTable = dt;
            }
            return result;


        }
        /// <summary>
        /// 错误后反结算
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public static bool posrebacksettleorder(string UserID, string OrderID)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/posrebacksettleorder/{0}/{1}/{2}/"
                                          , OrderID
                                          , UserID
                                          , ipaddress
                                          );
            String jsonResult = Request_Rest(address);
            if (jsonResult == "0")
            {
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            return result == "1";


        }
        /// <summary>
        /// 反结算
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static string rebacksettleorder(string OrderID, string UserID, string reason)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/rebacksettleorder.json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("userName");
            writer.WriteValue(UserID);
            writer.WritePropertyName("orderNo");
            writer.WriteValue(OrderID);
            writer.WritePropertyName("reason");
            writer.WriteValue(reason);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            if (jsonResult == "0")
            {
                return "";
            }
            string result = "1";
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString();
            }
            catch { }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }
        public static string debitamout(string OrderID)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/debitamout.json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("orderNo");
            writer.WriteValue(OrderID);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            if (jsonResult == "0")
            {
                return "";
            }
            string result = "1";
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString();
            }
            catch { }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }
        /// <summary>
        /// 修改称重数量
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static string updateDishWeight(string tableNo, string dishid, string primarykey, string dishnum)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/updateDishWeight.json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("tableNo");
            writer.WriteValue(tableNo);
            writer.WritePropertyName("dishid");
            writer.WriteValue(dishid);
            writer.WritePropertyName("primarykey");
            writer.WriteValue(primarykey);
            writer.WritePropertyName("dishnum");
            writer.WriteValue(dishnum);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            if (jsonResult == "0")
            {
                return "";
            }
            string result = "1";
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString();
            }
            catch { }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }

        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="UserID"></param>
        /// <param name="payDetail"></param>
        /// <returns></returns>
        public static string settleorder(string OrderID, string UserID, JArray payDetail)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/settleorder.json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("userName");
            writer.WriteValue(UserID);
            writer.WritePropertyName("orderNo");
            writer.WriteValue(OrderID);
            writer.WritePropertyName("payDetail");
            string paystr = payDetail.ToString();
            paystr = paystr.Replace("\r\n", "");
            paystr = paystr.Replace(@"\", "");
            writer.WriteValue(paystr);
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = sw.GetStringBuilder().ToString();
            jsonText = jsonText.Replace("\\", "");
            jsonText = jsonText.Replace(@"\", "");
            jsonText = jsonText.Replace("\"[", "[");
            jsonText = jsonText.Replace("]\"", "]");

            StringWriter sw2 = new StringWriter();
            sw2.Write(jsonText);

            Console.WriteLine(jsonText);
            //return wmsRestClient.Request_Rest(address);
            String jsonResult = Post_Rest(address, sw2);
            string result = "";
            if (jsonResult == "0")
            {
                return "";
            }
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString();
            }
            catch { result = "-1"; }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }

        public static string ChekDesk(string userid, string password)
        {
            string address = "http://" + server + "/" + apiPath + "/padinterface/login.json";
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("username");
            writer.WriteValue(userid);
            writer.WritePropertyName("password");
            writer.WriteValue(password);
            writer.WriteEndObject();
            writer.Flush();

            string jsonText = sw.GetStringBuilder().ToString();
            Console.WriteLine(jsonText);

            //return wmsRestClient.Request_Rest(address);
            String jsonResult = Post_Rest(address, sw);
            if (jsonResult == "0")
            {
                return "";
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["result"].ToString();
            string username = ja["fullname"].ToString();
            Globals.UserInfo.UserName = username;
            Globals.UserInfo.PassWord = password;
            Globals.UserInfo.UserID = userid;
            return result;
        }
        //获取数据库服务器时间
        public static DateTime PDA_GetServerTime()
        {
            //内向交货单 OrderType=1,收货单 OrderType=2 ...
            /* string address = String.Format("http://{0}:8080/datasnap/rest/TServerMethods1/PDA_GetServerTime/{1} "
                                           , myConst.ServerName
                                           , System.Net.Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString()
                                           );
             try
             {
                 string datestr = wmsRestClient.Request_Rest(address);
                 return DateTime.Parse(datestr);
             }
             catch
             {
                 return DateTime.Now;
             }*/
            return DateTime.Now;

        }

        public static bool DownloadFile(string URL, string filename)
        {

            try
            {

                HttpWebRequest Myrq = (HttpWebRequest)WebRequest.Create(URL);

                HttpWebResponse myrp = (HttpWebResponse)Myrq.GetResponse();

                Stream st = myrp.GetResponseStream();

                Stream so = new FileStream(filename, FileMode.Create);

                byte[] by = new byte[1024];

                int osize = st.Read(by, 0, (int)by.Length);

                while (osize > 0)
                {

                    so.Write(by, 0, osize);

                    osize = st.Read(by, 0, (int)by.Length);

                }

                so.Close();

                st.Close();

                myrp.Close();

                Myrq.Abort();

                return true;

            }

            catch (Exception e)
            {

                return false;

            }

        }
        //会员卡功能
        //memberinfo 会员卡号工k手机号
        public static JObject QueryBalance(string memberinfo)
        {
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/QueryBalance/{0}/", memberinfo);
            String jsonResult = Request_Rest60(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败,请检查内网是否正常...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            return ja;
        }
        public static bool VoidSale(string orderid, string pszPwd, string pszGPwd, out string info)
        {
            //orderid,pszPwd,pszGPwd
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/VoidSale/{0}/{1}/{2}/", orderid, pszPwd, pszGPwd);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            //将反序列化的JSON字符串转换成对象  
            info = ja["Info"].ToString();
            return result.Equals("1");
        }

        //memberinfo 会员卡号工k手机号
        public static JObject StoreCardDeposit(string memberinfo, double pszAmount, string pszSerial, int paytype)
        {
            int psTransType = 0;
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/StoreCardDeposit/{0}/{1}/{2}/{3}/{4}/{5}/", Globals.UserInfo.UserID, memberinfo, pszAmount, pszSerial, psTransType, paytype);
            String jsonResult = Request_Rest(address);
            //String jsonResult = "{\"Data\":\"1\",\"pszRestInfo\":\"\",\"pszRetcode\":\"\",\"pszRefnum\":\"\",\"pszTrace\":\"685646\",\"pszPan\":\"6201200131911018\",\"psStoreCardBalance\":\"120000\",\"psStoreCard2\":\"120000\"}";
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            return ja;
        }
        //激活卡
        public static JObject CardActive(string pszTrack2, string pszPwd, string pszMobile)
        {
            if (pszPwd.Trim().ToArray().Length <= 0)
                pszPwd = " ";
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/CardActive/{0}/{1}/{2}/", pszTrack2, pszPwd, pszMobile);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            return ja;
        }
        //会员卡消费
        public static JObject MemberSale(string aUserid, string orderid, string pszInput, string pszSerial, float pszCash, float pszPoint, int psTransType, float pszStore, string pszTicketList, string pszPwd, float memberyhqamount)
        {
            if (pszTicketList.Length <= 0)
                pszTicketList = "  ";

            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/Sale/{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/", aUserid, orderid, pszInput, pszSerial, pszCash, pszPoint, psTransType, pszStore, pszTicketList, pszPwd, memberyhqamount, server);
            String jsonResult = Request_Rest60(address);//会员结算的超时时间要多给点
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败，请检查内网网络...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            return ja;
        }



        public static bool OpenUp(string aUserID, string aUserPassword, int CallType, out string reinfo)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/OpenUp/{0}/{1}/{2}/{3}/", aUserID, aUserPassword, ipaddress, CallType);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败,请检查服务器是否开机,内网是否正常...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            string workdate = ja["workdate"].ToString();
            string Info = ja["Info"].ToString();
            Globals.workdate = workdate;

            reinfo = Info;
            //将反序列化的JSON字符串转换成对象  
            return result.Equals("1");
        }

        //
        //保存优惠内容
        public static bool saveOrderPreferential(string aUserid, string OrderID, string Preferential)
        {
            Preferential = Preferential.Replace(@"\", "");
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/saveOrderPreferential/{0}/{1}/{2}/{3}/", aUserid, ipaddress, OrderID, Preferential);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            return ja.Equals("1");
        }
        /// <summary>
        /// 清机
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static JObject clearMachine(string userid, string username, string authorizer)
        {
            string mac = GetMacAddr();
            string posid = getPosID();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/clearMachine/{0}/{1}/{2}/{3}/{4}/", userid, username, mac, posid, authorizer);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            return ja;
        }
        /// <summary>
        /// 结业
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static JObject endWork(string userid)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/endWork/{0}/{1}/", userid, ipaddress);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            return ja;
        }

        /// <summary>
        /// 零找金接口
        /// </summary>
        /// <param name="aUserID"></param>
        /// <param name="aUserPassword"></param>
        /// <param name="CallType"></param>
        /// <param name="reinfo"></param>
        /// <returns></returns>
        public static bool InputTellerCash(string aUserID, double cachamount, int CallType, out string reinfo)
        {
            string mac = GetMacAddr();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/InputTellerCash/{0}/{1}/{2}/{3}/", aUserID, mac, cachamount, CallType);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            string workdate = ja["workdate"].ToString();
            string Info = ja["Info"].ToString();
            Globals.workdate = workdate;

            reinfo = Info;
            //将反序列化的JSON字符串转换成对象  
            return result.Equals("1");
        }
        public static JArray querytables()
        {
            JArray jr = null;

            string address = "http://" + server + "/" + apiPath + "/padinterface/querytables.json";
            String jsonResult = Post_Rest(address, null);
            if (jsonResult == "0")
            {
                return jr;
            }
            jr = (JArray)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            //string result = jr["result"].ToString();
            return jr;
        }
        /// <summary>
        /// jde同步资料回调
        /// </summary>
        public static bool jdesyndata()
        {
            string address = "http://" + server + "/" + apiPath + "/padinterface/jdesyndata.json";
            //{"synkey":"candaosynkey"}
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("synkey");
            writer.WriteValue("candaosynkey");
            writer.WriteEndObject();
            writer.Flush();
            string jsonResult = Post_Rest(address, sw);
            if (jsonResult == "0")
                return false;

            var jr = (JObject)JsonConvert.DeserializeObject(jsonResult);
            return jr["result"].ToString().Equals("0");
        }
        /// <summary>
        /// 按类别获取优惠列表
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public static JArray getcoupon_rule(int typeid)
        {
            if (typeid < 90)
                typeid = 90;
            JArray jr = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getcoupon_rule/{0}/", typeid);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            jr = (JArray)JsonConvert.DeserializeObject(result);
            //将反序列化的JSON字符串转换成对象  
            //string result = jr["result"].ToString();
            return jr;
        }
        /// <summary>
        /// 广播UDP消息
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public static void broadcastmsg(int msgid, string msg)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/broadcastmsg/{0}/{1}/{2}", Globals.UserInfo.UserID, msgid, msg);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
        }
        public static void putOrder(string tableno, string orderid, TGzInfo gzinfo)
        {
            if (gzinfo.Gzcode == null)
                gzinfo.Gzcode = "0";
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/putOrder/{0}/{1}/{2}/{3}/{4}/{5}/", tableno, orderid, gzinfo.Gzcode, gzinfo.Gzname, gzinfo.Telephone, gzinfo.Relaperson);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
        }
        public static bool getOrderSequence(string tableno, out string sequence)
        {
            bool ret = false;
            sequence = "";
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getOrderSequence/{0}/", tableno);
            String jsonResult = Request_Rest(address);
            try
            {
                if (jsonResult.Equals("0"))
                {
                }
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                //将反序列化的JSON字符串转换成对象  
                string result = ja["Data"].ToString();
                ret = result.Equals("1");
                if (ret)
                {
                    sequence = ja["Info"].ToString();
                }
            }
            catch { }
            return ret;
        }

        public static void wmOrder(string orderid)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/wmOrder/{0}/", orderid);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
            }
        }


        /// <summary>
        /// 按类别获取优惠列表 第二版 POST接口
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public static JArray getcoupon_rulev2(string typeid, string orderid)
        {
            JArray jr = null;
            if (string.IsNullOrEmpty(orderid))
                orderid = "000000";

            string ipaddress = GetLocalIp(); //newspicyway/padinterface/getPreferentialList.json
            //string address = String.Format("http://" + server + "/newspicyway/padinterface/getPreferentialList.json?typeid={0}",typeid);
            string address = "http://" + server + "/" + apiPath + "/padinterface/getPreferentialList.json";

            StringWriter sw = new StringWriter();  //right1
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("machineno");
            writer.WriteValue(ipaddress);
            writer.WritePropertyName("userid");
            writer.WriteValue(Globals.UserInfo.UserID);
            writer.WritePropertyName("orderid");//有可能为空
            writer.WriteValue(orderid);
            writer.WritePropertyName("typeid");
            writer.WriteValue(typeid);
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = sw.GetStringBuilder().ToString();
            //Console.WriteLine(jsonText);
            String jsonResult = Post_Rest(address, sw);
            //String jsonResult = RestClient.Post_Rest(address, null);
            //String jsonResult = RestClient.Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
            //JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = jsonResult;// ja["Data"].ToString();
            try
            {
                jr = (JArray)JsonConvert.DeserializeObject(result);
            }
            catch { }
            //将反序列化的JSON字符串转换成对象  
            //string result = jr["result"].ToString();
            return jr;
        }

        /// <summary>
        /// 使用优惠返回一个金额
        /// </summary>
        /// <param name="preferentialid"></param>
        /// <param name="disrate"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static bool usePreferentialItem(string preferentialid, float disrate, string orderid, string type, string sub_type, ref string msg, ref float amount, float preferentialAmt)
        {
            JArray jr = null;
            string ipaddress = GetLocalIp();
            string address = "http://" + server + "/" + apiPath + "/padinterface/usePreferentialItem.json";
            StringWriter sw = new StringWriter();  //right1
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("machineno");
            writer.WriteValue(ipaddress);
            writer.WritePropertyName("userid");
            writer.WriteValue(Globals.UserInfo.UserID);
            writer.WritePropertyName("orderid");//有可能为空
            writer.WriteValue(orderid);
            writer.WritePropertyName("preferentialid");
            writer.WriteValue(preferentialid);
            writer.WritePropertyName("disrate");
            writer.WriteValue(disrate.ToString());
            writer.WritePropertyName("type");
            writer.WriteValue(type);
            writer.WritePropertyName("sub_type");
            writer.WriteValue(sub_type);
            //需要增加一个参数 PreferentialAmt记录所有已选的挂帐和优免金额 preferentialAmt 传给后台计算的时候去掉优惠
            writer.WritePropertyName("preferentialAmt");
            writer.WriteValue(preferentialAmt.ToString());
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = sw.GetStringBuilder().ToString();
            //Console.WriteLine(jsonText);
            String jsonResult = Post_Rest(address, sw);
            //String jsonResult = RestClient.Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                msg = "连接服务器失败...";
                return false;

            }
            JObject ja = null; //(JObject)JsonConvert.DeserializeObject(jsonResult);
            try
            { ja = (JObject)JsonConvert.DeserializeObject(jsonResult); }
            catch { amount = 0; msg = jsonResult; return false; }
            //JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            bool result = ja["result"].ToString().Equals("1");
            msg = ja["msg"].ToString();
            amount = float.Parse(ja["amount"].ToString());
            return result;
        }
        /// <summary>
        /// 撤销帐单优惠
        /// </summary>
        /// <param name="preferentialid"></param>
        /// <param name="disrate"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static bool cancelPreferentialItem(string preferentialid, string orderid, ref string msg)
        {
            JArray jr = null;
            string ipaddress = GetLocalIp();
            string address = "http://" + server + "/" + apiPath + "/padinterface/cancelPreferentialItem.json";
            StringWriter sw = new StringWriter();  //right1
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("machineno");
            writer.WriteValue(ipaddress);
            writer.WritePropertyName("userid");
            writer.WriteValue(Globals.UserInfo.UserID);
            writer.WritePropertyName("orderid");//有可能为空
            writer.WriteValue(orderid);
            writer.WritePropertyName("preferentialid");
            writer.WriteValue(preferentialid);
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = sw.GetStringBuilder().ToString();
            //Console.WriteLine(jsonText);
            String jsonResult = Post_Rest(address, sw);
            //String jsonResult = RestClient.Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            bool result = ja["result"].ToString().Equals("1");
            msg = ja["msg"].ToString();
            return result;
        }
        /// <summary>
        /// 获取帐单内容，用于打印
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public static bool getOrderInfo(string aUserid, string orderid, int printtype, out JArray jrorder, out JArray jrlist, out JArray jrjs)
        {
            String OrderJson = "";
            String ListJson = "";
            String JSJson = "";
            JArray jrOrder = null;
            JArray jrList = null;
            JArray jrJS = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getOrderInfo/{0}/{1}/{2}/", aUserid, orderid, printtype);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                jrorder = jrOrder;
                jrjs = jrJS;
                jrlist = jrList;
                return false;
                //throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            bool ret = result.Equals("1");
            if (result == "1")
            {
                OrderJson = ja["OrderJson"].ToString();
                OrderJson = OrderJson.Replace("|", "\"");
                ListJson = ja["ListJson"].ToString();
                ListJson = ListJson.Replace("|", "\"");
                ListJson = ListJson.Replace(@"\", @"\\");
                JSJson = ja["JSJson"].ToString();
                JSJson = JSJson.Replace("&quot", "\"");
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(OrderJson);
                    result = ja["Data"].ToString();
                    jrOrder = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(ListJson);
                    result = ja["Data"].ToString();
                    jrList = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(JSJson);
                    result = ja["Data"].ToString();
                    jrJS = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
            }
            jrorder = jrOrder;
            jrjs = jrJS;
            jrlist = jrList;
            //将反序列化的JSON字符串转换成对象  
            //string result = jr["result"].ToString();
            return ret;
        }
        /// <summary>
        /// 获取清单单内容
        /// </summary>
        /// <param name="aUserid"></param>
        /// <param name="jsorder"></param>
        /// <param name="jrorder"></param>
        /// <param name="jrjs"></param>
        /// <returns></returns>
        public static bool getClearMachineData(string aUserid, string jsorder, out JArray jrorder, out JArray jrjs)
        {
            String OrderJson = "";
            String JSJson = "";
            JArray jrOrder = null;
            JArray jrJS = null;
            String posid = getPosID();
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getClearMachineData/{0}/{1}/{2}/", aUserid, jsorder, posid);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                jrorder = jrOrder;
                jrjs = jrJS;
                return false;
                //throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            bool ret = result.Equals("1");
            if (result == "1")
            {
                OrderJson = ja["OrderJson"].ToString();
                OrderJson = OrderJson.Replace("|", "\"");
                JSJson = ja["JSJson"].ToString();
                JSJson = JSJson.Replace("|", "\"");
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(OrderJson);
                    result = ja["Data"].ToString();
                    jrOrder = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(JSJson);
                    result = ja["Data"].ToString();
                    jrJS = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
            }
            jrorder = jrOrder;
            jrjs = jrJS;
            return ret;
        }

        public static bool caleTableAmount(string aUserid, string orderid)
        {
            String OrderJson = ""; //orderid,userid
            String JSJson = "";
            JArray jrOrder = null;
            JArray jrJS = null;
            String posid = getPosID();
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/caleTableAmount/{0}/{1}/", orderid, aUserid);
            String jsonResult = Request_Rest60(address);
            //将反序列化的JSON字符串转换成对象  
            string result = jsonResult;
            bool ret = result.Equals("1");
            return ret;
        }
        public static void openCashCom()
        {
            portname = "COM" + getPortName();
            if (portname.Trim().ToString().Length > 0)
            {
                sc = new SerialClass(portname);
                try
                {
                    sc.openPort();
                }
                catch { }
                finally
                {
                }
            }
        }
        public static bool OpenCash()
        {
            String jsonResult = "";
            bool ret = false;
            portname = getPortName();
            if (portname.Trim().ToString().Length <= 0)
            {
                try
                {
                    string ip = getOpenCashIP();
                    string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/OpenCash/{0}/", ip);
                    jsonResult = Request_Rest(address);
                    //将反序列化的JSON字符串转换成对象  
                    string result = jsonResult;
                    ret = result.Equals("1");
                }
                catch { }
            }
            try
            {
                //开启IBM钱箱
                string filename = Application.StartupPath + "\\Cash.exe";
                if (File.Exists(filename))
                {
                    ShellExecute(IntPtr.Zero, "open", filename, null, null, ShowWindowCommands.SW_SHOWNORMAL);
                }
                else
                {
                    //IBM串口钱箱
                    if (portname.Trim().ToString().Length > 0)
                    {
                        //SerialClass sc = new SerialClass(portname);
                        //sc.DataReceived += new SerialClass.SerialPortDataReceiveEventArgs(sc_DataReceived);
                        try
                        {
                            //sc.openPort();
                            byte[] bytes = new byte[1];
                            bytes[0] = 0x07;
                            sc.SendData(bytes, 0, 1);
                        }
                        catch { }
                        finally
                        {
                            //sc.closePort();
                        }
                    }
                }
            }
            catch { }
            return ret;
        }

        /// <summary>
        /// 返回当天全部帐单
        /// </summary>
        /// <param name="aUserid"></param>
        /// <param name="jrorder"></param>
        /// <returns></returns>
        public static bool getAllOrderInfo2(string aUserid, out JArray jrorder)
        {
            String OrderJson = "";
            JArray jrOrder = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getAllOrderInfo2/{0}/", aUserid);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                jrorder = jrOrder;
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            bool ret = result.Equals("1");
            if (result == "1")
            {
                OrderJson = ja["OrderJson"].ToString();
                OrderJson = OrderJson.Replace("|", "\"");
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(OrderJson);
                    result = ja["Data"].ToString();
                    jrOrder = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
            }
            jrorder = jrOrder;
            return ret;
        }
        public static int getFoodStatus(string dishid, string dishunit)
        {
            string ipaddress = GetLocalIp();
            int result = 0;
            try
            {
                string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getFoodStatus/{0}/{1}/", dishid, dishunit);
                String jsonResult = Request_Rest(address);
                if (jsonResult.Equals("0"))
                {
                    return 0;
                }
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                //将反序列化的JSON字符串转换成对象  
                result = int.Parse(ja["Info"].ToString());
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 获取全部的挂帐单位
        /// </summary>
        /// <param name="aUserid"></param>
        /// <param name="gzData"></param>
        /// <returns></returns>
        public static bool getAllGZDW(string aUserid, out JArray gzData)
        {
            String OrderJson = "";
            JArray jrOrder = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getAllGZDW/{0}/", aUserid);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                gzData = jrOrder;
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            bool ret = result.Equals("1");
            if (result == "1")
            {
                OrderJson = ja["OrderJson"].ToString();
                OrderJson = OrderJson.Replace("|", "\"");
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(OrderJson);
                    result = ja["Data"].ToString();
                    jrOrder = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
            }
            gzData = jrOrder;
            return ret;
        }

        /// <summary>
        /// 获取全部可外卖菜品
        /// </summary>
        /// <param name="aUserid"></param>
        /// <param name="wmData"></param>
        /// <returns></returns>
        public static bool getAllWmFood(string aUserid, out JArray wmData)
        {
            String OrderJson = "";
            JArray jrOrder = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getAllWmFood/{0}/", aUserid);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                wmData = jrOrder;
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            bool ret = result.Equals("1");
            if (result == "1")
            {
                OrderJson = ja["OrderJson"].ToString();
                OrderJson = OrderJson.Replace("|", "\"");
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(OrderJson);
                    result = ja["Data"].ToString();
                    jrOrder = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
            }
            wmData = jrOrder;
            return ret;
        }
        /// <summary>
        /// 餐具
        /// </summary>
        /// <param name="aUserid"></param>
        /// <param name="wmData"></param>
        /// <returns></returns>
        public static bool getCJFood(string aUserid, out JArray cjData)
        {
            String OrderJson = "";
            JArray jrOrder = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getCJFood/{0}/", aUserid);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                cjData = jrOrder;
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            bool ret = result.Equals("1");
            if (result == "1")
            {
                OrderJson = ja["OrderJson"].ToString();
                OrderJson = OrderJson.Replace("|", "\"");
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(OrderJson);
                    result = ja["Data"].ToString();
                    jrOrder = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
            }
            cjData = jrOrder;
            return ret;
        }

        public static bool getGroupDetail(string dishid, out JArray groupData)
        {
            String OrderJson = "";
            JArray jrOrder = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getGroupDetail/{0}/", dishid);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                groupData = jrOrder;
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            bool ret = result.Equals("1");
            if (result == "1")
            {
                OrderJson = ja["OrderJson"].ToString();
                OrderJson = OrderJson.Replace("|", "\"");
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(OrderJson);
                    result = ja["Data"].ToString();
                    jrOrder = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
            }
            groupData = jrOrder;
            return ret;
        }
        /// <summary>
        /// 获取会员交易凭条内容
        /// </summary>
        /// <param name="aUserid"></param>
        /// <param name="gzData"></param>
        /// <returns></returns>
        public static bool getMemberSaleInfo(string aUserid, string orderid, out JArray gzData)
        {
            String OrderJson = "";
            JArray jrOrder = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getMemberSaleInfo/{0}/{1}/", aUserid, orderid);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                gzData = jrOrder;
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            bool ret = result.Equals("1");
            if (result == "1")
            {
                OrderJson = ja["OrderJson"].ToString();
                OrderJson = OrderJson.Replace("|", "\"");
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(OrderJson);
                    result = ja["Data"].ToString();
                    jrOrder = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
            }
            gzData = jrOrder;
            return ret;
        }


        /// <summary>
        /// 获取员工前台权限
        /// </summary>
        /// <param name="aUserid"></param>
        /// <param name="rightData"></param>
        /// <returns></returns>
        public static bool getUserRights(string aUserid, out JArray rightData)
        {
            String OrderJson = "";
            JArray jrOrder = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getUserRights/{0}/", aUserid);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                rightData = jrOrder;
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            bool ret = result.Equals("1");
            if (result == "1")
            {
                OrderJson = ja["OrderJson"].ToString();
                OrderJson = OrderJson.Replace("|", "\"");
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(OrderJson);
                    result = ja["Data"].ToString();
                    jrOrder = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
            }
            rightData = jrOrder;
            return ret;
        }
        public static bool getuserrights(string userid, string password, out JObject rightData)
        {
            JObject jrOrder = null;
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/userrights.json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("username");
            writer.WriteValue(userid);
            writer.WritePropertyName("password");
            writer.WriteValue(password);
            writer.WriteEndObject();
            writer.Flush();
            String rightjson = "";
            String jsonResult = Post_Rest(address, sw);
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            if (!jsonResult.Equals("0"))
            {
                try
                {
                    rightjson = ja["rights"].ToString();
                    jrOrder = (JObject)JsonConvert.DeserializeObject(rightjson);
                }
                catch { }
                rightData = jrOrder;
            }
            //将反序列化的JSON字符串转换成对象  
            string result = ja["result"].ToString();

            bool ret = result.Equals("0");
            rightData = jrOrder;
            return ret;
        }

        public static bool getMenuCombodish(string dishid, string menuid, out JObject jaData)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/getMenuCombodish.json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("dishides");
            writer.WriteValue(dishid);
            writer.WritePropertyName("menuid");
            writer.WriteValue(menuid);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            JObject ja = null;
            bool ret = true;// result.Equals("0");
            JArray jr = null;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                string rowsstr = ja["rows"].ToString();
                jr = (JArray)JsonConvert.DeserializeObject(rowsstr);
                ja = (JObject)jr[0];

            }
            catch { ret = false; }
            jaData = ja;
            return ret;
        }
        /// <summary>
        /// 获取第二杯半价和第二份起半价数据 由于当时做的是临时方案，固定了，后面有时间优化做成灵活的一个dataser转回来
        /// </summary>
        /// <param name="aUserid"></param>
        /// <param name="jsorder"></param>
        /// <param name="jrorder"></param>
        /// <returns></returns>
        public static bool getFavorale(string aUserid, string jsorder, out JArray jrorder, out JArray jrlist, out JArray jrdouble)
        {
            String OrderJson = "";
            String JSJson = "";
            String DoubleJson = "";
            JArray jrOrder = null;
            JArray jrList = null;
            JArray jrDouble = null;
            String posid = getPosID();
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getFavorale/{0}/{1}/", aUserid, jsorder);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                jrorder = jrOrder;
                jrlist = jrList;
                jrdouble = jrDouble;
                return false;
                //throw new Exception("连接服务器失败...");
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            bool ret = result.Equals("1");
            if (result == "1")
            {
                OrderJson = ja["OrderJson"].ToString();
                OrderJson = OrderJson.Replace("|", "\"");
                JSJson = ja["ListJson"].ToString();
                JSJson = JSJson.Replace("|", "\"");
                DoubleJson = ja["DoubleJson"].ToString();
                DoubleJson = DoubleJson.Replace("|", "\"");

                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(OrderJson);
                    result = ja["Data"].ToString();
                    jrOrder = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(JSJson);
                    result = ja["Data"].ToString();
                    jrList = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
                try
                {
                    ja = (JObject)JsonConvert.DeserializeObject(DoubleJson);
                    result = ja["Data"].ToString();
                    jrDouble = (JArray)JsonConvert.DeserializeObject(result);
                }
                catch { }
            }
            jrorder = jrOrder;
            jrlist = jrList;
            jrdouble = jrDouble;
            return ret;
        }

        ///以下为外卖接口  外卖开台
        ///1、开台
        ///2、下单
        ///3、
        ///
        //public static String founding = HTTP + URL_HOST + "/newspicyway/padinterface/setorder.json";
        public static bool setorder(string tableNo, string UserID, ref string orderid)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/setorder.json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("tableNo");
            writer.WriteValue(tableNo);
            writer.WritePropertyName("username");
            writer.WriteValue(UserID);
            writer.WritePropertyName("childNum");
            writer.WriteValue(0);
            writer.WritePropertyName("manNum");
            writer.WriteValue(0);
            writer.WritePropertyName("womanNum");
            writer.WriteValue(1);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            ///如果返回1，已经开台，先关掉
            ///{"result":"0","delaytime":"10","vipaddress":"192.168.40.25:8081","locktime":"120","backpsd":"1","orderid":"H20150416003934"}
            bool result = false;
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                string javaresult = ja["result"].ToString();
                result = ja["result"].ToString().Equals("0");
                orderid = ja["orderid"].ToString();
            }
            catch { return false; }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }
        /// <summary>
        /// 堂食开台接口
        /// </summary>
        /// <param name="tableNo"></param>
        /// <param name="UserID"></param>
        /// <param name="manNum"></param>
        /// <param name="womanNum"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static bool setorder(string tableNo, string UserID, int manNum, int womanNum, string ageperiod, ref string orderid)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/setorder.json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("tableNo");
            writer.WriteValue(tableNo);
            writer.WritePropertyName("username");
            writer.WriteValue(UserID);
            writer.WritePropertyName("childNum");
            writer.WriteValue(0);
            writer.WritePropertyName("manNum");
            writer.WriteValue(manNum);
            writer.WritePropertyName("womanNum");
            writer.WriteValue(womanNum);
            writer.WritePropertyName("ageperiod");
            writer.WriteValue(ageperiod);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            ///如果返回1，已经开台，先关掉
            ///{"result":"0","delaytime":"10","vipaddress":"192.168.40.25:8081","locktime":"120","backpsd":"1","orderid":"H20150416003934"}
            bool result = false;
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                string javaresult = ja["result"].ToString();
                result = ja["result"].ToString().Equals("0");
                orderid = ja["orderid"].ToString();
            }
            catch { return false; }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }
        /// <summary>
        /// 获取全部分类
        /// </summary>
        /// <returns></returns>
        public static bool getFoodType(out JArray jr)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/getMenuColumn.json", server2);
            String jsonResult = Post_Rest(address, null);
            bool result = false;
            JArray ja = null;
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(jsonResult);
                string javaresult = jo["rows"].ToString();
                ja = (JArray)JsonConvert.DeserializeObject(javaresult);
                result = true;// ja["result"].ToString().Equals("0");
            }
            catch { result = false; }
            //将反序列化的JSON字符串转换成对象  
            jr = ja;
            return result;

        }
        /// <summary>
        /// 退菜
        /// </summary>
        /// <param name="tableNo"></param>
        /// <param name="UserID"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static bool discarddish(StringWriter sw)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/discarddish.json", server2);
            String jsonResult = Post_Rest(address, sw);
            bool result = false;
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                string javaresult = ja["result"].ToString();
                result = ja["result"].ToString().Equals("0");
            }
            catch { return false; }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }
        public static bool verifyuser(string userid)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/verifyuser.json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("username");
            writer.WriteValue(userid);
            writer.WritePropertyName("loginType");
            writer.WriteValue("030101");
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            bool result = false;
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                string javaresult = ja["result"].ToString();
                result = ja["result"].ToString().Equals("0");
            }
            catch { return false; }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }
        public static string getAllFoodArray(ref JsonWriter writer, DataTable dt, string orderid, int ordertype)
        {
            writer.WriteStartArray();
            string str0 = "0";
            string str1 = "1";
            foreach (DataRow dr in dt.Rows)
            {
                string Groupid = dr["Groupid"].ToString();
                string Orderstatus = dr["Orderstatus"].ToString();
                string primarykey = getGUID();
                if (Groupid.Equals("") || Orderstatus.Equals("0"))
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("printtype");
                    writer.WriteValue(str0);
                    string orderprice = dr["price"].ToString();
                    if (ordertype == 1)
                        orderprice = "0";//赠送
                    string dishid = dr["dishid"].ToString();
                    string pricetype = ordertype.ToString();
                    if (dishid.Equals(Globals.cjSetting.Id))
                    {
                        orderprice = dr["price"].ToString();
                        pricetype = "0";
                    }
                    writer.WritePropertyName("pricetype");
                    writer.WriteValue(pricetype);
                    writer.WritePropertyName("orderprice");
                    writer.WriteValue(double.Parse(orderprice));
                    writer.WritePropertyName("orignalprice");
                    writer.WriteValue(double.Parse(dr["price2"].ToString()));
                    writer.WritePropertyName("dishid");
                    writer.WriteValue(dishid);
                    //如果鱼锅，用鱼锅方法取得鱼锅JSON
                    string dishes = "";
                    int dishtype = 0;
                    if (!Groupid.Equals(""))
                    {
                        string otype = dr["ordertype"].ToString();
                        //鱼锅 dishes ,以后加套餐再分开 Weigh
                        if (otype.Equals("2") || (otype.Equals("3")))
                        {
                            //套餐
                            dishtype = 2;
                            writer.WritePropertyName("dishes");
                            writer.WriteStartArray();
                            writeCombos(orderid, ref writer, dt, dr["Groupid"].ToString(), ordertype, true, primarykey);
                            writeCombos(orderid, ref writer, dt, dr["Groupid"].ToString(), ordertype, false, primarykey);
                            writer.WriteEndArray();
                        }
                        else
                        {
                            //鱼锅
                            dishtype = 1;
                            writer.WritePropertyName("dishes");
                            writer.WriteStartArray();
                            writeDishes(orderid, ref writer, dt, dr["Groupid"].ToString(), ordertype, primarykey);
                            writer.WriteEndArray();
                        }

                    }
                    else
                    {
                        writer.WritePropertyName("dishes");
                        writer.WriteValue(dishes);
                    }
                    string userid = dr["userid"].ToString();
                    writer.WritePropertyName("userName");
                    writer.WriteValue(userid);
                    string dishunit = dr["dishunit"].ToString();
                    writer.WritePropertyName("dishunit");
                    writer.WriteValue(dishunit);
                    writer.WritePropertyName("orderid");
                    writer.WriteValue(orderid);
                    writer.WritePropertyName("relatedishid");
                    writer.WriteValue("");
                    writer.WritePropertyName("dishtype");
                    writer.WriteValue(dishtype);//int.Parse(str0)
                    writer.WritePropertyName("orderseq");
                    writer.WriteValue(str1);
                    string dishnum = dr["dishnum"].ToString();
                    writer.WritePropertyName("dishnum");
                    writer.WriteValue(double.Parse(dishnum));
                    writer.WritePropertyName("sperequire"); //忌口
                    writer.WriteValue("");
                    writer.WritePropertyName("primarykey"); ////
                    writer.WriteValue(primarykey);
                    string dishstatus = "0";
                    if (dr["weigh"].ToString().Equals("1"))
                    {
                        dishstatus = "1";
                    }
                    writer.WritePropertyName("dishstatus");
                    writer.WriteValue(dishstatus);
                    writer.WritePropertyName("ispot");
                    writer.WriteValue("0");
                    writer.WriteEndObject();
                }

            }
            writer.WriteEndArray();
            return "";
        }
        private static void writeCombos(string orderid, ref JsonWriter writer, DataTable dt, string groupid, int ordertype, bool isfish, string primarykey)
        {
            string str0 = "0";
            string str1 = "1";
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Groupid"].ToString().Equals(groupid))
                {
                    bool canadd = true;
                    string orderstatus = dr["Orderstatus"].ToString();//isfish
                    if (isfish)
                    {
                        canadd = orderstatus == "6";
                    }
                    else
                    {
                        canadd = orderstatus != "6";
                    }
                    if (!dr["Orderstatus"].ToString().Equals("0") && !dr["Orderstatus"].ToString().Equals("5") && !dr["Orderstatus"].ToString().Equals("2") && canadd)
                    {
                        //单品
                        writer.WriteStartObject();
                        writer.WritePropertyName("printtype");
                        writer.WriteValue(str0);
                        string orderprice = dr["price"].ToString();
                        if (ordertype == 1)
                            orderprice = "0";//赠送
                        string dishid = dr["dishid"].ToString();
                        string pricetype = ordertype.ToString();
                        if (dishid.Equals(Globals.cjSetting.Id))
                        {
                            orderprice = dr["price"].ToString();
                            pricetype = "0";
                        }
                        writer.WritePropertyName("pricetype");
                        writer.WriteValue(pricetype);
                        writer.WritePropertyName("orderprice");
                        writer.WriteValue(double.Parse(orderprice));
                        writer.WritePropertyName("orignalprice");
                        writer.WriteValue(double.Parse(dr["price2"].ToString()));
                        writer.WritePropertyName("dishid");
                        writer.WriteValue(dishid);
                        //如果鱼锅，用鱼锅方法取得鱼锅JSON
                        string dishes = "";
                        int dishtype = 0;
                        writer.WritePropertyName("dishes");
                        string otype = dr["ordertype"].ToString();
                        if (dr["Orderstatus"].ToString().Equals("6"))
                        {
                            dishtype = 2;
                            //writer.WritePropertyName("dishes");
                            string groupid2 = dr["Groupid2"].ToString();
                            dr["Groupid2"] = groupid;
                            writer.WriteStartArray();
                            string primarykey2 = "";
                            writeDishes(orderid, ref writer, dt, groupid2, ordertype, primarykey2);
                            writer.WriteEndArray();
                        }
                        else
                            writer.WriteValue(dishes);
                        string userid = dr["userid"].ToString();
                        writer.WritePropertyName("userName");
                        writer.WriteValue(userid);
                        string dishunit = dr["dishunit"].ToString();
                        writer.WritePropertyName("dishunit");
                        writer.WriteValue(dishunit);
                        writer.WritePropertyName("orderid");
                        writer.WriteValue(orderid);
                        writer.WritePropertyName("relatedishid");
                        writer.WriteValue("");
                        writer.WritePropertyName("dishtype");
                        if (dr["Orderstatus"].ToString().Equals("6"))
                            writer.WriteValue(1);//int.Parse(str0)
                        else
                            writer.WriteValue(dishtype);//int.Parse(str0)
                        writer.WritePropertyName("orderseq");
                        writer.WriteValue(str1);
                        string dishnum = dr["dishnum"].ToString();
                        writer.WritePropertyName("dishnum");
                        writer.WriteValue(double.Parse(dishnum));
                        writer.WritePropertyName("sperequire"); //忌口
                        writer.WriteValue("");
                        writer.WritePropertyName("primarykey"); ////
                        writer.WriteValue(getGUID());
                        string dishstatus = "0";
                        if (dr["weigh"].ToString().Equals("1"))
                        {
                            dishstatus = "1";
                        }
                        writer.WritePropertyName("dishstatus");
                        writer.WriteValue(dishstatus);
                        writer.WritePropertyName("ispot");
                        if (dr["Orderstatus"].ToString().Equals("2"))
                            writer.WriteValue("1");
                        else
                            writer.WriteValue("0");
                        writer.WriteEndObject();
                    }
                }
            }
        }
        private static void writeDishes(string orderid, ref JsonWriter writer, DataTable dt, string groupid, int ordertype, string primarykey)
        {
            string str0 = "0";
            string str1 = "1";
            int i = 0;
            string primarykeydish = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Groupid2"].ToString().Equals(groupid))
                {
                    if (!dr["Orderstatus"].ToString().Equals("0"))
                    {
                        //单品
                        if (primarykey.Equals(""))
                            primarykeydish = getGUID();
                        else
                            primarykeydish = primarykey + "-" + i.ToString();
                        writer.WriteStartObject();
                        writer.WritePropertyName("printtype");
                        writer.WriteValue(str0);
                        string orderprice = dr["price"].ToString();
                        if (ordertype == 1)
                            orderprice = "0";//赠送
                        string dishid = dr["dishid"].ToString();
                        string pricetype = ordertype.ToString();
                        if (dishid.Equals(Globals.cjSetting.Id))
                        {
                            orderprice = dr["price"].ToString();
                            pricetype = "0";
                        }
                        writer.WritePropertyName("pricetype");
                        writer.WriteValue(pricetype);
                        writer.WritePropertyName("orderprice");
                        writer.WriteValue(double.Parse(orderprice));
                        writer.WritePropertyName("orignalprice");
                        writer.WriteValue(double.Parse(dr["price2"].ToString()));
                        writer.WritePropertyName("dishid");
                        writer.WriteValue(dishid);
                        //如果鱼锅，用鱼锅方法取得鱼锅JSON
                        string dishes = "";
                        int dishtype = 0;
                        writer.WritePropertyName("dishes");
                        writer.WriteValue(dishes);
                        string userid = dr["userid"].ToString();
                        writer.WritePropertyName("userName");
                        writer.WriteValue(userid);
                        string dishunit = dr["dishunit"].ToString();
                        writer.WritePropertyName("dishunit");
                        writer.WriteValue(dishunit);
                        writer.WritePropertyName("orderid");
                        writer.WriteValue(orderid);
                        writer.WritePropertyName("relatedishid");
                        writer.WriteValue("");
                        writer.WritePropertyName("dishtype");
                        writer.WriteValue(dishtype);//int.Parse(str0)
                        writer.WritePropertyName("orderseq");
                        writer.WriteValue(str1);
                        string dishnum = dr["dishnum"].ToString();
                        writer.WritePropertyName("dishnum");
                        writer.WriteValue(double.Parse(dishnum));
                        writer.WritePropertyName("sperequire"); //忌口
                        writer.WriteValue("");
                        writer.WritePropertyName("primarykey"); ////
                        writer.WriteValue(primarykeydish);
                        string dishstatus = "0";
                        if (dr["weigh"].ToString().Equals("1"))
                        {
                            dishstatus = "1";
                        }
                        writer.WritePropertyName("dishstatus");
                        writer.WriteValue(dishstatus);
                        writer.WritePropertyName("ispot");
                        if (dr["Orderstatus"].ToString().Equals("2"))
                            writer.WriteValue("1");
                        else
                            writer.WriteValue("0");
                        writer.WriteEndObject();
                        i++;
                    }
                }
            }
        }
        public static string getGUID()
        {
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }
        public static bool bookorder(DataTable dt, string tableid, string UserID, string orderid, int sequence, int ordertype)
        {
            //string address = String.Format("http://{0}/" + apiPath + "/padinterface/bookorder.json", server2);
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/bookorderList.json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("currenttableid");
            writer.WriteValue(tableid);
            writer.WritePropertyName("globalsperequire");
            writer.WriteValue("");
            writer.WritePropertyName("orderid");
            writer.WriteValue(orderid);
            writer.WritePropertyName("operationType");
            writer.WriteValue(1);
            writer.WritePropertyName("sequence");
            writer.WriteValue(sequence);
            writer.WritePropertyName("rows");//所有菜品
            getAllFoodArray(ref writer, dt, orderid, ordertype);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            ///如果返回1，已经开台，先关掉
            ///{"result":"0","delaytime":"10","vipaddress":"192.168.40.25:8081","locktime":"120","backpsd":"1","orderid":"H20150416003934"}
            bool result = false;
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                string javaresult = ja["result"].ToString();
                if (javaresult.Equals("1"))
                {
                    //返回现有的帐单号，用现有的帐单号结算
                    //RestClient.GetTableInfo()
                }
                result = ja["result"].ToString().Equals("0");
            }
            catch { return false; }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }
        public static bool getSystemSetData(string settingname, out TSetting setting)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/getSystemSetData.Json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue(settingname);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            bool result = false;
            setting = new TSetting();
            setting.Itemid = "0";
            setting.Type = "";
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                string javaresult = ja["rows"].ToString();
                JArray jr = (JArray)JsonConvert.DeserializeObject(javaresult);
                ja = (JObject)jr[0];
                setting.Id = ja["id"].ToString();
                setting.Status = ja["status"].ToString();
                setting.ItemSort = ja["itemSort"].ToString();
                setting.Typename = ja["typename"].ToString();
                setting.ItemDesc = ja["itemDesc"].ToString();
                setting.Itemid = ja["itemid"].ToString();
                setting.Type = ja["type"].ToString();
                result = true;
            }
            catch { return false; }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }
        public static bool getSystemSetData(out TRoundInfo roundJson)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/getSystemSetData.Json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue("ROUNDING");
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            bool result = false;
            roundJson = new TRoundInfo();
            roundJson.Itemid = "0";
            roundJson.Type = "";
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                string javaresult = ja["rows"].ToString();
                JArray jr = (JArray)JsonConvert.DeserializeObject(javaresult);
                ja = (JObject)jr[0];
                roundJson.Id = ja["id"].ToString();
                roundJson.Status = ja["status"].ToString();
                roundJson.ItemSort = ja["itemSort"].ToString();
                roundJson.Typename = ja["typename"].ToString();
                roundJson.ItemDesc = ja["itemDesc"].ToString();
                roundJson.Itemid = ja["itemid"].ToString();
                roundJson.Type = ja["type"].ToString();
                ja = (JObject)jr[1];
                roundJson.Roundtype = ja["itemid"].ToString();
                result = true;// ja["result"].ToString().Equals("0");

            }
            catch { return false; }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }

        /// <summary>
        /// 清除帐单
        /// </summary>
        /// <param name="tableno"></param>
        /// <returns></returns>
        public static bool cleantable(string tableno)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/cleantable.Json", server2);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("tableNo");
            writer.WriteValue(tableno);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            bool result = false;
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString().Equals("0");
            }
            catch { return false; }
            //将反序列化的JSON字符串转换成对象  
            return result;
        }
        public static bool getOrderCouponList(string aUserid, string orderid, out JArray jrorder)
        {
            String OrderJson = "";
            JArray jrOrder = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/GetOrderCouponList/{0}/{1}/", orderid, aUserid);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                jrorder = jrOrder;
                return false;
                //throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            OrderJson = result;
            OrderJson = OrderJson.Replace("|", "\"");
            try
            {
                //ja = (JObject)JsonConvert.DeserializeObject(OrderJson);
                //result = ja["Data"].ToString();
                jrOrder = (JArray)JsonConvert.DeserializeObject(result);
            }
            catch { }
            jrorder = jrOrder;
            //将反序列化的JSON字符串转换成对象  
            //string result = jr["result"].ToString();
            return jrorder != null;
        }

        public static bool getBackDishInfo(string orderid, string dishid, string dishunit, string tableno, out JArray jrorder)
        {
            String OrderJson = "";
            JArray jrOrder = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getBackDishInfo/{0}/{1}/{2}/{3}/", orderid, dishid, dishunit, tableno);
            String jsonResult = Request_Rest(address);
            if (jsonResult.Equals("0"))
            {
                jrorder = jrOrder;
                return false;
                //throw new Exception("连接服务器失败...");

            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            //将反序列化的JSON字符串转换成对象  
            string result = ja["Data"].ToString();
            OrderJson = result;
            OrderJson = OrderJson.Replace("|", "\"");
            try
            {
                jrOrder = (JArray)JsonConvert.DeserializeObject(result);
            }
            catch { }
            jrorder = jrOrder;
            //将反序列化的JSON字符串转换成对象  
            return jrorder != null;
        }
        public static bool deletePosOperation(string tableno)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/deletePosOperation/{0}", tableno);
            String jsonResult = Request_Rest(address);
            return true;
        }

        public static void getTicketList(String psTicketInfo)
        {
            pszTicket[] pszTicketList = null;
            string tickstr = psTicketInfo;
            string[] Ticks = null;
            Ticks = tickstr.Split(new char[] { ';' });
            if (tickstr.Trim().ToString().Length <= 0)
            {
                Array.Resize(ref pszTicketList, 0);
            }
            else
            {
                try
                {
                    Array.Resize(ref pszTicketList, Ticks.Length);
                    string[] tick = null;
                    String preCoupon_code = "";
                    for (int i = 0; i < Ticks.Length; i++)
                    {
                        string str = Ticks[i].ToString();
                        tick = str.Split(new char[] { '|' });
                        if (preCoupon_code != tick[0])
                        {
                            pszTicketList[i].Coupon_code = tick[0];
                            pszTicketList[i].Coupons_Name = tick[3];
                            pszTicketList[i].Coupon_Amount = 0;
                            try
                            {
                                String tickprice = (float.Parse(tick[1]) / 100.00).ToString();
                                pszTicketList[i].Coupon_Amount = float.Parse(tickprice);
                            }
                            catch { }
                            pszTicketList[i].Coupon_No = tick[4];
                            pszTicketList[i].Coupon_NoAmount = 0;
                            pszTicketList[i].Copon_Type = tick[2];
                            preCoupon_code = tick[0];
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 获取所有银行数据。
        /// </summary>
        /// <returns></returns>
        public static List<BankInfo> GetAllBankInfos()
        {
            var addr = string.Format("http://{0}/" + apiPath + "/bankinterface/getallbank.json", server2);
            List<BankInfo> info = new List<BankInfo>();
            try
            {
                string jsonResult = Request_Rest(addr);
                JArray dataArray = (JArray)JsonConvert.DeserializeObject(jsonResult);
                foreach (var da in dataArray)
                {
                    BankInfo item = new BankInfo();
                    item.Id = Convert.ToInt32(da["itemid"].ToString());
                    item.Name = da["itemDesc"].ToString();
                    item.SortIndex = Convert.ToInt32(da["itemSort"].ToString());
                    info.Add(item);
                }
                return info.OrderBy(t => t.SortIndex).ToList();
            }
            catch (Exception exp)
            {
                return info;
            }
        }
    }


}


/*

修改菜品重量
	JSONObject jsonObject = new JSONObject();
					jsonObject.put("tableNo", tableno);
					jsonObject.put("dishid", modifyDishInfo.getDishId());
					List<? extends IBaseDishCount> counts = IShopCart.getOrderDishes().get(modifyDishInfo.getDishId());
					if (typeflag.equals("1")) {
						int normal_size = counts.get(fishpotposition).getNumbers().length;
						if (normal_size > 1) {// 多单位计量
							jsonObject.put("primarykey", ((INormalDishCount) counts.get(fishpotposition)).getPrimarykey() + "-" + fishPosition);
						} else {
							jsonObject.put("primarykey", ((INormalDishCount) counts.get(fishpotposition)).getPrimarykey());
						}
					}
					if (typeflag.equals("4")) {
						jsonObject.put("primarykey", modifyDishCount.getPrimarykey() + "-" + fishPosition);
					}

					jsonObject.put("dishnum", et_modifydishweight.getText().toString());
public static String modifydish = HTTP + URL_HOST + WEB_NAME + "/padinterface/updateDishWeight.json";
5.结算
http://192.168.40.186:8080/newspicyway/padinterface/settleorder.json
{
	"userName" : "admin",
	"orderNo" : "001",
	"payDetail" : [{
			"payWay" : "0",（现金）
			"payAmount" : "100", 
		}, {
			"payWay" : "1",（银行卡）
			"payAmount" : "200", 
			"bankCardNo" : "7777777777"
		}, {
			"payWay" : "2",（会员卡）
			"payAmount" : "300",
			"memerberCardNo" : "9999",
		}
	]
}
*/