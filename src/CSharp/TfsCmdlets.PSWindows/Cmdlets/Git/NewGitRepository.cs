using System.Management.Automation;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace TfsCmdlets.Cmdlets.Git
{
    [Cmdlet(VerbsCommon.New, "GitRepository", ConfirmImpact = ConfirmImpact.Medium,
        SupportsShouldProcess = true)]
    [OutputType(typeof(GitRepository))]
    public class NewGitRepository : ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(Repository, "Create Git repository")) return;

            var tp = GetProject();
            var tpc = tp.Store.TeamProjectCollection;
            var gitClient = tpc.GetClient<GitHttpClient>();

            var tpRef = new TeamProjectReference
            {
                Id = tp.Guid,
                Name = tp.Name
            };

            var repoToCreate = new GitRepository
            {
                Name = Repository,
                ProjectReference = tpRef
            };

            var result = gitClient.CreateRepositoryAsync(repoToCreate, tp.Name).Result;

            if (Passthru)
            {
                WriteObject(result);
            }
        }

        [Parameter(Mandatory = true, Position = 0)]
        [Alias("Name")]
        public string Repository { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Parameter(ValueFromPipeline = true)]
        public override object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        [Parameter]
        public override object Server { get; set; }

        [Parameter]
        public override object Credential { get; set; }
    }
}