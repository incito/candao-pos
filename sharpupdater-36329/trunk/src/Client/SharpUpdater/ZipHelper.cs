using System;
using System.IO;
using System.Reflection;

namespace CnSharp.Windows.Updater
{
    public class ZipHelper
    {
        public static void Unzip(string zipFile, string targetFolder)
        {
            var type = Assembly.GetExecutingAssembly().GetType("CnSharp.IO.ZipUtil");
            if (type == null)
            {
                var ass = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "CnSharp.ZipUtil.dll"));
                type = ass.GetType("CnSharp.IO.ZipUtil");
            }
            var m = type.GetMethod("Unzip");
            m.Invoke(null, new object[] { zipFile, targetFolder });
        }
    }
}