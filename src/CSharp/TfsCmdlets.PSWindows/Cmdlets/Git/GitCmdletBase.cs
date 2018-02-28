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
            while (true)
            {
                switch (repository)
                {
                    case PSObject pso:
                    {
                        repository = pso.BaseObject;
                        continue;
                    }
                    case IGitRepositoryAdapter r:
                    {
                        yield return r;
                        break;
                    }
                    case string s:
                    {
                        var tp = GetProject();

                        foreach (var r in GitRepositoryService.GetRepositories(tp.Name, Project, Collection, Server, Credential))
                        {
                            yield return r;
                        }
                        break;
                    }
                    default:
                    {
                        throw new PSArgumentException($"Invalid git repository name {repository}");
                    }
                }
                break;
            }
        }

        [Import(typeof(IGitRepositoryService))]
        protected IGitRepositoryService GitRepositoryService { get; set; }
    }
}
