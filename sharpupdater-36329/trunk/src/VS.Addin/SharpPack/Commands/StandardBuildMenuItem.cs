using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CnSharp.Delivery.VisualStudio.PackingTool.Utils;
using CnSharp.Windows.Updater.Util;
using EnvDTE;
using EnvDTE80;
using VSLangProj80;
using Process = System.Diagnostics.Process;

namespace CnSharp.Delivery.VisualStudio.PackingTool.Commands
{
    public class StandardBuildMenuItem : CommandMenuItem,IBuildCommand
    {
        protected string BuildDir;
        protected CacheHelper<ProjectCache> CacheHelper;
        protected Manifest Manifest;
        protected Project Project;
        protected ProjectCache ProjectCache;
        protected Solution2 Solution;
        protected SolutionBuild2 SolutionBuild;
        protected VSProject2 VsProject;
        private ProductInfo _productToRelease;
        private int _rootLength;

        public StandardBuildMenuItem()
        {
            ProjectNodeName = "Project";
            CommandName = "PackagePublish";
            MenuText = "Package and Publish";
            BuildSuccessText = "Build Successfully.";
            FaceId = 2171;
        }

        public string BuildSuccessText { get; set; }
        protected bool ZipOnly { get; set; }

        protected virtual bool ExecuteBeforeBuild()
        {
            var projects = new List<Project>();
            foreach (Reference3 r in VsProject.References)
            {
                if (r.SourceProject != null)
                    projects.Add(r.SourceProject);
            }
            var fv = new VersionEditForm(Project, projects);
            bool ok = (fv.ShowDialog() == DialogResult.OK);
            if (ok)
                _productToRelease = fv.ProductToRelease;
            return ok;
        }

        protected virtual bool IsFileExcluded(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            var fileName = Path.GetFileName(file).ToLower();

            return (ext == ".pdb" || ext == ".log" || file.Contains(".vshost.") || fileName == "updater.exe" ||
                    fileName == "manifest.xml" || fileName == "releaselist.xml");
        }

        protected virtual bool IsFileUnselected(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            return (ext == ".xml" && File.Exists(file.Substring(0, file.Length - 4) + ".dll"));
        }

        protected virtual List<FileListItem> GatherFiles(string dir)
        {
            _rootLength = dir.Length;
            return GatherFilesInFolder(dir, true);
        }

        private List<FileListItem> GatherFilesInFolder(string dir, bool isFirst)
        {
            var list = new List<FileListItem>();
            string dirShortName = dir.Substring(_rootLength);
            bool folderExcluded = ProjectCache != null && ProjectCache.ExcludeFolders.Contains(dirShortName);
            if (!isFirst)
            {
                var folderItem = new FileListItem {Dir = dir, Selected = !folderExcluded};
                list.Add(folderItem);
            }

            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                if (!IsFileExcluded(file))
                {
                    string shortName = dir.Substring(_rootLength);
                    bool selected = !folderExcluded &&
                                    (ProjectCache == null || !ProjectCache.ExcludeFiles.Contains(shortName))
                                    && !IsFileUnselected(file);
                    list.Add(new FileListItem
                    {
                        Dir = file,
                        IsFile = true,
                        Selected = selected
                    });
                }
            }

            string[] folders = Directory.GetDirectories(dir);
            foreach (string folder in folders)
            {
                list.AddRange(GatherFilesInFolder(folder, false).ToArray());
            }
            return list;
        }

        protected virtual bool BuildManifest()
        {
            BuildDir = Path.GetDirectoryName(Project.FullName) + "\\bin\\" + SolutionBuild.ActiveConfiguration.Name +
                       "\\";
            string cacheDir = Common.GetHostDir() + "\\_Projects\\" + Project.FullName.MD5() + "\\project.xml";
            CacheHelper = new CacheHelper<ProjectCache>();
            try
            {
                ProjectCache = CacheHelper.Get(cacheDir);
            }
            catch
            {
                ProjectCache = new ProjectCache();
            }

            if (_productToRelease == null)
            {
                _productToRelease = ProjectUtil.GetProductInfo(Project);
            }
            List<FileListItem> files = GatherFiles(BuildDir);
            ////string releaseFileName = Path.GetDirectoryName(prj.FullName) + "\\" + Common.ManifestFileName;

            string releaseFileName = Path.GetDirectoryName(Project.FullName) + "\\bin\\" + Common.ManifestFileName;

            if (File.Exists(releaseFileName))
                Manifest = FileUtil.ReadManifest(releaseFileName);
            if (Manifest == null)
            {
                string[] icons = Directory.GetFiles(BuildDir, "*.ico");
                Manifest = new Manifest
                {
                    AppName = _productToRelease.Name,
                    Company = _productToRelease.CompanyName,
                    EntryPoint = Common.GetAssemblyName(Project) + ".exe",
                    ShortcutIcon = icons.Length > 0 ? Path.GetFileName(icons[0]) : ""
                };
            }
            Manifest.ReleaseVersion = _productToRelease.Version;
            Manifest.MinVersion = _productToRelease.Version;

            var f = new ReleaseForm(BuildDir, files, Manifest);
            f.ZipOnly = ZipOnly;
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (ProjectCache == null)
                    ProjectCache = new ProjectCache();
                ProjectCache.ExcludeFiles = f.GetExcludedFiles().ToList();
                ProjectCache.ExcludeFolders = f.GetExcludedFolders().ToList();
                CacheHelper.Save(ProjectCache, cacheDir);
                return true;
            }
            return false;
        }


        protected virtual void ExecuteAfterRelease()
        {
            var di = new DirectoryInfo(BuildDir);
            OutputMessage(string.Format("{0}:{1}", BuildSuccessText, di.Parent.FullName) + Environment.NewLine);
            Process.Start(di.Parent.FullName);
        }

        public void OutputMessage(string outputStr)
        {
            //get output window
            OutputWindow ow = Dte.ToolWindows.OutputWindow;
            //create own pane type
            OutputWindowPane outputPane = null;
            foreach (OutputWindowPane pane in ow.OutputWindowPanes)
            {
                if (pane.Name == CommandName)
                {
                    outputPane = pane;
                    break;
                }
            }
            if (outputPane == null)
                outputPane = ow.OutputWindowPanes.Add(CommandName);
            //output message
            outputPane.OutputString(outputStr);
            outputPane.Activate();
        }

        public override bool Execute()
        {
            return Build();
        }

        public virtual bool Build()
        {
            try
            {
                //  _applicationObject.ExecuteCommand("Build.BuildSelection", "");
                Project = (Project)((Array)Dte.ActiveSolutionProjects).GetValue(0);
                VsProject = ((VSProject2)(Project.Object));

                if (!ExecuteBeforeBuild())
                    return false;

                Solution = (Solution2)Dte.Solution;
                SolutionBuild = (SolutionBuild2)Solution.SolutionBuild;

                //sb2.BuildProject(sb2.ActiveConfiguration.Name, prj.UniqueName, true);
                SolutionBuild.Build(true);
                //_dte.ExecuteCommand("Build.RebuildSolution");

                if (SolutionBuild.LastBuildInfo != 0)
                {
                    return false;
                }

                if (!BuildManifest())
                    return false;

                ExecuteAfterRelease();
                return true;
            }
            catch (Exception ex)
            {
                OutputMessage(ex.Message);
                Logger.WriteExceptionLog(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, Common.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}