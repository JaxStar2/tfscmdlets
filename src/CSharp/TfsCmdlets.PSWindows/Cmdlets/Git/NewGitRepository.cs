using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Cmdlets.TeamProject;

namespace TfsCmdlets.Cmdlets.Git
{
    [Cmdlet(verbName: VerbsCommon.New, nounName: "GitRepository", ConfirmImpact = ConfirmImpact.Medium,
        SupportsShouldProcess = true)]
    [OutputType(typeof(Microsoft.TeamFoundation.SourceControl.WebApi.GitRepository))]
    public class NewGitRepository : ProjectLevelCmdlet
    {
        [Parameter(Mandatory = true)]
        [Alias("Name")]
        public string Repository { get; set; }

        [Parameter()]
        public SwitchParameter Passthru { get; set; }

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(Repository, "Create Git repository")) return;

            var tp = GetProject();
            var tpc = tp.Store.TeamProjectCollection;
            var gitClient = tpc.GetClient<GitHttpClient>();

            var tpRef = new TeamProjectReference()
            {
                Id = tp.Guid,
                Name = tp.Name
            };

            var repoToCreate = new GitRepository()
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
    }
}