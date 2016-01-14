using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using CnSharp.Windows.Updater.Util;

namespace PackingConsole
{
    class ManifestBuilder
    {
        private readonly string _entryExePath;
        private readonly string _webSite;
        private readonly string _iconPath;
        private readonly string _releaseUrl;
        private readonly Assembly _assembly;
        private readonly Manifest _productManifest;
        private readonly IReleaseNoteLoader _releaseNoteLoader;

        public ManifestBuilder(string entryExePath, string releaseUrl, IReleaseNoteLoader releaseNoteLoader ,string webSite, string iconPath )
        {
            _entryExePath = entryExePath;
            _webSite = webSite;
            _iconPath = iconPath;
            _releaseUrl = releaseUrl;
            _assembly = Assembly.LoadFile(_entryExePath);
            _releaseNoteLoader = releaseNoteLoader;
            try
            {
                _productManifest = FileUtil.ReadManifest(releaseUrl + "/" + Manifest.ManifestFileName);

            }
            catch
            {
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(releaseUrl + "/ReleaseList.xml");
                    doc.InnerXml =
                        doc.InnerXml.Replace("ReleaseList", "Manifest")
                            .Replace("ApplicationStart", "EntryPoint")
                            .Replace("UpdateDescription", "ReleaseNote");
                    _productManifest = XmlSerializerHelper.LoadObjectFromXmlString<Manifest>(doc.InnerXml);
                }
                catch
                {
                    _productManifest = null;
                }
                _productManifest = null;
            }
        }

        public Manifest BuildManifest()
        {
            var ver = FileVersionInfo.GetVersionInfo(_entryExePath); //_assembly.GetName().Version.ToString();
            var manifest = new Manifest
            {
                AppName = ver.ProductName,
                Company = ver.CompanyName,
                ReleaseVersion = ver.ProductVersion,
                MinVersion = ver.ProductVersion,
                ReleaseDate = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                EntryPoint = Path.GetFileName(_entryExePath),
                ReleaseUrl = _releaseUrl,
                ShortcutIcon = _iconPath,
                WebSite = _webSite,
                ReleaseNote = _releaseNoteLoader.GetNote()
            };
            var dir = Path.GetDirectoryName(_entryExePath);
            manifest.Files = GatherFilesInFolder(dir, manifest).ToList();
            if (string.IsNullOrEmpty(_iconPath) || !File.Exists(Path.Combine(dir, _iconPath)))
                manifest.ShortcutIcon = GetIconFileName(dir);
            return manifest;
        }

        private string GetIconFileName(string dir)
        {
            var files = Directory.GetFiles(dir, "*.ico");
            if (files.Length == 0)
                return string.Empty;
            var file =  files.Select(f => new FileInfo(f))
                .OrderByDescending(m => m.LastWriteTime)
                .FirstOrDefault();
            return file == null ? string.Empty :
                file.FullName.Substring(dir.Length);
        }

        protected virtual bool IsFileExcluded(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            var fileName = Path.GetFileName(file).ToLower();

            return (ext == ".pdb" || ext == ".log" || file.Contains(".vshost.") || fileName == "updater.exe" ||
                    fileName == "manifest.xml" || fileName == "releaselist.xml");
        }


        private IEnumerable<ReleaseFile> GatherFilesInFolder(string dir,Manifest manifest)
        {
            string[] files = Directory.GetFiles(dir,"*.*",SearchOption.AllDirectories);
            return from file in files where !IsFileExcluded(file) select (new ReleaseFile
            {
                FileName = file.Substring(dir.Length+1),
                FileSize = new FileInfo(file).Length,
                Version =  new VersionNumberGenerator(file,_productManifest).GetVersionNumber() ?? manifest.ReleaseVersion
            });
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
    }
}