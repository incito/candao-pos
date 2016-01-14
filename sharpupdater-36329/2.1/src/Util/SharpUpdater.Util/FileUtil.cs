using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace CnSharp.Windows.Updater.Util
{
    public static class FileUtil
    {
        public static ReleaseList ReadReleaseList(string fileName)
        {
            //if (!File.Exists(fileName))
            //    return null;
            return XmlSerializerHelper.LoadObjectFromXml<ReleaseList>(fileName);
        }

        public static void SaveReleaseList(ReleaseList list, string fileName)
        {
            string xml = XmlSerializerHelper.GetXmlStringFromObject(list);
            //WriteText(fileName, xml, Encoding.UTF8);
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var dir = Path.GetDirectoryName(fileName);
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            doc.Save(fileName);
        }

        /// <summary>
        /// 写文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="coding"></param>
        public static void WriteText(string path, string content, Encoding coding)
        {
            var dir = Path.GetDirectoryName(path);
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            using (var sw = new StreamWriter(path, false, coding))
                sw.Write(content);
        }

        /// <summary>
        /// 读文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="coding"></param>
        /// <returns></returns>
        public static string ReadText(string path, Encoding coding)
        {
            using (var sr = new StreamReader(path, coding))
                return sr.ReadToEnd();
        }

        public static void CopyFiles(string sourceDirectory, string targetDirectory)
        {
            if (!Directory.Exists(sourceDirectory)) return;
            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            string[] directories = Directory.GetDirectories(sourceDirectory);
            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyFiles(d, targetDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }

            string[] files = Directory.GetFiles(sourceDirectory);
            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, targetDirectory + s.Substring(s.LastIndexOf("\\")), true);
                }
            }
        }

        public static void DeleteFolder(string path)
        {
            DeleteDirectory(new DirectoryInfo(path));
        }

        public static void DeleteDirectory(DirectoryInfo dir)
        {
            if (dir.Exists)
            {
                DirectoryInfo[] childs = dir.GetDirectories();
                foreach (DirectoryInfo child in childs)
                {
                    DeleteDirectory(child);
                }
                dir.Delete(true);
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5(string input)
        {
            var md5 = new MD5CryptoServiceProvider();

            byte[] inBytes = Encoding.Default.GetBytes(input);

            byte[] outBytes = md5.ComputeHash(inBytes);

            var output = new StringBuilder();

            for (int i = 0; i < outBytes.Length; i++)
            {
                output.Append(outBytes[i].ToString("x2"));
            }

            return output.ToString();
        }
    }
}