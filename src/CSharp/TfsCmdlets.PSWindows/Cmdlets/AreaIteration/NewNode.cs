using System;
using System.Management.Automation;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(VerbsCommon.New, "Area", SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo")]
    public class NewArea : NewNodeCmdletBase
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [Alias("Area")]
        public override object Path { get; set; }
    }

    [Cmdlet(VerbsCommon.New, "Iteration", SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo")]
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

        private INodeInfoAdapter CreateNewNode(string path, NodeScope scope, DateTime? startDate = null, DateTime? finishDate = null)
        {
            var tp = GetProject();
            var projectName = tp.Name;
            var fullPath = NormalizePath(path, projectName, scope.ToString());
            var parentPath = System.IO.Path.GetDirectoryName(fullPath);
            var nodeName = System.IO.Path.GetFileName(fullPath);

            INodeInfoAdapter parentNode;

            try
            {
                parentNode = CommonStructureService.GetNodeFromPath(parentPath, Collection, Server, Credential);
            }
            catch
            {
                parentNode = CreateNewNode(parentPath, scope);
            }

            string nodeUri;

            if (parentNode == null || !ShouldProcess($"{Path}", $"Create {Scope} Path")) return null;

            if (startDate.HasValue || finishDate.HasValue)
            {
                nodeUri = CommonStructureService.CreateNode(nodeName, parentNode.Uri, startDate, finishDate, Collection, Server, Credential);
            }
            else
            {
                nodeUri = CommonStructureService.CreateNode(nodeName, parentNode.Uri, Collection, Server, Credential);
            }
            
            return CommonStructureService.GetNode(nodeUri, Collection, Server, Credential);
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