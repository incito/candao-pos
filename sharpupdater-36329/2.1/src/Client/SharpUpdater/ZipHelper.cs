using System;
using System.IO;
using System.Reflection;

namespace CnSharp.Windows.Updater
{
    public class ZipHelper
    {
        public static void Unzip(string zipFile, string targetFolder)
        {
            Assembly ass = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "CnSharp.ZipUtil.dll"));
            Type type = ass.GetType("CnSharp.IO.ZipUtil");
            MethodInfo m = type.GetMethod("Unzip");
            m.Invoke(null, new object[] {zipFile, targetFolder});
        }
    }
}