using System;
using System.IO;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using VSLangProj80;

namespace CnSharp.Delivery.VisualStudio.PackingTool.Commands
{
    public class ZipBuildMenuItem : StandardBuildMenuItem
    {
        public ZipBuildMenuItem() 
        {
            CommandName = "PackageZip";
            MenuText = "Package as .zip";
            FaceId = 105;

            ZipOnly = true;
        }


        protected override bool IsFileExcluded(string file)
        {
            var ext = Path.GetExtension(file).ToLower();

            return (ext == ".pdb" || ext == ".log" || file.Contains(".vshost."));
        }


        public override bool Build()
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
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, Common.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
