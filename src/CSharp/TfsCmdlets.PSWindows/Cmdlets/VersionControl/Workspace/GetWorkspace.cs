using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.VersionControl.Workspace
{
    [Cmdlet(VerbsCommon.Get, "Workspace")]
    [OutputType("Microsoft.TeamFoundation.VersionControl.Client.Workspace,Microsoft.TeamFoundation.VersionControl.Client")]
    public class GetWorkspace: WorkspaceCmdletBase
    {
        [Parameter(Position = 0)]
        [SupportsWildcards]
        public override object Workspace { get; set; } = "*";

        [Parameter]
        [SupportsWildcards]
        [Alias("Owner")]
        public string OwnerName { get; set; }

        [Parameter]
        [SupportsWildcards]
        [Alias("Computer")]
        public string ComputerName { get; set; }

        [Parameter(ValueFromPipeline=true)]
        public override object Collection { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(WorkspaceService.GetWorkspaces(Workspace, OwnerName, ComputerName, Collection, Server, Credential), true);
        }
    }
}
