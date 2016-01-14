using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using CnSharp.Windows.Updater.Util;
using CnSharp.Windows.Updater.Util.UpdateProxy;

namespace CnSharp.Windows.Updater
{
    public class Common
    {
        public static IUpdateProxy GetUpdateProxy(ReleaseList releaseList)
        {
            //todo:implement it by yourself
            return null;
        }
        #region Public Methods

        public static string GetLocalText(string key)
        {
            const string resourceNameSpace = "CnSharp.Windows.Updater.Language";
            CultureInfo ci = CultureInfo.CurrentCulture;
            string cultureName = ci.Name;
            if (cultureName.Contains("zh-CN"))
            {
                cultureName = "zh-Hans";
            }
            var rm = new ResourceManager(string.Format("{0}.{1}", resourceNameSpace, cultureName), Assembly.GetExecutingAssembly());
            //if (rm == null)
            //     rm = new ResourceManager(string.Format("{0}.en-us", resourceNameSpace), assembly);
            return rm.GetString(key, ci);
        }

        public static void Start(ReleaseList releaseList)
        {
            Start(releaseList, false);
        }
         
        public static void Start(ReleaseList releaseList,bool exception)
        {
            Process.Start(releaseList.ApplicationStart, (exception ? "exception" : "ok"));
        }


        #endregion
    }
}