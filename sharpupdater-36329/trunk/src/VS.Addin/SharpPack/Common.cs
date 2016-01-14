using System.Reflection;
using System.Windows.Forms;
using CnSharp.Windows.Updater.Util;
using EnvDTE;

namespace CnSharp.Delivery.VisualStudio.PackingTool
{
    public  class Common
    {
        public const string ProductName = "SharpPack";
        public const string ZipExt = ".zip";
        public const string ManifestFileName = "Manifest.xml";
        private static string hostDir;

        public static Logger MyLogger
        {
            get
            {
                return new Logger(GetHostDir()+ "\\_Logs\\");
            }
        }

        public static string GetHostDir()
        {
            if(!string.IsNullOrEmpty(hostDir))
                return hostDir;
            hostDir = Assembly.GetExecutingAssembly().Location;
            hostDir = hostDir.Substring(0, hostDir.LastIndexOf('\\'));
            return hostDir;
        }

        public static string GetAssemblyName(Project project)
        {
            for (int i = 1; i <= project.Properties.Count; i++)
            {
                Property property = project.Properties.Item(i);
                if (property.Name == "AssemblyName")
                    return property.Value.ToString();
            }
            return string.Empty;
        }

        public static DialogResult ShowError(string message)
        {
            return MessageBox.Show(message, ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}