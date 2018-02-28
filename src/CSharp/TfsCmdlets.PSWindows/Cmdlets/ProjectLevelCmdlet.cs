using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets
{
    public abstract class ProjectLevelCmdlet : CollectionLevelCmdlet
    {
        protected IProjectAdapter GetProject()
        {
            return GetProject(Project, Collection, Server, Credential);
        }

        protected IEnumerable<IProjectAdapter> GetProjects()
        {
            return GetProjects(Project, Collection, Server, Credential);
        }

        protected IProjectAdapter GetProject(object project, object collection, object server, object credential)
        {
            return ProjectService.GetProject(project, collection, server, credential);
        }

        protected IEnumerable<IProjectAdapter> GetProjects(object project, object collection, object server, object credential)
        {
            return ProjectService.GetProjects(project, collection, server, credential);
        }

        public abstract object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        [Import(typeof(IProjectService))]
        protected IProjectService ProjectService { get; set; }
    }
}
