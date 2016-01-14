using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CnSharp.Windows.Updater.Util
{
    /// <summary>
    /// manifest of app
    /// </summary>
    public class Manifest
    {
        public const string ManifestFileName = "manifest.xml";

        private List<ReleaseFile> _files;
        private string _releaseNote;
        /// <summary>
        /// entry point of app,old name 'ApplicationStart'
        /// </summary>
        public string EntryPoint { get; set; }
        public string AppName { get; set; }
        public string Company { get; set; }
        public string MinVersion { get; set; }
        public string ReleaseDate { get; set; }
        public string ReleaseUrl { get; set; }
        public string ReleaseVersion { get; set; }
        public string ShortcutIcon { get; set; }
        public bool Packaged { get; set; }
        public string WebSite { get; set; }
        /// <summary>
        /// release note of app,old name 'UpdateDescription'
        /// </summary>
        [XmlIgnore]
        public string ReleaseNote
        {
            get { return _releaseNote; }
            set { _releaseNote = value; }
        }

        [XmlElement("ReleaseNote")] 
        public XmlNode TextCData 
        {
            get
            {
                return new XmlDocument().CreateCDataSection(_releaseNote);
            }
            set { _releaseNote = value.Value; }
        }

        public List<ReleaseFile> Files
        {
            get { return _files ?? (_files = new List<ReleaseFile>()); }
            set { _files = value; }
        }

        public int CompareTo(string version)
        {
            return ReleaseVersion.CompareVersion(version);
        }

    
        public int CompareTo(Manifest otherList)
        {
            if (otherList == null)
                throw new ArgumentNullException("otherList");
            int diff = CompareTo(otherList.ReleaseVersion);
            if (diff != 0)
                return diff;
            return (ReleaseDate == otherList.ReleaseDate)
                       ? 0
                       : (DateTime.Parse(ReleaseDate) > DateTime.Parse(otherList.ReleaseDate) ? 1 : -1);
        }

        public ReleaseFile[] GetDifferences(Manifest otherList, out long fileSize)
        {
            if (otherList.Packaged)
            {
                fileSize = otherList.Files[0].FileSize;
                return new[] { otherList.Files[0] };
            }
            fileSize = 0;
            if (CompareTo(otherList) == 0)
                return null;
            var ht = new Hashtable();
            foreach (var file in _files)
            {
                ht.Add(file.FileName, file.Version);
            }
            var diffrences = new List<ReleaseFile>();
            foreach (var file in otherList.Files)
            {
                if ((!ht.ContainsKey(file.FileName)) || ht[file.FileName] == null ||
                    file.Version.CompareVersion(ht[file.FileName].ToString()) != 0
                    )
                {
                    diffrences.Add(file);
                    fileSize += file.FileSize;
                }
            }
            return diffrences.ToArray();
        }

        public string ZipUrl
        {
            get
            {
                var url = ReleaseUrl;
                if (!url.EndsWith("/"))
                    url += "/";
                return string.Format("{0}{1}_{2}.zip", url, AppName, ReleaseVersion);
            }
        }

        public string GetReleaseFileUrl(string fileName)
        {
            var url = ReleaseUrl;
            if (!url.EndsWith("/"))
                url += "/";
            return string.Format("{0}{1}/{2}", url, ReleaseVersion, fileName);
        }

        public string VersionRoot
        {
            get
            {
                var url = ReleaseUrl;
                if (!url.EndsWith("/"))
                    url += "/";
                return string.Format("{0}{1}/", url,ReleaseVersion);
            }
        }
    }
}
