using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client.Channels;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Cmdlets.TeamProject;

namespace TfsCmdlets.Cmdlets.Git
{
    [Cmdlet(verbName: VerbsCommon.Remove, nounName: "GitRepository", SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(Microsoft.TeamFoundation.SourceControl.WebApi.GitRepository))]
    public class RemoveGitRepository : GitCmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [SupportsWildcards()]
        [Alias("Name")]
        public object Repository { get; set; }

        protected override void ProcessRecord()
        {
            var reposToDelete = GetRepositories(Repository);

            var tp = GetProject();
            var tpc = tp.Store.TeamProjectCollection;
            var gitClient = tpc.GetClient<GitHttpClient>();

            foreach (var repo in reposToDelete)
            {
                if (!ShouldProcess(repo.Name, $"Delete Git repository from Team Project {tp.Name}")) continue;

                gitClient.DeleteRepositoryAsync(repo.Id).Wait();
            }
        }
    }
}