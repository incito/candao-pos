using System.Collections.Generic;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Windows.Updater.Service.API.UpdateLogStorage
{
    public interface IUpdateLogStorage
    {
        void Write(string clientId, UpdateLog log);
        List<UpdateLog> All(string clientId);
        List<UpdateLog> GetBetweenVersion(string clientId, string baseVersion, string topVersion);
        //string GetLatestVersion(string clientId);
    }
}