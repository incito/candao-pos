using System;
using System.IO;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Windows.Updater
{
    public class Installer
    {
        private delegate void UnzipDelegate(string zipPath,string folder);

        public event EventHandler  ZipCompleted;

        public  void Install(string tempDir, string appDir,Manifest manifest,string[] ignoreFiles)
        {
            if (File.Exists(tempDir))
            {
                if(tempDir.ToLower().EndsWith(".zip"))
                    Unzip(tempDir,appDir,ignoreFiles);
                else
                {
                    ClearIgnoreFiles(tempDir,ignoreFiles);
                    File.Move(tempDir,Path.Combine(appDir,Path.GetFileName(tempDir)));
                }
                return;
            }
            if (manifest != null && manifest.Packaged)
            {
                 Unzip(Path.Combine(tempDir, manifest.Files[0].FileName), appDir,ignoreFiles);
            }
            else
            {
                ClearIgnoreFiles(tempDir, ignoreFiles);
                FileUtil.CopyFiles(tempDir, appDir);
            }
        }

        private void Unzip(string zipFile, string appDir, string[] ignoreFiles)
        {
            if (ignoreFiles == null || ignoreFiles.Length == 0)
            {
                ZipHelper.Unzip(zipFile, appDir);
                return;
            }
            var dir = Path.GetDirectoryName(zipFile);
            dir += "\\" + Guid.NewGuid();
            ZipHelper.Unzip(zipFile, dir);
            ClearIgnoreFiles(dir, ignoreFiles);
            FileUtil.CopyFiles(dir,appDir);
        }

        private void ClearIgnoreFiles(string dir, string[] ignoreFiles)
        {
            if (ignoreFiles == null)
                return;
            foreach (var ignoreFile in ignoreFiles)
            {
                var delFile = Path.Combine(dir, ignoreFile);
                if (File.Exists(delFile))
                    File.Delete(delFile);
            }
        }

         private void DoZipCompleted(IAsyncResult asyncResult)
         {
            if(asyncResult.IsCompleted && ZipCompleted != null)
                ZipCompleted(null, EventArgs.Empty);
         }
    }
}