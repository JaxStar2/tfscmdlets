using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Server;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(verbName: VerbsCommon.Move, nounName: "Area", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    public class MoveArea : MoveAreaIterationCmdletBase
    {
        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Area")]
        public override object Path { get; set; }
    }

    [Cmdlet(verbName: VerbsCommon.Move, nounName: "Iteration", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    public class MoveIteration : MoveAreaIterationCmdletBase
    {
        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Iteration")]
        public override object Path { get; set; }
    }

    public abstract class MoveAreaIterationCmdletBase : AreaIterationCmdletBase
    {
        [Parameter(Position = 1)]
        public object Destination { get; set; }

        [Parameter()]
        public SwitchParameter Passthru { get; set; }

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

            var cssService = GetCssService();
            var movedNodes = new List<NodeInfo>();

            foreach (var n in originalNodes)
            {
                if (!ShouldProcess(n.Path, $"Move node to {destinationNode}")) continue;

                cssService.MoveBranch(n.Uri, destinationNode.Uri);
                movedNodes.Add(cssService.GetNode(n.Uri));
            }

            if (!Passthru) return;

            foreach (var node in movedNodes)
            {
                WriteObject(node);
            }
        }
    }
}