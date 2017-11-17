using System;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Microsoft.TeamFoundation.Client.Channels;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Cmdlets.TeamProject;

namespace TfsCmdlets.Cmdlets.Git
{
    [Cmdlet(verbName: VerbsCommon.Rename, nounName: "GitRepository", ConfirmImpact = ConfirmImpact.Medium,SupportsShouldProcess = true)]
    [OutputType(typeof(Microsoft.TeamFoundation.SourceControl.WebApi.GitRepository))]
    public class RenameGitRepository : GitCmdletBase
    {
        [Parameter()]
        [SupportsWildcards()]
        [Alias("Name")]
        public object Repository { get; set; } = "*";

        [Parameter()]
        public string NewName { get; set; }

        [Parameter()]
        public SwitchParameter Passthru { get; set; }

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
    }
}