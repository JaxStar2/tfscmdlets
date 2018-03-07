using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(VerbsCommon.Move, "Area", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo,Microsoft.TeamFoundation.Client")]
    public class MoveArea : MoveNodeCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [SupportsWildcards]
        [Alias("Area")]
        public override object Path { get; set; }
    }

    [Cmdlet(VerbsCommon.Move, "Iteration", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo,Microsoft.TeamFoundation.Client")]
    public class MoveIteration : MoveNodeCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [SupportsWildcards]
        [Alias("Iteration")]
        public override object Path { get; set; }
    }

    public abstract class MoveNodeCmdletBase : NodeCmdletBase
    {
        protected override void ProcessRecord()
        {
            var originalNodes = GetNodes(Path).ToList();

            if (originalNodes.Count == 0)
            {
                throw new PSArgumentException($"Invalid or non-existent {Scope} {Path}");
            }

            var destinationNode = GetNodes(Destination).FirstOrDefault();

            if (destinationNode == null)
            {
                throw new PSArgumentException($"Invalid or non-existent destination {Scope} {Destination}");
            }

            var movedNodes = new List<INodeInfoAdapter>();

            foreach (var n in originalNodes)
            {
                if (!ShouldProcess(n.Path, $"Move node to {destinationNode}")) continue;

                CommonStructureService.MoveBranch(n.Uri, destinationNode.Uri, Collection, Server, Credential);
                movedNodes.Add(CommonStructureService.GetNode(n.Uri, Collection, Server, Credential));
            }

            if (!Passthru) return;

            foreach (var node in movedNodes)
            {
                WriteObject(node);
            }
        }

        [Parameter(Position = 1)]
        public object Destination { get; set; }

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