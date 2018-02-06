using System.Management.Automation;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace TfsCmdlets.Cmdlets.Git
{
    [Cmdlet(VerbsCommon.Rename, "GitRepository", ConfirmImpact = ConfirmImpact.Medium,SupportsShouldProcess = true)]
    [OutputType(typeof(GitRepository))]
    public class RenameGitRepository : GitCmdletBase
    {
        protected override void ProcessRecord()
        {
            var reposToRename = GetRepositories(Repository);

            var tp = GetProject();
            var tpc = tp.Store.TeamProjectCollection;
            var gitClient = tpc.GetClient<GitHttpClient>();

            foreach (var repo in reposToRename)
            {
                if (!ShouldProcess(repo.Name, $"Rename Git repository in team project {tp.Name} to {NewName}"))
                    continue;

                var result = gitClient.RenameRepositoryAsync(repo, NewName).Result;

                if (!Passthru) continue;

                WriteObject(result);
            }
        }

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [Alias("Name")]
        public object Repository { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string NewName { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Parameter]
        public override object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }
    }
}