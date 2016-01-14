using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace CnSharp.Delivery.VisualStudio.PackingTool.Plugins
{
    public class TfsSourceControl : ISourceControl
    {
        private static readonly Hashtable _table = Hashtable.Synchronized(new Hashtable());

        public int CheckOut(string slnDir,string file)
        {
            var workspace = GetWorkspace(slnDir);
            if(workspace == null)
                return -1;
            return workspace.PendEdit(file);
        }

        private static Workspace GetWorkspace(string slnDir)
        {
            if (_table.Contains(slnDir))
            {
                return _table[slnDir] as Workspace;
            }
            var projectCollections = new List<RegisteredProjectCollection>((RegisteredTfsConnections.GetProjectCollections()));
            var onlineCollections = projectCollections.Where(c => c.Offline == false);

            // fail if there are no registered collections that are currently on-line
            if (!onlineCollections.Any())
            {
                _table.Add(slnDir,null);
                return null;
            }
            Workspace workspace = null;
            // find a project collection with at least one team project
            foreach (var registeredProjectCollection in onlineCollections)
            {
                var projectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(registeredProjectCollection);

                var versionControl = (VersionControlServer)projectCollection.GetService(typeof(VersionControlServer));

                var teamProjects = new List<TeamProject>(versionControl.GetAllTeamProjects(false));

                // if there are no team projects in this collection, skip it
                if (teamProjects.Count < 1) continue;

                var dir = new DirectoryInfo(slnDir);
                while (workspace == null)
                {
                    workspace = versionControl.TryGetWorkspace(dir.FullName);
                    if (dir.Parent == null)
                        break;
                    dir = dir.Parent;
                }

                if (workspace != null && workspace.HasUsePermission)
                    break;
            }
            _table.Add(slnDir, workspace);
            return workspace;
        }
    }
}
