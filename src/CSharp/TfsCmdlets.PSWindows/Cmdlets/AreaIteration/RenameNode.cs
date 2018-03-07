using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(VerbsCommon.Rename, "Area", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo,Microsoft.TeamFoundation.Client")]
    public class RenameArea : RenameNodeCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [Alias("Area")]
        public override object Path { get; set; } 
    }

    [Cmdlet(VerbsCommon.Rename, "Iteration", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo,Microsoft.TeamFoundation.Client")]
    public class RenameIteration : RenameNodeCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [Alias("Iteration")]
        public override object Path { get; set; }
    }

    public abstract class RenameNodeCmdletBase : SetNodeCmdletBase
    {
        [Parameter]
        public override int MoveBy { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

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
