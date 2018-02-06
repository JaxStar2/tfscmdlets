using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Management.Automation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Providers;

namespace TfsCmdlets.Cmdlets
{
    public abstract class ProjectLevelCmdlet : CollectionLevelCmdlet
    {
        protected Project GetProject()
        {
            return GetProject(Project, Collection, Server, Credential);
        }

        protected IEnumerable<Project> GetProjects()
        {
            return GetProjects(Project, Collection, Server, Credential);
        }

        protected Project GetProject(object project, object collection, object server, object credential)
        {
            return ProjectProvider.GetProject(project, collection, server, credential);
        }

        protected IEnumerable<Project> GetProjects(object project, object collection, object server, object credential)
        {
            return ProjectProvider.GetProjects(project, collection, server, credential);
        }

        public abstract object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        [Import(typeof(IProjectProvider))]
        private IProjectProvider ProjectProvider { get; set; }
    }
}
