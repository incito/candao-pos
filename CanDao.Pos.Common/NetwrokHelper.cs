using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace CanDao.Pos.Common
{
    /// <summary>
    /// 网络辅助类。
    /// </summary>
    public class NetwrokHelper
    {
        /// <summary>
        /// 检测与某个网络地址是否连通。
        /// </summary>
        /// <param name="ipAddr">IP地址。如192.168.1.1，或者是网站地址如：www.baidu.com。</param>
        /// <returns>如果连通返回true，否则返回false。</returns>
        public static bool DetectNetworkConnection(string ipAddr)
        {
            if (ipAddr.Contains("/newspicyway"))
            {
                var temp = ipAddr.Replace("/newspicyway", "");
                ipAddr = temp;
            }
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions {DontFragment = true};
            byte[] buffer = Encoding.UTF8.GetBytes("");
            int timeout = 2000; //响应时间，毫秒
            int lastIdx = ipAddr.LastIndexOf(":", StringComparison.Ordinal);
            try
            {
                if (lastIdx > 5) //排除http:和https:
                {
                    var portString = ipAddr.Substring(lastIdx + 1, ipAddr.Length - lastIdx - 1);
                    TcpClient tcp = new TcpClient(ipAddr.Remove(lastIdx), Convert.ToInt32(portString));
                    tcp.GetStream();
                    tcp.Close();
                    return true;
                }

                var reply = pingSender.Send(ipAddr, timeout, buffer, options);
                if (reply != null)
                {
                    var info = reply.Status.ToString();
                    return info.Equals("Success");
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}