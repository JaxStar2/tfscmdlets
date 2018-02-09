using System;
using System.Management.Automation;
using Microsoft.TeamFoundation.Server;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(VerbsCommon.New, "Area", SupportsShouldProcess = true)]
    [OutputType(typeof(NodeInfo))]
    public class NewArea : NewNodeCmdletBase
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [Alias("Area")]
        public override object Path { get; set; }
    }

    [Cmdlet(VerbsCommon.New, "Iteration", SupportsShouldProcess = true)]
    [OutputType(typeof(NodeInfo))]
    public class NewIteration : NewNodeCmdletBase
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [Alias("Iteration")]
        public override object Path { get; set; }
    }

    public abstract class NewNodeCmdletBase : NodeCmdletBase
    {
        protected override void ProcessRecord()
        {
            var node = CreateNewNode($"{Path}", Scope);

            if (!Passthru) return;

            WriteObject(node);
        }

        private NodeInfo CreateNewNode(string path, NodeScope scope, DateTime? startDate = null, DateTime? finishDate = null)
        {
            var tp = GetProject();
            var tpc = tp.Store.TeamProjectCollection;
            var projectName = tp.Name;
            var fullPath = NormalizePath(path, projectName, scope.ToString());
            var parentPath = System.IO.Path.GetDirectoryName(fullPath);
            var nodeName = System.IO.Path.GetFileName(fullPath);
            var cssService = tpc.GetService<ICommonStructureService>();

            NodeInfo parentNode;

            try
            {
                parentNode = cssService.GetNodeFromPath(parentPath);
            }
            catch
            {
                parentNode = CreateNewNode(parentPath, scope);
            }

            string nodeUri;

            if (parentNode == null || !ShouldProcess($"{Path}", $"Create {Scope} Path")) return null;

            if (startDate.HasValue || finishDate.HasValue)
            {
                var cssService4 = tpc.GetService<ICommonStructureService4>();
                nodeUri = cssService4.CreateNode(nodeName, parentNode.Uri, startDate, finishDate);
            }
            else
            {
                nodeUri = cssService.CreateNode(nodeName, parentNode.Uri);
            }

            return cssService.GetNode(nodeUri);
        }

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