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
        /// 地址字典。
        /// </summary>
        private static readonly Dictionary<string, string> AddrDic = new Dictionary<string, string>();

        /// <summary>
        /// 获取指定名称的服务地址。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetServiceAddr(string name)
        {
            if (AddrDic.ContainsKey(name))
                return AddrDic[name];
            return null;
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
                            var uri = string.Format("http://{0}{1}", SystemConfigCache.JavaServer, addr);
                            AddrDic.Add(element.Name.LocalName, uri);
                        }
                    }

                    var dataServerElement = root.Element("DataServerApi");
                    if (dataServerElement != null)
                    {
                        foreach (var element in dataServerElement.Elements())
                        {
                            var addr = XmlHelper.GetAttrValue(element, "Addr");
                            var uri = string.Format("http://{0}{1}", SystemConfigCache.DataServer, addr);
                            AddrDic.Add(element.Name.LocalName, uri);
                        }
                    }

                    var cloudServerElement = root.Element("CloudServerApi");
                    if (cloudServerElement != null)
                    {
                        foreach (var element in cloudServerElement.Elements())
                        {
                            var addr = XmlHelper.GetAttrValue(element, "Addr");
                            var uri = string.Format("http://{0}{1}", SystemConfigCache.CloudServer, addr);
                            AddrDic.Add(element.Name.LocalName, uri);
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