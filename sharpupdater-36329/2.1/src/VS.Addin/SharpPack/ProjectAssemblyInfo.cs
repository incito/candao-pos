using EnvDTE;

namespace CnSharp.Windows.Updater.SharpPack
{
    public class ProjectAssemblyInfo
    {
        public ProjectAssemblyInfo(Project project)
        {
            Project = project;
        }

        //private Project _project;
        //public Project Project
        //{
        //    get { return _project; }
        //    set { _project = value;
        //        AssemblyName = _project == null ? null :  _project.Properties.Item("AssemblyName").Value.ToString();
        //    }
        //}

        public Project Project { get; set; }

        public string Version { get; set; }
        public string NewVersion { get; set; }
        public string ProductName { get; set; }
        //public string AssemblyName { get; private set; }
    }
}
