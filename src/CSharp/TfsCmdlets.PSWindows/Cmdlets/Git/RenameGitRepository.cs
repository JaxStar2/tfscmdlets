using System.Collections.Generic;
using System.Management.Automation;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Cmdlets.Git
{
    [Cmdlet(VerbsCommon.Rename, "GitRepository", ConfirmImpact = ConfirmImpact.Medium,SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.SourceControl.WebApi.GitRepository,Microsoft.TeamFoundation.SourceControl.WebApi")]
    public class RenameGitRepository : GitCmdletBase
    {
        protected override void ProcessRecord()
        {
            var reposToRename = GitRepositoryService.GetRepositories(Repository, Project, Collection, Server, Credential);

            var tp = GetProject();

            foreach (var repo in reposToRename)
            {
                if (!ShouldProcess(repo.Name, $"Rename Git repository in team project {tp.Name} to {NewName}"))
                    continue;

                var result = GitRepositoryService.RenameRepository(repo, NewName, Project, Collection, Server, Credential);

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