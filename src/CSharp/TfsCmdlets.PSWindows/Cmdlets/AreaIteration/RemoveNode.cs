using System.Linq;
using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(VerbsCommon.Remove, "Area", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo,Microsoft.TeamFoundation.Client")]
    public class RemoveArea : RemoveNodeCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [SupportsWildcards]
        [Alias("Area")]
        public override object Path { get; set; }
    }

    [Cmdlet(VerbsCommon.Remove, "Iteration", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo,Microsoft.TeamFoundation.Client")]
    public class RemoveIteration : RemoveNodeCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [SupportsWildcards]
        [Alias("Iteration")]
        public override object Path { get; set; }
    }

    public abstract class RemoveNodeCmdletBase : NodeCmdletBase
    {
        protected override void ProcessRecord()
        {
            var nodes = GetNodes(Path).OrderByDescending(o => o.Path);
            var newNode = GetNodes(MoveTo).First();

            foreach (var node in nodes)
            {
                if (!ShouldProcess(node.Path, "Delete Area")) continue;

                CommonStructureService.DeleteBranches(new[] { node.Uri }, newNode.Uri, Collection, Server, Credential);
            }
        }

        [Parameter(Position = 1)]
        [Alias("NewPath")]
        public object MoveTo { get; set; } = @"\";

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

