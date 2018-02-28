using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(VerbsCommon.Set, "Area", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo")]
    public class SetArea : SetNodeCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true,ValueFromPipeline = true)]
        [SupportsWildcards]
        [Alias("Area")]
        public override object Path { get; set; } = @"\**";
    }

    [Cmdlet(VerbsCommon.Set, "Iteration", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo")]
    public class SetIteration : SetNodeCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [SupportsWildcards]
        [Alias("Iteration")]
        public override object Path { get; set; } = @"\**";

    }

    public abstract class SetNodeCmdletBase : NodeCmdletBase
    {
        protected override void ProcessRecord()
        {
            var nodes = GetNodes(Path).ToList();

            if (nodes.Count == 0)
            {
                throw new PSArgumentException($"Invalid or non-existent {Scope} {Path}");
            }

            if (!string.IsNullOrWhiteSpace(NewName))
            {
                RenameNodes(nodes, NewName);
            }

            if (MoveBy != 0)
            {
                MoveNodes(nodes, MoveBy);
            }
        }

        private void RenameNodes(List<INodeInfoAdapter> nodes, string newName)
        {
            foreach (var node in nodes)
            {
                if (!ShouldProcess($"{Path}", $"Rename {Scope} to {newName}")) continue;

                CommonStructureService.RenameNode(node.Uri, newName, Collection, Server, Credential);
                WriteObject(CommonStructureService.GetNode(node.Uri, Collection, Server, Credential));
            }
        }

        private void MoveNodes(List<INodeInfoAdapter> nodes, int moveBy)
        {
            foreach (var node in nodes)
            {
                if (!ShouldProcess($"{Path}",
                    $"Reorder {Scope} by moving it {moveBy} positions (negative is up, positive is down)")) continue;

                CommonStructureService.ReorderNode(node.Uri, moveBy, Collection, Server, Credential);
                WriteObject(CommonStructureService.GetNode(node.Uri, Collection, Server, Credential));
            }
        }

        [Parameter(Position = 1)]
        public string NewName { get; set; }

        [Parameter]
        public virtual int MoveBy { get; set; }

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
