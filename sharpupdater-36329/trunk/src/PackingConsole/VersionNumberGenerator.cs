using System.Diagnostics;
using System.IO;
using System.Net;
using CnSharp.Windows.Updater.Util;

namespace PackingConsole
{
    class VersionNumberGenerator
    {
        private readonly string _file;
        private readonly Manifest _productManifest;


        public VersionNumberGenerator(string file, Manifest productManifest)
        {
            _file = file;
            _productManifest = productManifest;
        }

        public string GetVersionNumber()
        {
            var fi = new FileInfo(_file);
            var ext = fi.Extension.ToLower();
            if (ext == ".exe" || ext == ".dll")
            {
                return FileVersionInfo.GetVersionInfo(_file).FileVersion;
            }
            if (_productManifest == null)
                return null;
            foreach (var releaseFile in _productManifest.Files)
            {
                if (_file.ToLower().EndsWith(releaseFile.FileName.ToLower()))
                {
                    if (fi.Length != releaseFile.FileSize)
                        return null;
                    if (!FileComparor.IsSame(_file,DownloadReleaseFile(releaseFile)))
                    {
                        return null;
                    }
                    return releaseFile.Version;
                }
            }
            return null;
        }

        private string DownloadReleaseFile(ReleaseFile releaseFile)
        {
            var fileName = Path.Combine(Path.GetTempPath(), "PackingConsole","Cache",_productManifest.AppName,
                _productManifest.ReleaseVersion,releaseFile.FileName);
            if (File.Exists(fileName))
                return fileName;
            using (var wc = new WebClient())
            {
                var dir = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                wc.DownloadFile(_productManifest.GetReleaseFileUrl(releaseFile.FileName),fileName);
            }
            return fileName;
        }
    }
}