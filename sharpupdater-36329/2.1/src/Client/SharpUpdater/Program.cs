using System;
using System.Windows.Forms;

namespace CnSharp.Windows.Updater
{
    internal static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args != null && args.Length > 0)
            {
                string tempDir = args[0];
                string newVersionXmlPath = "";
                if (args.Length > 1)
                {
                    newVersionXmlPath = args[1];
                }
                var installForm = new InstallForm(tempDir, newVersionXmlPath);
                Application.Run(installForm);
                return;
            }
            var connForm = new ConnectionForm();
            Application.Run(connForm);
        }

        #endregion
    }
}