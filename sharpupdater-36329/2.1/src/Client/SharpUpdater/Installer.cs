using System;
using System.IO;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Windows.Updater
{
    public class Installer
    {
        private delegate void UnzipDelegate(string zipPath,string folder);

        public event EventHandler  ZipCompleted;

        public  void Install(string tempDir, string appDir,ReleaseList releaseList)
        {
            if (releaseList.Packaged)
            {
                //var d = new UnzipDelegate(ZipHelper.Unzip);
                //d.BeginInvoke(Path.Combine(tempDir, releaseList.Files[0].FileName), appDir, DoZipCompleted, null);
                //ZipHelper.Unzip(Path.Combine(tempDir, releaseList.Files[0].FileName), appDir);
                ZipHelper.Unzip(Path.Combine(tempDir, releaseList.Files[0].FileName), appDir);
            }
            else
            {
                FileUtil.CopyFiles(tempDir, appDir);
            }
        }

         private void DoZipCompleted(IAsyncResult asyncResult)
         {
            if(asyncResult.IsCompleted && ZipCompleted != null)
                ZipCompleted(null, EventArgs.Empty);
         }
    }
}