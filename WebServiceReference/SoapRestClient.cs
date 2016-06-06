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
        private const string AccessErrorFlag = "Access violation";
        private static bool alreadLogAllTableInfo;//是否已经记录了所有餐台信息。防止每次获取餐台信息时都打印接口返回数据。
        private static bool alreadLogAllFood;

        public static string server = "";
        public static string server2 = "";
        public static string Server3 = "";
        public static string dataServer = "";
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
        public static string GetMacAddr()
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

            xpath = "configuration/client/DataServerYazuo";
            node = xml.SelectSingleNode(xpath);
            dataServer = node.Attributes["address"].Value;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">访问URL</param>
        /// <param name="timeoutSecond">接口超时时间，默认15秒。</param>
        /// <param name="restartDataServerTimes">重启DataServer次数，默认1次。</param>
        /// <returns></returns>
        private static string Request_Rest(string url, int timeoutSecond = 15, int restartDataServerTimes = 1)
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
                request.Timeout = timeoutSecond * 1000;
                response = request.GetResponse() as HttpWebResponse;
                if (request.HaveResponse == true && response != null)
                {
                    reader = new StreamReader(response.GetResponseStream());
                    sbSource = new StringBuilder(reader.ReadToEnd());
                    string returnStr = FromUnicodeString(sbSource.ToString());
                    if (returnStr.StartsWith(AccessErrorFlag))
                        throw new Exception("DataServer访问越界，返回数据错误。");

                    returnStr = returnStr.Replace("{\"result\":[\"", "");
                    returnStr = returnStr.Replace("\"]}", "");
                    return returnStr;
                }
            }
            catch (WebException wex)
            {
                AllLog.Instance.E(wex);
                var serverConnect = CheckServerConnection();
                if (!string.IsNullOrEmpty(serverConnect))
                {
                    AllLog.Instance.E(serverConnect);
                    Msg.ShowError(serverConnect);
                }
                else
                {
                    if (restartDataServerTimes > 0)
                    {
                        if (RestartDataserver())
                        {
                            return Request_Rest(url, timeoutSecond, --restartDataServerTimes);
                        }
                    }
                    Msg.ShowError("DataServer服务或网络出现问题，请联系管理人员。");
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
            return Request_Rest(url, 60);
        }

        public static String Post_Rest(string url, StringWriter sw, int timeoutSecond = 30)
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
                request.ContentType = "application/json; charset=utf-8";
                request.Timeout = timeoutSecond * 1000;
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
                AllLog.Instance.E("Addr：{0}。Exception：{1}", url, wex.Message);
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


        /// <summary>
        ///登录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// http://192.168.102.7/ladaotu/padinterface/login.json
        public static string Login(string userid, string password, string loginType)
        {
            string newloginType = getRightCode(loginType);
            string address = "http://" + server + "/" + apiPath + "/padinterface/login.json";
            AllLog.Instance.I(string.Format("【login】 userid：{0}，loginType：{1}。", userid, loginType));
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
            Console.WriteLine(jsonText);
            String jsonResult = Post_Rest(address, sw);
            AllLog.Instance.I("【login】 result：{0}。", jsonResult);
            if (jsonResult == "0")
                return "";

            Globals.UserInfo.msg = jsonResult;
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            Globals.UserInfo.msg = "";
            string result = ja["result"].ToString();
            string username = ja["fullname"] != null ? ja["fullname"].ToString() : "";
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

            if (ja["msg"] != null)
                Globals.UserInfo.msg = ja["msg"].ToString();
            return result;
        }

        public static string GetServerTableInfo(string TableName, string UserID)
        {
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/GetServerTableInfo/{0}/{1} ", TableName, UserID);
            AllLog.Instance.I(string.Format("【GetServerTableInfo】 TableName：{0}，UserID：{1}。", TableName, UserID));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【GetServerTableInfo】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return "";

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            string result = ja["Data"].ToString();
            if (!result.Equals("0"))
            {
                JArray jr = (JArray)JsonConvert.DeserializeObject(result);
                ja = (JObject)jr[0];
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
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/GetOrder/{0}/{1} ", TableName, UserID);
            AllLog.Instance.I(string.Format("【GetOrder】 TableName：{0}，UserID：{1}。", TableName, UserID));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【GetOrder】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return "";

            JObject jaAll = (JObject)JsonConvert.DeserializeObject(jsonResult);
            string result = jaAll["Data"].ToString();
            if (!result.Equals("0"))
            {
                string tableinfojson = jaAll["OrderJson"].ToString();
                JObject ja = (JObject)JsonConvert.DeserializeObject(tableinfojson);
                result = ja["Data"].ToString();
                JArray jr = (JArray)JsonConvert.DeserializeObject(result);
                ja = (JObject)jr[0];
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

                    //国际化处理品项名称和单位
                    var column = DataTableHelper.CreateDataColumn(typeof(string), "原始单位", "dishunitSrc", "");//中英文国际化的原始单位。
                    dt.Columns.Add(column);
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["title"] = InternationaHelper.GetBeforeSeparatorFlagData(dr["title"].ToString());
                        dr["dishunitSrc"] = dr["dishunit"];
                        dr["dishunit"] = InternationaHelper.GetBeforeSeparatorFlagData(dr["dishunit"].ToString());
                    }

                    Globals.OrderTable = dt;
                }
            }
            return result;


        }

        public static bool setMemberPrice(string UserID, string OrderID, string memberno)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/setMemberPrice/{0}/{1}/{2}/{3}/", UserID, OrderID, ipaddress, memberno);
            AllLog.Instance.I(string.Format("【setMemberPrice】 OrderID：{0}，memberno：{1}。", OrderID, memberno));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【setMemberPrice】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return false;

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            return ja["Data"].ToString() == "1";
        }

        /// <summary>
        /// 设回会员价。
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public static bool setMemberPrice2(string UserID, string OrderID)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/setMemberPrice2/{0}/{1}/{2}/", UserID, OrderID, ipaddress);
            AllLog.Instance.I(string.Format("【setMemberPrice2】 OrderID：{0}。", OrderID));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【setMemberPrice2】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return false;

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            return ja["Data"].ToString() == "1";
        }

        public static bool cancelOrder(string UserID, string OrderID, string tableno)
        {
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/cancelOrder/{0}/{1}/{2}/", UserID, OrderID, tableno);
            AllLog.Instance.I(string.Format("【cancelOrder】 OrderID：{0}，tableno：{1}。", OrderID, tableno));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【cancelOrder】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return false;

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            return ja["Data"].ToString() == "1";
        }

        public static bool rebackorder(string UserID, string OrderID, ref String errStr)
        {
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/rebackorder/{0}/{1}/", UserID, OrderID);
            AllLog.Instance.I(string.Format("【rebackorder】 OrderID：{0}。", OrderID));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【rebackorder】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return false;

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            errStr = ja["Info"].ToString();
            return ja["Data"].ToString() == "1";
        }

        public static bool accountsorder(string UserID, string OrderID, ref String errStr)
        {
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/accountsorder/{0}/{1}/", UserID, OrderID);
            AllLog.Instance.I(string.Format("【accountsorder】 OrderID：{0}。", OrderID));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【accountsorder】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return false;

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            errStr = ja["Info"].ToString();
            return ja["Data"].ToString() == "1";
        }

        /// <summary>
        /// 获取帐单列表
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static string GetServerTableList(string OrderID, string UserID)
        {
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/GetServerTableList/{0}/{1} ", OrderID, UserID);
            AllLog.Instance.I(string.Format("【GetServerTableList】 OrderID：{0}。", OrderID));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【GetServerTableList】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return "";

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            string result = ja["Data"].ToString();

            try
            {
                Globals.OrderTable.Clear();
            }
            catch
            {
                // ignored
            }

            if (!result.Equals("0"))
            {
                //把JSON转为DataSet
                DataTableConverter dtc = new DataTableConverter();
                JsonReader jread = new JsonTextReader(new StringReader(result));
                DataTable dt = new DataTable();
                dt.TableName = "tb_data";
                dt.Clear();
                dtc.ReadJson(jread, typeof(DataTable), dt, new JsonSerializer());
                Globals.OrderTable.Clear();

                //国际化处理品项名称和单位
                var column = DataTableHelper.CreateDataColumn(typeof(string), "原始单位", "dishunitSrc", "");//中英文国际化的原始单位。
                dt.Columns.Add(column);
                foreach (DataRow dr in dt.Rows)
                {
                    dr["title"] = InternationaHelper.GetBeforeSeparatorFlagData(dr["title"].ToString());
                    dr["dishunitSrc"] = dr["dishunit"];
                    dr["dishunit"] = InternationaHelper.GetBeforeSeparatorFlagData(dr["dishunit"].ToString());
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
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/posrebacksettleorder/{0}/{1}/{2}/", OrderID, UserID, server);
            AllLog.Instance.I(string.Format("【posrebacksettleorder】 OrderID：{0}。", OrderID));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【posrebacksettleorder】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return false;

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            return ja["Data"].ToString() == "1";
        }

        /// <summary>
        /// 反结算
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static bool rebacksettleorder(string OrderID, string UserID, string reason, out string msg)
        {
            msg = null;
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/rebacksettleorder.json", server2);
            AllLog.Instance.I(string.Format("【rebacksettleorder】 OrderID：{0}，reason：{1}。", OrderID, reason));
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
            AllLog.Instance.I(string.Format("【rebacksettleorder】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return false;

            string result = "1";
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString();
                msg = ja["msg"].ToString();
            }
            catch { }
            return result.Equals("0");
        }

        public static string debitamout(string OrderID)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/debitamout.json", server2);
            AllLog.Instance.I(string.Format("【debitamout】 OrderID：{0}。", OrderID));
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("orderNo");
            writer.WriteValue(OrderID);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【debitamout】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return "";

            string result = "1";
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString();
            }
            catch { }
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
            AllLog.Instance.I(string.Format("【updateDishWeight】 tableNo：{0}，dishid：{1}，primarykey：{2}，dishnum：{3}。", tableNo, dishid, primarykey, dishnum));
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
            AllLog.Instance.I(string.Format("【updateDishWeight】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return "";

            string result = "1";
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString();
            }
            catch { }
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
            AllLog.Instance.I(string.Format("【settleorder】 OrderID：{0}。", OrderID));
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

            AllLog.Instance.I(string.Format("【settleorder】 request：{0}。", sw2));
            String jsonResult = Post_Rest(address, sw2);
            AllLog.Instance.I(string.Format("【settleorder】 result：{0}。", jsonResult));
            if (jsonResult == "0")
                return "";

            string result = "-1";
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString();
            }
            catch { }
            return result;
        }

        //查询
        public static JObject QueryBalance(string memberinfo)
        {
            string address = String.Format("http://" + dataServer + "/datasnap/rest/TServerMethods1/QueryBalance/{0}/", memberinfo);
            AllLog.Instance.I(string.Format("【QueryBalance】 memberinfo：{0}。", memberinfo));
            String jsonResult = Request_Rest60(address);
            AllLog.Instance.I(string.Format("【QueryBalance】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败,请检查内网是否正常...");

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            return ja;
        }

        /// <summary>
        /// 反结算
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="pszPwd"></param>
        /// <param name="pszGPwd"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool VoidSale(string orderid, string pszPwd, string pszGPwd, out string info)
        {
            //orderid,pszPwd,pszGPwd
            string address = String.Format("http://" + dataServer + "/datasnap/rest/TServerMethods1/VoidSale/{0}/{1}/{2}/", orderid, pszPwd, pszGPwd);
            AllLog.Instance.I(string.Format("【VoidSale】 orderid：{0}。", orderid));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【VoidSale】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败...");

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            string result = ja["Data"].ToString();
            info = ja["Info"].ToString();
            return result.Equals("1");
        }

        //充值
        public static JObject StoreCardDeposit(string memberinfo, double pszAmount, string pszSerial, int paytype)
        {
            int psTransType = 0;
            string address = String.Format("http://" + dataServer + "/datasnap/rest/TServerMethods1/StoreCardDeposit/{0}/{1}/{2}/{3}/{4}/{5}/", Globals.UserInfo.UserID, memberinfo, pszAmount, pszSerial, psTransType, paytype);
            AllLog.Instance.I(string.Format("【StoreCardDeposit】 memberinfo：{0}，pszAmount：{1}，pszSerial：{2}。", memberinfo, pszAmount, pszSerial));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【StoreCardDeposit】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败...");

            return (JObject)JsonConvert.DeserializeObject(jsonResult);
        }

        //激活卡
        public static JObject CardActive(string pszTrack2, string pszPwd, string pszMobile)
        {
            if (pszPwd.Trim().ToArray().Length <= 0)
                pszPwd = " ";
            string address = String.Format("http://" + dataServer + "/datasnap/rest/TServerMethods1/CardActive/{0}/{1}/{2}/", pszTrack2, pszPwd, pszMobile);
            AllLog.Instance.I(string.Format("【CardActive】 pszTrack2：{0}，pszMobile：{1}。", pszTrack2, pszMobile));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【CardActive】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败...");

            return (JObject)JsonConvert.DeserializeObject(jsonResult);
        }

        //会员卡消费
        public static JObject MemberSale(string aUserid, string orderid, string pszInput, string pszSerial, float pszCash, float pszPoint, int psTransType, float pszStore, string pszTicketList, string pszPwd, float memberyhqamount)
        {
            if (pszTicketList.Length <= 0)
                pszTicketList = "  ";

            string address = String.Format("http://" + dataServer + "/datasnap/rest/TServerMethods1/Sale/{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/", aUserid, orderid, pszInput, pszSerial, pszCash, pszPoint, psTransType, pszStore, pszTicketList, pszPwd, memberyhqamount, server);
            AllLog.Instance.I(string.Format("【Sale】 orderid：{0}，pszCash：{1}，memberyhqamount：{2}。", orderid, pszCash, memberyhqamount));
            String jsonResult = Request_Rest60(address);//会员结算的超时时间要多给点
            AllLog.Instance.I(string.Format("【Sale】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败，请检查内网网络...");

            return (JObject)JsonConvert.DeserializeObject(jsonResult);
        }

        public static bool OpenUp(string aUserID, string aUserPassword, int CallType, out string reinfo)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/OpenUp/{0}/{1}/{2}/{3}/", aUserID, aUserPassword, ipaddress, CallType);
            AllLog.Instance.I(string.Format("【OpenUp】 aUserID：{0}。", aUserID));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【OpenUp】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败,请检查服务器是否开机,内网是否正常...");

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            string workdate = ja["workdate"].ToString();
            reinfo = ja["Info"].ToString();
            Globals.workdate = workdate;

            return ja["Data"].ToString().Equals("1");
        }

        //保存优惠内容
        public static bool saveOrderPreferential(string aUserid, string OrderID, string Preferential)
        {
            Preferential = Preferential.Replace(@"\", "");
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/saveOrderPreferential/{0}/{1}/{2}/{3}/", aUserid, ipaddress, OrderID, Preferential);
            AllLog.Instance.I(string.Format("【saveOrderPreferential】 OrderID：{0}，Preferential：{1}。", OrderID, Preferential));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【saveOrderPreferential】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败...");

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            return ja["Data"].ToString().Equals("1");
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
            AllLog.Instance.I(string.Format("【clearMachine】 username：{0}，authorizer：{1}。", username, authorizer));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【clearMachine】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败...");

            return (JObject)JsonConvert.DeserializeObject(jsonResult);
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
            AllLog.Instance.I(string.Format("【endWork】 userid：{0}。", userid));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【endWork】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败...");

            return (JObject)JsonConvert.DeserializeObject(jsonResult);
        }

        /// <summary>
        /// 零找金接口
        /// </summary>
        /// <param name="aUserID"></param>
        /// <param name="cachamount"></param>
        /// <param name="CallType"></param>
        /// <param name="reinfo"></param>
        /// <returns></returns>
        public static bool InputTellerCash(string aUserID, double cachamount, int CallType, out string reinfo)
        {
            string mac = GetMacAddr();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/InputTellerCash/{0}/{1}/{2}/{3}/", aUserID, mac, cachamount, CallType);
            AllLog.Instance.I(string.Format("【InputTellerCash】 aUserID：{0}，cachamount：{1}，CallType：{2}。", aUserID, cachamount, CallType));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【InputTellerCash】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败...");

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            string workdate = ja["workdate"].ToString();
            reinfo = ja["Info"].ToString();
            Globals.workdate = workdate;

            return ja["Data"].ToString().Equals("1");
        }

        /// <summary>
        /// jde同步资料回调
        /// </summary>
        /// <returns>执行结果。执行成功返回null，当有错误时返回错误信息。</returns>
        public static string jdesyndata()
        {
            try
            {
                string address = "http://" + server + "/" + apiPath + "/padinterface/jdesyndata.json";
                AllLog.Instance.I("【jdesyndata】 begin。");
                StringWriter sw = new StringWriter();
                JsonWriter writer = new JsonTextWriter(sw);
                writer.WriteStartObject();
                writer.WritePropertyName("synkey");
                writer.WriteValue("candaosynkey");
                writer.WriteEndObject();
                writer.Flush();
                string jsonResult = Post_Rest(address, sw, 500);
                AllLog.Instance.I(string.Format("【jdesyndata】 result：{0}。", jsonResult));
                if (jsonResult == "0")
                    return "结业数据上传超时。";

                var jr = (JObject)JsonConvert.DeserializeObject(jsonResult);
                var result = jr["code"].ToString().Equals("0000");
                var info = jr["message"] != null ? jr["message"].ToString() : null;
                return result ? null : (string.IsNullOrEmpty(info) ? "结业数据上传失败。" : info);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
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
            AllLog.Instance.I(string.Format("【broadcastmsg】 msgid：{0}，msg：{1}。", msgid, msg));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【broadcastmsg】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败...");
        }

        public static void putOrder(string tableno, string orderid, TGzInfo gzinfo)
        {
            if (gzinfo.Gzcode == null)
                gzinfo.Gzcode = "0";
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/putOrder/{0}/{1}/{2}/{3}/{4}/{5}/", tableno, orderid, gzinfo.Gzcode, gzinfo.Gzname, gzinfo.Telephone, gzinfo.Relaperson);
            AllLog.Instance.I(string.Format("【putOrder】 tableno：{0}，orderid：{1}。", tableno, orderid));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【putOrder】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败...");
        }

        public static bool getOrderSequence(string tableno, out string sequence)
        {
            bool ret = false;
            sequence = "";
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getOrderSequence/{0}/", tableno);
            AllLog.Instance.I(string.Format("【getOrderSequence】 tableno：{0}。", tableno));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【getOrderSequence】 result：{0}。", jsonResult));
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                ret = ja["Data"].ToString().Equals("1");
                if (ret)
                    sequence = ja["Info"].ToString();
            }
            catch { }
            return ret;
        }

        public static void wmOrder(string orderid)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/wmOrder/{0}/", orderid);
            AllLog.Instance.I(string.Format("【wmOrder】 orderid：{0}。", orderid));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【wmOrder】 result：{0}。", jsonResult));
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

            string ipaddress = GetLocalIp();
            string address = "http://" + server + "/" + apiPath + "/padinterface/getPreferentialList.json";
            AllLog.Instance.I(string.Format("【getPreferentialList】 typeid：{0}，orderid：{1}。", typeid, orderid));
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
            String jsonResult = Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【getPreferentialList】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
                throw new Exception("连接服务器失败...");

            string result = jsonResult;// ja["Data"].ToString();
            try
            {
                jr = (JArray)JsonConvert.DeserializeObject(result);
            }
            catch { }
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
            AllLog.Instance.I(string.Format("【usePreferentialItem】 preferentialid：{0}，disrate：{1}，orderid：{2}。", preferentialid, disrate, orderid));
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
            //需要增加一个参数 PreferentialAmt记录所有已选的挂帐和优免金额 preferentialAmt 传给后台计算的时候去掉优惠
            writer.WritePropertyName("preferentialAmt");
            writer.WriteValue(preferentialAmt.ToString());
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = sw.GetStringBuilder().ToString();
            AllLog.Instance.I(string.Format("【usePreferentialItem】 request：{0}，", sw));
            String jsonResult = Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【usePreferentialItem】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                msg = "连接服务器失败...";
                return false;
            }

            JObject ja;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            }
            catch
            {
                amount = 0;
                msg = jsonResult;
                return false;
            }

            msg = ja["msg"].ToString();
            amount = float.Parse(ja["amount"].ToString());
            return ja["result"].ToString().Equals("1");
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
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getOrderInfo/{0}/{1}/{2}/", aUserid, orderid, printtype);
            AllLog.Instance.I(string.Format("【getOrderInfo】 orderid：{0}，printtype：{1}。", orderid, printtype));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【getOrderInfo】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                jrorder = jrOrder;
                jrjs = jrJS;
                jrlist = jrList;
                return false;
            }

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
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
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getClearMachineData/{0}/{1}/{2}/", aUserid, jsorder, posid);
            AllLog.Instance.I(string.Format("【getClearMachineData】 aUserid：{0}，jsorder：{1}。", aUserid, jsorder));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【getClearMachineData】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                jrorder = jrOrder;
                jrjs = jrJS;
                return false;
            }

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
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
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/caleTableAmount/{0}/{1}/", orderid, aUserid);
            AllLog.Instance.I(string.Format("【caleTableAmount】 aUserid：{0}，orderid：{1}。", aUserid, orderid));
            String jsonResult = Request_Rest60(address);
            AllLog.Instance.I(string.Format("【caleTableAmount】 result：{0}。", jsonResult));
            return jsonResult.Equals("1");
        }

        public static void openCashCom()
        {
            portname = "COM" + getPortName();
            if (portname.Trim().Length > 0)
            {
                sc = new SerialClass(portname);
                try
                {
                    sc.openPort();
                }
                catch { }
            }
        }

        public static bool OpenCash()
        {
            String jsonResult = "";
            bool ret = false;
            portname = getPortName();
            if (portname.Trim().Length <= 0)
            {
                try
                {
                    string ip = getOpenCashIP();
                    string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/OpenCash/{0}/", ip);
                    AllLog.Instance.I("【OpenCash】 start。");
                    jsonResult = Request_Rest(address);
                    AllLog.Instance.I(string.Format("【OpenCash】 result：{0}。", jsonResult));
                    ret = jsonResult.Equals("1");
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
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getAllOrderInfo2/{0}/", aUserid);
            AllLog.Instance.I(string.Format("【getAllOrderInfo2】 aUserid：{0}。", aUserid));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【getAllOrderInfo2】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                jrorder = null;
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
            int result = 0;
            try
            {
                string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getFoodStatus/{0}/{1}/", dishid, dishunit);
                AllLog.Instance.I(string.Format("【getFoodStatus】 dishid：{0}，dishunit：{1}。", dishid, dishunit));
                String jsonResult = Request_Rest(address);
                AllLog.Instance.I(string.Format("【getFoodStatus】 result：{0}。", jsonResult));
                if (jsonResult.Equals("0"))
                    return 0;

                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
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
            AllLog.Instance.I(string.Format("【getAllGZDW】 aUserid：{0}。", aUserid));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【getAllGZDW】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                gzData = jrOrder;
                return false;
            }

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
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
            if (!alreadLogAllFood)
                AllLog.Instance.I(string.Format("【getAllWmFood】 aUserid：{0}。", aUserid));
            String jsonResult = Request_Rest(address);
            if (!alreadLogAllFood)
                AllLog.Instance.I(string.Format("【getAllWmFood】 result：{0}。", jsonResult));
            alreadLogAllFood = true;
            if (jsonResult.Equals("0"))
            {
                wmData = jrOrder;
                return false;
            }

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
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
            AllLog.Instance.I(string.Format("【getCJFood】 aUserid：{0}。", aUserid));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【getCJFood】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                cjData = jrOrder;
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
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
            AllLog.Instance.I(string.Format("【getGroupDetail】 dishid：{0}。", dishid));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【getGroupDetail】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                groupData = jrOrder;
                return false;
            }

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
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
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getMemberSaleInfo/{0}/{1}/", aUserid, orderid);
            AllLog.Instance.I(string.Format("【getMemberSaleInfo】 orderid：{0}，aUserid：{1}。", orderid, aUserid));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【getMemberSaleInfo】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                gzData = jrOrder;
                return false;
            }
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
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
            AllLog.Instance.I(string.Format("【getUserRights】 aUserid：{0}。", aUserid));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【getUserRights】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                rightData = jrOrder;
                return false;
            }

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
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
            AllLog.Instance.I(string.Format("【userrights】 userid：{0}。", userid));
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
            AllLog.Instance.I(string.Format("【userrights】 result：{0}。", jsonResult));
            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            if (!jsonResult.Equals("0"))
            {
                try
                {
                    rightjson = ja["rights"].ToString();
                    jrOrder = (JObject)JsonConvert.DeserializeObject(rightjson);
                }
                catch { }
            }

            rightData = jrOrder;
            return ja["result"].ToString().Equals("0");
        }

        public static bool getMenuCombodish(string dishid, string menuid, out JObject jaData)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/getMenuCombodish.json", server2);
            AllLog.Instance.I(string.Format("【getMenuCombodish】 dishid：{0}，menuid：{1}。", dishid, menuid));
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
            AllLog.Instance.I(string.Format("【getMenuCombodish】 result：{0}。", jsonResult));
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
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getFavorale/{0}/{1}/", aUserid, jsorder);
            AllLog.Instance.I(string.Format("【getFavorale】 aUserid：{0}，jsorder：{1}。", aUserid, jsorder));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【getFavorale】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                jrorder = jrOrder;
                jrlist = jrList;
                jrdouble = jrDouble;
                return false;
            }

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
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
            AllLog.Instance.I(string.Format("【setorder】 tableNo：{0}，UserID：{1}。", tableNo, UserID));
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
            AllLog.Instance.I(string.Format("【setorder】 result：{0}。", jsonResult));
            bool result = false;
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                string javaresult = ja["result"].ToString();
                result = ja["result"].ToString().Equals("0");
                orderid = ja["orderid"].ToString();
            }
            catch { return false; }
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
            AllLog.Instance.I(string.Format("【setorder】 tableNo：{0}，ageperiod：{1}。", tableNo, ageperiod));
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
            AllLog.Instance.I(string.Format("【setorder】 result：{0}。", jsonResult));
            bool result = false;
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString().Equals("0");
                orderid = ja["orderid"].ToString();
            }
            catch { return false; }
            return result;
        }

        /// <summary>
        /// 获取全部分类
        /// </summary>
        /// <returns></returns>
        public static bool getFoodType(out JArray jr)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/getMenuColumn.json", server2);
            AllLog.Instance.I("【getMenuColumn】 start。");
            String jsonResult = Post_Rest(address, null);
            AllLog.Instance.I(string.Format("【getMenuColumn】 result：{0}。", jsonResult));
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
            AllLog.Instance.I(string.Format("【discarddish】 sw：{0}。", sw));
            String jsonResult = Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【discarddish】 result：{0}。", jsonResult));
            bool result = false;
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString().Equals("0");
            }
            catch { return false; }
            return result;
        }

        public static bool verifyuser(string userid)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/verifyuser.json", server2);
            AllLog.Instance.I(string.Format("【verifyuser】 userid：{0}。", userid));
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
            AllLog.Instance.I(string.Format("【verifyuser】 result：{0}。", jsonResult));
            bool result = false;
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString().Equals("0");
            }
            catch { return false; }
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
                            writeCombos(orderid, ref writer, dt, dr["Groupid"].ToString(), ordertype, true);
                            writeCombos(orderid, ref writer, dt, dr["Groupid"].ToString(), ordertype, false);
                            writer.WriteEndArray();
                        }
                        else
                        {
                            //鱼锅
                            dishtype = 1;
                            writer.WritePropertyName("dishes");
                            writer.WriteStartArray();
                            writeDishes(orderid, ref writer, dt, dr["Groupid"].ToString(), ordertype);
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
                    string dishunit = dr["dishunitSrc"].ToString();//dr["dishunit"].ToString();//国际化以后采用原始单位。
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
                    writer.WriteValue(dr["primarykey"].ToString());
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

        private static void writeCombos(string orderid, ref JsonWriter writer, DataTable dt, string groupid, int ordertype, bool isfish)
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
                            writeDishes(orderid, ref writer, dt, groupid2, ordertype);
                            writer.WriteEndArray();
                        }
                        else
                            writer.WriteValue(dishes);
                        string userid = dr["userid"].ToString();
                        writer.WritePropertyName("userName");
                        writer.WriteValue(userid);
                        string dishunit = dr["dishunitSrc"].ToString();//dr["dishunit"].ToString();//国际化以后采用原始单位。
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
                        writer.WriteValue(dr["primarykey"].ToString());
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

        private static void writeDishes(string orderid, ref JsonWriter writer, DataTable dt, string groupid, int ordertype)
        {
            string str0 = "0";
            string str1 = "1";
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Groupid2"].ToString().Equals(groupid))
                {
                    if (!dr["Orderstatus"].ToString().Equals("0"))
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
                        writer.WritePropertyName("dishes");
                        writer.WriteValue(dishes);
                        string userid = dr["userid"].ToString();
                        writer.WritePropertyName("userName");
                        writer.WriteValue(userid);
                        string dishunit = dr["dishunitSrc"].ToString();//dr["dishunit"].ToString();//国际化以后采用原始单位。
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
                        writer.WriteValue(dr["primarykey"].ToString());
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
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/bookorderList.json", server2);
            AllLog.Instance.I(string.Format("【bookorderList】 tableid：{0}，orderid：{1}，sequence：{2}。", tableid, orderid, sequence));
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
            AllLog.Instance.I(string.Format("【bookorderList】 result：{0}。", jsonResult));
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
            return result;
        }

        public static bool getSystemSetData(string settingname, out TSetting setting)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/getSystemSetData.Json", server2);
            AllLog.Instance.I(string.Format("【getSystemSetData】 settingname：{0}。", settingname));
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue(settingname);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【getSystemSetData】 result：{0}。", jsonResult));
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
            return result;
        }

        public static bool getSystemSetData(out TRoundInfo roundJson)
        {
            string address = String.Format("http://{0}/" + apiPath + "/padinterface/getSystemSetData.Json", server2);
            AllLog.Instance.I(string.Format("【getSystemSetData】 settingname：{0}。", "ROUNDING"));
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue("ROUNDING");
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【getSystemSetData】 result：{0}。", jsonResult));
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
            AllLog.Instance.I(string.Format("【cleantable】 tableno：{0}。", tableno));
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("tableNo");
            writer.WriteValue(tableno);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【cleantable】 result：{0}。", jsonResult));
            bool result = false;
            try
            {
                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
                result = ja["result"].ToString().Equals("0");
            }
            catch { return false; }
            return result;
        }

        public static bool getOrderCouponList(string aUserid, string orderid, out JArray jrorder)
        {
            String OrderJson = "";
            JArray jrOrder = null;
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/GetOrderCouponList/{0}/{1}/", orderid, aUserid);
            AllLog.Instance.I(string.Format("【GetOrderCouponList】 orderid：{0}。", orderid));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【GetOrderCouponList】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                jrorder = jrOrder;
                return false;
            }

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            string result = ja["Data"].ToString();
            OrderJson = result;
            OrderJson = OrderJson.Replace("|", "\"");
            try
            {
                jrOrder = (JArray)JsonConvert.DeserializeObject(result);
            }
            catch { }
            jrorder = jrOrder;
            return jrorder != null;
        }

        public static bool getBackDishInfo(string orderid, string dishid, string dishunit, string tableno, out JArray jrorder)
        {
            String OrderJson = "";
            JArray jrOrder = null;
            string ipaddress = GetLocalIp();
            dishunit = dishunit.Replace("#", "&quot"); //DataServer接口不能直接传 '#'，大坑。
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/getBackDishInfo/{0}/{1}/{2}/{3}/", orderid, dishid, dishunit, tableno);
            AllLog.Instance.I(string.Format("【getBackDishInfo】 orderid：{0}，dishid：{1}，tableno：{2}。", orderid, dishid, tableno));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【getBackDishInfo】 result：{0}。", jsonResult));
            if (jsonResult.Equals("0"))
            {
                jrorder = jrOrder;
                return false;
            }

            JObject ja = (JObject)JsonConvert.DeserializeObject(jsonResult);
            string result = ja["Data"].ToString();
            OrderJson = result;
            OrderJson = OrderJson.Replace("|", "\"");
            try
            {
                jrOrder = (JArray)JsonConvert.DeserializeObject(result);
            }
            catch { }
            jrorder = jrOrder;
            return jrorder != null;
        }

        public static bool DeletePosOperation(string tableno)
        {
            string ipaddress = GetLocalIp();
            string address = String.Format("http://" + Server3 + "/datasnap/rest/TServerMethods1/deletePosOperation/{0}", tableno);
            AllLog.Instance.I(string.Format("【deletePosOperation】 tableno：{0}。", tableno));
            String jsonResult = Request_Rest(address);
            AllLog.Instance.I(string.Format("【deletePosOperation】 result：{0}。", jsonResult));
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
            var addr = string.Format("http://{0}/" + apiPath + "/bankinterface/getallbank.json", server);
            List<BankInfo> info = new List<BankInfo>();
            try
            {
                AllLog.Instance.I("【getallbank】 start。");
                string jsonResult = Request_Rest(addr);
                AllLog.Instance.I(string.Format("【getallbank】 result：{0}。", jsonResult));
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

        /// <summary>
        /// 获取所有挂账单位。
        /// </summary>
        /// <returns>Item1返回错误信息，如果正确则返回null；Item2返回数据集合。</returns>
        public static Tuple<string, JArray> GetAllOnAccountCompany()
        {
            var addr = string.Format("http://{0}/" + apiPath + "/padinterface/getCooperationUnit.json", server);
            try
            {
                AllLog.Instance.I("【 getCooperationUnit 】 start。");
                string jsonResult = Post_Rest(addr, null);
                AllLog.Instance.I(string.Format("【 getCooperationUnit 】 result：{0}。", jsonResult));
                return new Tuple<string, JArray>(null, (JArray)JsonConvert.DeserializeObject(jsonResult));
            }
            catch (Exception ex)
            {
                return new Tuple<string, JArray>(ex.Message, null);
            }
        }

        /// <summary>
        /// 重启DataServer接口。
        /// </summary>
        /// <returns></returns>
        public static bool RestartDataserver()
        {
            var addr = string.Format("http://{0}/" + apiPath + "/controller/restartDataserver", server);
            try
            {
                AllLog.Instance.I("【 restartDataserver 】 start。");
                string jsonResult = Post_Rest(addr, null);
                AllLog.Instance.I(string.Format("【 restartDataserver 】 result：{0}。", jsonResult));
                return jsonResult.Equals("0");//返回0表示成功，其他失败。
            }
            catch (Exception ex)
            {
                AllLog.Instance.E("重启DataServer时异常。", ex);
                return false;
            }
        }

        /// <summary>
        /// 检测服务的连接状况。
        /// </summary>
        /// <returns>连接成功返回null，否则返回错误信息。</returns>
        public static string CheckServerConnection()
        {
            var temp = server.Split(':');
            int serverPort = 80;
            var serverIp = temp[0];
            if (temp.Count() > 1)
                serverPort = Convert.ToInt32(temp[1]);

            //先检测门店后台网络连接
            if (!NetworkHelper.DetectIpConnection(serverIp))
            {
                return "后台服务器连接失败，请检查网络连接或后台服务器已经开机。";
            }
            if (!NetworkHelper.DetectNetworkConnection(serverIp, serverPort))
            {
                return "后台服务未启动，请联系管理人员。";
            }

            return null;
        }

        /// <summary>
        /// 检测DataServer服务连接情况。
        /// </summary>
        /// <returns>正常返回null，否则返回错误信息。</returns>
        public static string CheckDataServerConnection()
        {
            var temp = Server3.Split(':');
            int serverPort = 80;
            var serverIp = temp[0];
            if (temp.Count() > 1)
                serverPort = Convert.ToInt32(temp[1]);

            //先检测门店后台网络连接
            if (!NetworkHelper.DetectIpConnection(serverIp))
            {
                return "后台服务器连接失败，请检查网络连接或后台服务器已经开机。";
            }
            if (!NetworkHelper.DetectNetworkConnection(serverIp, serverPort))
            {
                return "DataServer服务未启动，请联系管理人员启动后台服务。";
            }

            return null;
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