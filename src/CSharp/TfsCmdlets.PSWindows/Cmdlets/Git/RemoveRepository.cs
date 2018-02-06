using System.Management.Automation;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace TfsCmdlets.Cmdlets.Git
{
    [Cmdlet(VerbsCommon.Remove, "GitRepository", SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(GitRepository))]
    public class RemoveGitRepository : GitCmdletBase
    {
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

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [SupportsWildcards]
        [Alias("Name")]
        public object Repository { get; set; }

        [Parameter]
        public override object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        [Parameter]
        public override object Server { get; set; }

        [Parameter]
        public override object Credential { get; set; }
    }
}