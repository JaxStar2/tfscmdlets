using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace TfsCmdlets.Cmdlets.Git
{
    public abstract class GitCmdletBase : ProjectLevelCmdlet
    {
        protected IEnumerable<GitRepository> GetRepositories(object repository)
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
                    case GitRepository r:
                    {
                        yield return r;
                        break;
                    }
                    case string s:
                    {
                        var tp = GetProject();
                        var tpc = tp.Store.TeamProjectCollection;
                        var gitService = tpc.GetService<Microsoft.TeamFoundation.Git.Client.GitRepositoryService>();

                        foreach (var r in gitService.QueryRepositories(tp.Name).Where(o => o.Name.IsLike(s)))
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
    }
}
