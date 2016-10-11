using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using JunLan.Common.Base;

namespace CanDao.Pos.Common
{
    public static class ServiceAddrCache
    {
        private const string CfgFile = @"Config\ServiceAddr.xml";

        /// <summary>
        /// Java后台地址字典。
        /// </summary>
        private static readonly  Dictionary<string, string> JavaAddrDic = new Dictionary<string, string>();
 
        /// <summary>
        /// 餐道会员地址字典。
        /// </summary>
        private static readonly  Dictionary<string, string> CloundAddrDic = new Dictionary<string, string>();
 
        /// <summary>
        /// 雅座会员地址字典。
        /// </summary>
        private static readonly Dictionary<string, string> YaZuoAddrDic = new Dictionary<string, string>(); 

        /// <summary>
        /// 获取指定名称的服务地址。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetServiceAddr(string name)
        {
            string addr = null;
            if (JavaAddrDic.ContainsKey(name))
                addr = string.Format("{0}{1}", SystemConfigCache.JavaServer, JavaAddrDic[name]);

            if(YaZuoAddrDic.ContainsKey(name))
                addr = string.Format("{0}{1}", Globals.YaZuoServer, YaZuoAddrDic[name]);

            if (CloundAddrDic.ContainsKey(name))
                addr = string.Format("{0}{1}", Globals.CloudServer, CloundAddrDic[name]);

            if (!string.IsNullOrEmpty(addr) && !addr.StartsWith("http://"))
                addr = string.Format("http://{0}", addr);

            return addr;
        }

        public static void LoadCfgFile()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CfgFile);
            if (File.Exists(file))
            {
                try
                {
                    var xDoc = XDocument.Load(file);
                    var root = xDoc.Root;
                    if (root== null)
                        return;

                    var javaServerElement = root.Element("JavaServerApi");
                    if (javaServerElement != null)
                    {
                        foreach (var element in javaServerElement.Elements())
                        {
                            var addr = XmlHelper.GetAttrValue(element, "Addr");
                            JavaAddrDic.Add(element.Name.LocalName, addr);
                        }
                    }

                    var yaZuoServerElement = root.Element("YaZuoApi");
                    if (yaZuoServerElement != null)
                    {
                        foreach (var element in yaZuoServerElement.Elements())
                        {
                            var addr = XmlHelper.GetAttrValue(element, "Addr");
                            YaZuoAddrDic.Add(element.Name.LocalName, addr);
                        }
                    }

                    var cloudServerElement = root.Element("CloudServerApi");
                    if (cloudServerElement != null)
                    {
                        foreach (var element in cloudServerElement.Elements())
                        {
                            var addr = XmlHelper.GetAttrValue(element, "Addr");
                            CloundAddrDic.Add(element.Name.LocalName, addr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrLog.Instance.E(ex);
                }
            }
            else
            {
                ErrLog.Instance.E("服务地址配置文件不存在！");
            }
        }
    }
}