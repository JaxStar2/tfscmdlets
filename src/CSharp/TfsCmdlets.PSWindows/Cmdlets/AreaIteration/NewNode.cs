using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Server;
using TfsCmdlets.Cmdlets.TeamProject;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(verbName: VerbsCommon.New, nounName: "Area", SupportsShouldProcess = true)]
    public class NewArea : NewAreaIterationCmdletBase
    {
        [Parameter(Position = 0)]
        [Alias("Area")]
        public override object Path { get; set; }
    }

    [Cmdlet(verbName: VerbsCommon.New, nounName: "Iteration", SupportsShouldProcess = true)]
    public class NewIteration : NewAreaIterationCmdletBase
    {
        [Parameter(Position = 0)]
        [Alias("Iteration")]
        public override object Path { get; set; }
    }

    public abstract class NewAreaIterationCmdletBase : AreaIterationCmdletBase
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public override object Path { get; set; }

        [Parameter()]
        public SwitchParameter Passthru { get; set; }

        protected override void ProcessRecord()
        {
            var node = CreateNewNode($"{Path}", Scope, Project, Collection);

            if (!Passthru) return;

            WriteObject(node);
        }

        private NodeInfo CreateNewNode(string path, NodeScope scope, object project, object collection,
            DateTime? startDate = null, DateTime? finishDate = null)
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
                parentNode = CreateNewNode(parentPath, scope, project, collection);
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
    }
}