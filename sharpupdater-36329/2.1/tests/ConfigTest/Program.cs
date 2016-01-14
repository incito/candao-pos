using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CnSharp.Windows.Updater.SharpPack;
using CnSharp.Windows.Updater.Util;

namespace ConfigTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            var reList = new ReleaseList { AppName = "Demo", ReleaseVersion = "1.0", Company = "CnSharp",ApplicationStart = "demo.exe"};
            var dir = @"C:\Users\Jeremy Chin\Dev\Studio\OpenSource\SharpUpdater\2.1\tests\Demo_V1.0\bin\Debug\";
            _rootLength = dir.Length;

            var files = GatherFiles(dir);
            var f =
                new ReleaseForm(
                    dir,
                    files,
                  reList
                    );
            Application.Run(f);
        }

        private static int _rootLength;
         static List<FileListItem> GatherFiles(string dir)
        {
            _rootLength = dir.Length;
            return GatherFilesInFolder(dir, true);
        }

        static List<FileListItem> GatherFilesInFolder(string dir, bool isFirst)
        {
            var list = new List<FileListItem>();
            var dirShortName = dir.Trim('\\').Split('\\').Last();
            var folderExcluded = false;
            if (!isFirst)
            {
                var folderItem = new FileListItem { Dir = dir, Selected = !folderExcluded };
                list.Add(folderItem);
            }

            var files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                string shortName = dir.Substring(_rootLength);
                var selected = true;
                list.Add(new FileListItem
                {
                    Dir = file,
                    IsFile = true,
                    Selected = selected
                });
             }

            string[] folders = Directory.GetDirectories(dir);
            foreach (string folder in folders)
            {
                list.AddRange(GatherFilesInFolder(folder, false).ToArray());
            }
            return list;
        } 

    }
}
