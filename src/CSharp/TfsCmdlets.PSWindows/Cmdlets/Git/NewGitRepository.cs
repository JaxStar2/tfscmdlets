using System;
using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.Git
{
    [Cmdlet(VerbsCommon.New, "GitRepository", ConfirmImpact = ConfirmImpact.Medium,
        SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.SourceControl.WebApi.GitRepository,Microsoft.TeamFoundation.SourceControl.WebApi")]
    public class NewGitRepository : GitCmdletBase
    {
        protected override void ProcessRecord()
        {
            var repo = GitRepositoryService.CreateRepository(Repository, Project, Collection, Server, Credential);

            if (Passthru)
                WriteObject(repo);
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