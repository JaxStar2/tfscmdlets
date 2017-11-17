using System.Management.Automation;
using Microsoft.TeamFoundation.Server;
using TfsCmdlets.Cmdlets.AreaIteration;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(verbName: VerbsCommon.Rename, nounName: "Area", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    [OutputType(typeof(Microsoft.TeamFoundation.Server.NodeInfo))]
    public class RenameArea : RenameAreaIterationCmdletBase
    {
        [Parameter(Position = 0)]
        [Alias("Area")]
        public override object Path { get; set; } 
    }

    [Cmdlet(verbName: VerbsCommon.Rename, nounName: "Iteration", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    [OutputType(typeof(Microsoft.TeamFoundation.Server.NodeInfo))]
    public class RenameIteration : RenameAreaIterationCmdletBase
    {
        [Parameter(Position = 0)]
        [Alias("Iteration")]
        public override object Path { get; set; }
    }

    public abstract class RenameAreaIterationCmdletBase : SetAreaIterationCmdletBase
    {
        [Parameter()]
        public SwitchParameter Passthru { get; set; }

        public override int MoveBy { get; set; }

    }
}
