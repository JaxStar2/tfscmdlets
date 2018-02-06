using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.Git
{
    [Cmdlet(VerbsCommon.Get, "GitRepository")]
    [OutputType(typeof(Microsoft.TeamFoundation.SourceControl.WebApi.GitRepository))]
    public class GetGitRepository : GitCmdletBase
    {
        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Name")]
        public string Repository { get; set; } = "*";

        protected override void ProcessRecord()
        {
            WriteObject(GetRepositories(Repository), true);
        }

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