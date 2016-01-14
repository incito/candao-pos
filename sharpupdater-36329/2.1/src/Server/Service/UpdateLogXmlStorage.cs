using System.Collections.Generic;
using System.IO;
using System.Text;
using CnSharp.Windows.Updater.Service.API.UpdateLogStorage;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Windows.Updater.Service.Hosting
{
    public class UpdateLogXmlStorage : IUpdateLogStorage
    {
        private static readonly string _xmlPath = System.Web.HttpContext.Current.Server.MapPath("/{0}/updatelog.xml");
        public void Write(string clientId, UpdateLog log)
        {
            var path = string.Format(_xmlPath, clientId);
            var dir = Path.GetDirectoryName(path);
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var currentLogs = new List<UpdateLog>();
            if (File.Exists(path))
            {
                currentLogs = XmlSerializerHelper.LoadObjectFromXml<List<UpdateLog>>(path);
                if (currentLogs.Count > 0)
                {
                    var diff = ReleaseList.CompareVersion(currentLogs[0].Version, log.Version);
                    if(diff > 0)
                        return;
                    if(diff == 0)
                        currentLogs[0].Description = log.Description;
                     else
                        currentLogs.Insert(0, log);
                }
                else
                {
                    currentLogs.Insert(0, log);
                }
            }
            else
                 currentLogs.Insert(0,log);
            var xml = XmlSerializerHelper.GetXmlStringFromObject(currentLogs);
            FileUtil.WriteText(path, xml, Encoding.UTF8);
        }

        public List<UpdateLog> All(string clientId)
        {  
              var path = string.Format(_xmlPath, clientId);
              try
              {
                  return XmlSerializerHelper.LoadObjectFromXml<List<UpdateLog>>(path);
              }
              catch
              {
                  return null;
              }
        }

        public List<UpdateLog> GetBetweenVersion(string clientId, string baseVersion, string topVersion)
        {
            var path = string.Format(_xmlPath, clientId);
            try
            {
                var logs = XmlSerializerHelper.LoadObjectFromXml<List<UpdateLog>>(path);
                var index = logs.FindIndex(log => (log.Version == baseVersion));
                if (index >= 0)
                {
                    var count = logs.Count;
                    logs.RemoveRange(index, count - index);
                }
                index = logs.FindIndex(log => (log.Version == topVersion));
                if (index > 0)
                {
                    logs.RemoveRange(0,index);
                }
                return logs;
            }
            catch
            {
                return null;
            }
        }


        public string GetLatestVersion(string clientId)
        {
            var path = string.Format(_xmlPath, clientId);
            try
            {
                var logs = XmlSerializerHelper.LoadObjectFromXml<List<UpdateLog>>(path);
                return logs[0].Version;
            }
            catch
            {
                return null;
            }
        }
    }
}