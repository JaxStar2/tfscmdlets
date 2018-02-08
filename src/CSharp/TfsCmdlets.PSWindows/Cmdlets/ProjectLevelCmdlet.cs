using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Management.Automation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Services;

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
            return ProjectService.GetProject(project, collection, server, credential);
        }

        protected IEnumerable<Project> GetProjects(object project, object collection, object server, object credential)
        {
            return ProjectService.GetProjects(project, collection, server, credential);
        }

        public abstract object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }
    }
}
