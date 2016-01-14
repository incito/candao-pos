using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CnSharp.Windows.Updater.SharpPack.Plugin;
using CnSharp.Windows.Updater.Util;
using EnvDTE;

namespace CnSharp.Windows.Updater.SharpPack
{
    public partial class VersionEditForm : Form
    {
        #region Constants and Fields


        private readonly Project _startProject;
        private readonly Dictionary<Project,ProjectAssemblyInfo> _assemblyInfos = new Dictionary<Project, ProjectAssemblyInfo>();
        private string _currentProductName;
        private string _currentCompanyName;
        private string _newVersion;
        private ISourceControl _sourceControl;
        private ISourceControl SourceControl
        {
            get
            {
                return _sourceControl ??
                       (_sourceControl = new TfsSourceControl());
            }
        }

        #endregion

        #region Constructors and Destructors

        public VersionEditForm(Project startProject, IEnumerable<Project> refProjects)
        {
            InitializeComponent();
            projectGrid.Rows.Clear();
            _startProject = startProject;
            BindProject(startProject);
            foreach (var project in refProjects)
            {
                //if (project == startProject || project.UniqueName == "<MiscFiles>") 
                //    continue;
                BindProject(project);
            }
          
        }

        private void BindProject(Project project)
        {
            var ass = GetProjectAssemblyInfo(project);
            _assemblyInfos.Add(project,ass);
            projectGrid.Rows.Add(
                project.Name, 
                ass != null ? ass.Version : string.Empty
             );
            projectGrid.Rows[projectGrid.Rows.Count - 1].Tag = ass;
            if (project == _startProject)
            {
                projectGrid.Rows[projectGrid.Rows.Count - 1].DefaultCellStyle.Font = new Font(
                projectGrid.Font.FontFamily,
                  projectGrid.Font.Size,
                     FontStyle.Bold
                );
                if (ass != null) 
                    _currentProductName = ass.ProductName;
            }
        }

        private   ProjectAssemblyInfo GetProjectAssemblyInfo(Project project)
        {
            string assemblyInfoFile;
            var assemblyInfo = GetAssemblyInfo(project, out assemblyInfoFile);
            var currentVersion =
                Regex.Match(assemblyInfo, @"AssemblyFileVersion\(""(?<version>.+)""\)").Groups["version"].Value;
            var currentProductName =
                Regex.Match(assemblyInfo, @"AssemblyProduct\(""(?<product>.+)""\)").Groups["product"].Value;
            if (project == _startProject)
            {
                _currentCompanyName = Regex.Match(assemblyInfo, @"AssemblyCompany\(""(?<company>.+)""\)").Groups["company"].Value;
            }
            return new ProjectAssemblyInfo(project) { Version = currentVersion, ProductName = currentProductName };
        }

        private static string GetAssemblyInfo(Project project,out string assemblyInfoFile)
        {
            var prjDir = Path.GetDirectoryName(project.FileName);
            assemblyInfoFile = prjDir + "\\Properties\\AssemblyInfo.cs";
            if (!File.Exists(assemblyInfoFile))
            {
                assemblyInfoFile = prjDir + "\\AssemblyInfo.cs";
            }
            if (!File.Exists(assemblyInfoFile))
            {
                return null;
            }
            var assemblyInfo = FileUtil.ReadText(assemblyInfoFile, Encoding.Default);
            return assemblyInfo;
        }

        #endregion

        #region Public Properties

        public Dictionary<Project, ProjectAssemblyInfo> ProjectInfo
        {
            get { return _assemblyInfos; }
        }

        public ProductInfo ProductToRelease { get; private set; }

        #endregion

        #region Methods

        private void FormVersionLoad(object sender, EventArgs e)
        {
            ActiveControl = projectGrid;
        }

        private void Button1Click(object sender, EventArgs e)
        {
            projectGrid.EndEdit();
            var i = 0;
            foreach (DataGridViewRow row in projectGrid.Rows)
            {
                var assemblyInfo = row.Tag as ProjectAssemblyInfo;
                var newVersion = row.Cells[1].Value == null ? null :  row.Cells[1].Value.ToString().Trim();
                if (string.IsNullOrEmpty(newVersion) && i == 0 ) 
                    return;

                if (assemblyInfo != null)
                {
                    if (assemblyInfo.Version != newVersion)
                    {
                        assemblyInfo.NewVersion = newVersion;
                        string assemblyInfoFile;
                        var  assemblyText = GetAssemblyInfo(assemblyInfo.Project, out assemblyInfoFile);

                        CheckOut(assemblyInfo, assemblyInfoFile);

                        assemblyText = Regex.Replace(assemblyText, @"AssemblyFileVersion\("".+""\)",
                            string.Format("AssemblyFileVersion(\"{0}\")", newVersion));
                        assemblyText = Regex.Replace(assemblyText, @"AssemblyVersion\("".+""\)",
                     string.Format("AssemblyVersion(\"{0}\")", newVersion));
                        FileUtil.WriteText(assemblyInfoFile, assemblyText, Encoding.Default);

                        //assemblyInfo.Project.Save();
                    }

                    if (i == 0)
                    {
                        _newVersion = newVersion;
                    }
                }
             
                i++;
                row.DefaultCellStyle.ForeColor = Color.Green;
            }

            ProductToRelease = new ProductInfo
                {
                    CompanyName = _currentCompanyName,
                    Name = _currentProductName,
                    Version = _newVersion
                };

            DialogResult = DialogResult.OK;
        }

        protected virtual  void CheckOut(ProjectAssemblyInfo assemblyInfo, string assemblyInfoFile)
        {
            if (SourceControl != null)
            {
                try
                {
                    var files =
                       SourceControl.CheckOut(
                            Path.GetDirectoryName(assemblyInfo.Project.DTE.Solution.FullName),
                            assemblyInfoFile);
                    if (files <= 0)
                        File.SetAttributes(assemblyInfoFile, FileAttributes.Normal);
                }
                catch (Exception exception)
                {
                    Common.MyLogger.WriteExceptionLog(exception);
                    File.SetAttributes(assemblyInfoFile, FileAttributes.Normal);
                }
            }
            else
            {
                File.SetAttributes(assemblyInfoFile, FileAttributes.Normal);
            }
        }

        #endregion

        //private void chkSingle_CheckedChanged(object sender, EventArgs e)
        //{
        //    var allVersionControlled = chkAll.Checked;
        //    var i = 0;
        //    foreach (DataGridViewRow row in projectGrid.Rows)
        //    {
        //        if (i > 0)
        //        {
        //            row.DefaultCellStyle.ForeColor = !allVersionControlled ? Color.Gray : SystemColors.WindowText;
        //            row.ReadOnly = !allVersionControlled;
        //        }
        //        i++;
        //    }

        //    chkSame.Enabled = chkAll.Checked;
        //    if(!chkSame.Enabled)
        //        chkSame.Checked = false;
        //}

        private void chkSame_CheckedChanged(object sender, EventArgs e)
        {
            var i = 0;
            var v = projectGrid.Rows[0].Cells[1].Value.ToString();
            foreach (DataGridViewRow row in projectGrid.Rows)
            {
                if (i > 0)
                {
                    row.Cells[1].Value = chkSame.Checked ? v : (row.Tag as ProjectAssemblyInfo).Version;
                }
                i++;
            }
        }
    }
}