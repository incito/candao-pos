using System.IO;

namespace CnSharp.Windows.Updater.Util
{
    public class Monitor
    {
        public static ReleaseList GetNewVersion(string appDir)
        {
            string localXmlPath = Path.Combine(appDir, Constants.ReleaseConfigFileName);
            ReleaseList localRelease = FileUtil.ReadReleaseList(localXmlPath);
            return FileUtil.ReadReleaseList(localRelease.ReleaseUrl);
        }

        public static void Download(string baseUrl, string[] files, string saveDir)
        {
            foreach (string file in files)
            {
                DownloadUtil.DownloadFile(saveDir, baseUrl, file);
            }
        }
    }
}