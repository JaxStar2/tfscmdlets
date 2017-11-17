using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Server;
using TfsCmdlets.Cmdlets.AreaIteration;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(verbName: VerbsCommon.Set, nounName: "Area", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    [OutputType(typeof(Microsoft.TeamFoundation.Server.NodeInfo))]
    public class SetArea : SetAreaIterationCmdletBase
    {
        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Area")]
        public override object Path { get; set; } = @"\**";
    }

    [Cmdlet(verbName: VerbsCommon.Set, nounName: "Iteration", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
    [OutputType(typeof(Microsoft.TeamFoundation.Server.NodeInfo))]
    public class SetIteration : SetAreaIterationCmdletBase
    {
        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Iteration")]
        public override object Path { get; set; } = @"\**";

    }

    public abstract class SetAreaIterationCmdletBase : AreaIterationCmdletBase
    {
        [Parameter(Position = 1)]
        public string NewName { get; set; }

        [Parameter]
        public virtual int MoveBy { get; set; }

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

    }
}
