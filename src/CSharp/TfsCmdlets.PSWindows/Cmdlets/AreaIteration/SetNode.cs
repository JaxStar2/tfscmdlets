using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Server;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(VerbsCommon.Set, "Area", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    [OutputType(typeof(NodeInfo))]
    public class SetArea : SetNodeCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true,ValueFromPipeline = true)]
        [SupportsWildcards]
        [Alias("Area")]
        public override object Path { get; set; } = @"\**";
    }

    [Cmdlet(VerbsCommon.Set, "Iteration", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    [OutputType(typeof(NodeInfo))]
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

            var cssService = GetCssService();

            if (!string.IsNullOrWhiteSpace(NewName))
            {
                RenameNodes(nodes, NewName, cssService);
            }

            if (MoveBy != 0)
            {
                MoveNodes(nodes, MoveBy, cssService);
            }
        }

        private void RenameNodes(IEnumerable<NodeInfo> nodes, string newName, ICommonStructureService cssService)
        {
            foreach (var node in nodes)
            {
                if (!ShouldProcess($"{Path}", $"Rename {Scope} to {newName}")) continue;

                cssService.RenameNode(node.Uri, newName);
                WriteObject(cssService.GetNode(node.Uri));
            }
        }

        private void MoveNodes(IEnumerable<NodeInfo> nodes, int moveBy, ICommonStructureService cssService)
        {
            foreach (var node in nodes)
            {
                if (!ShouldProcess($"{Path}",
                    $"Reorder {Scope} by moving it {moveBy} positions (negative is up, positive is down)")) continue;

                cssService.ReorderNode(node.Uri, moveBy);
                WriteObject(cssService.GetNode(node.Uri));
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
