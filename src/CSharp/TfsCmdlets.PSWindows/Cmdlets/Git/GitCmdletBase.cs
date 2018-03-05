using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.Git
{
    public abstract class GitCmdletBase : ProjectLevelCmdlet
    {
        protected IEnumerable<IGitRepositoryAdapter> GetRepositories(object repository)
        {
            return GitRepositoryService.GetRepositories(repository, Project, Collection, Server, Credential);
        }

        [Import(typeof(IGitRepositoryService))]
        protected IGitRepositoryService GitRepositoryService { get; set; }
    }
}
