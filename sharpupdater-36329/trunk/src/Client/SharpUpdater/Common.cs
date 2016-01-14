using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Windows.Updater
{
    public class Common
    {
        private static  string[] _ignoreFiles;

        public static string[] IgnoreFiles
        {
            get
            {
                if (_ignoreFiles == null)
                {
#if DEBUG
                    _ignoreFiles = new []
        {
             "Updater.exe",
            "CnSharp.ZipUtil.dll",
            "SharpUpdater.Util.dll",
            "ICSharpCode.SharpZipLib.dll"
        };
#else
                    _ignoreFiles = new []
        {
             "Updater.exe"
        };
#endif
                }
                return _ignoreFiles;
            }
        } 
        public static IUpdatesChecker GetUpdateProxy(Manifest manifest)
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

        public static void Start(Manifest manifest)
        {
            Start(manifest, false);
        }
         
        public static void Start(Manifest manifest,bool exception)
        {
            Process.Start(manifest.EntryPoint, (exception ? "exception" : "ok"));
        }


        #endregion
    }
}