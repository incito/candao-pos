using System;
using System.IO;
using CnSharp.Windows.Updater.Util;

namespace PackingConsole
{
    class Program
    {
        /// <summary>
        /// entry point
        /// </summary>
        /// <param name="args">
        /// 0 entry point .exe path
        /// 1 release host root  url
        /// 2 release note url
        /// 3 web site url
        /// 4 ico file path(optional)
        /// </param>
        /// <returns></returns>
        /// <example>
        /// C:\Users\xxx\wms.exe http://10.10.11.23:8023/wms/ http://jira.cnsharp.org/jira/browse/WMS?selectedTab=com.atlassian.jira.plugin.system.project%3Aroadmap-panel http://wms.cnsharp.org wms.ico 1  
        /// </example>
        static int Main(string[] args)
        {
            if (args == null || args.Length < 4)
            {
                Console.WriteLine("输入参数不正确！");
                return -1;
            }

            string entryExePath,  releaseUrl, webSite, iconPath = null;
            IReleaseNoteLoader releaseNoteLoader;
            entryExePath = args[0];
            releaseUrl = args[1];
            releaseNoteLoader = new JiraReleaseNoteLoader(args[2]);
            webSite = args[3];
            if (args.Length > 4)
                iconPath = args[4];

            var builder = new ManifestBuilder(entryExePath, releaseUrl, releaseNoteLoader, webSite, iconPath);
            var manifest = builder.BuildManifest();
            var dir = Path.GetDirectoryName(entryExePath);
            manifest.SaveManifest(Path.Combine(dir,Manifest.ManifestFileName));
            return 0;
        }

    }
}
