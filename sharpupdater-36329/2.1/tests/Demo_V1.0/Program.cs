using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ZipUpdater.Monitor.Test
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Process.Start("updater.exe");
                Application.Exit();
                return;
            }
            if (!args[0].StartsWith("ok"))
            {
                Application.Exit();
                return;
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}