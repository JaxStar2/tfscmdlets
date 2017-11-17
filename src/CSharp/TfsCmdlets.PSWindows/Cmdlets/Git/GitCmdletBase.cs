using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace TfsCmdlets.Cmdlets.Git
{
    public class GitCmdletBase : ProjectLevelCmdlet
    {
        protected IEnumerable<GitRepository> GetRepositories(object repository)
        {
            switch (repository)
            {
                case PSObject pso:
                    {
                        foreach (var r in GetRepositories(pso.BaseObject))
                        {
                            yield return r;
                        }
                        break;
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
        }
    }
}
