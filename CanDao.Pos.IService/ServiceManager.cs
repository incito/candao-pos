using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using JunLan.Common.Base;

namespace CanDao.Pos.IService
{
    public class ServiceManager
    {
        #region Fields

        private const string CfgFile = @"Config\ServiceInterfaceCfg.xml";

        /// <summary>
        /// 接口名与实例Dll的对应关系。
        /// </summary>
        private readonly Dictionary<string, ServiceInterfaceInfo> _serviceInterfaceDic;

        #endregion

        #region Single Instance

        private static ServiceManager _instance;
        private static readonly object LockObj = new object();
        private ServiceManager()
        {
            _serviceInterfaceDic = new Dictionary<string, ServiceInterfaceInfo>();
            Init();
        }

        /// <summary>
        /// 获取服务管理类的唯一实例。
        /// </summary>
        public static ServiceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ServiceManager();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 获取服务对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetServiceIntance<T>()
        {
            var interfaceName = typeof(T).Name;
            if (!_serviceInterfaceDic.ContainsKey(interfaceName))
            {
                ErrLog.Instance.E("接口名为：\"{0}\"的接口没有在接口配置文件中配置相关信息！", interfaceName);
                return default(T);
            }

            try
            {
                ServiceInterfaceInfo info = _serviceInterfaceDic[interfaceName];
                AllLog.Instance.D("CreateObject {0} : {1} - {2}", info.Name, info.DllName, info.ClassName);

                string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, info.DllName);
                Assembly assembly = Assembly.LoadFrom(dllPath);
                if (assembly == null)
                {
                    ErrLog.Instance.E("创建程序集\"{0}\"失败！", dllPath);
                    return default(T);
                }

                object obj = assembly.CreateInstance(info.ClassName);
                if (obj == null)
                {
                    ErrLog.Instance.E("创建程序集\"{0}\"下的\"{1}\"实例失败！", dllPath, info.ClassName);
                    return default(T);
                }

                return (T)obj;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("创建服务\"{0}\"失败，失败原因：{1}", interfaceName, ex.Message);
                return default(T);
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// 初始化数据，从配置文件读取信息。
        /// </summary>
        private void Init()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CfgFile);
            if (!File.Exists(file))
            {
                AllLog.Instance.F("服务接口配置文件不存在！{0}", file);
                return;
            }

            XDocument xDoc = XDocument.Load(file);
            foreach (var xElement in xDoc.Root.Elements())
            {
                try
                {
                    string name = XmlHelper.GetAttrValue(xElement, "Name");
                    string dllName = XmlHelper.GetAttrValue(xElement, "DllName");
                    string className = XmlHelper.GetAttrValue(xElement, "ClassName");
                    var info = new ServiceInterfaceInfo(name, dllName, className);
                    _serviceInterfaceDic.Add(name, info);
                }
                catch (Exception exp)
                {
                    ErrLog.Instance.E(exp);
                }
            }
        }

        #endregion
    }
}