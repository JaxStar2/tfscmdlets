using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(verbName: VerbsCommon.Remove, nounName: "Area", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [OutputType(typeof(Microsoft.TeamFoundation.Server.NodeInfo))]
    public class RemoveArea : RemoveAreaIterationCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [SupportsWildcards]
        [Alias("Area")]
        public override object Path { get; set; }
    }

    [Cmdlet(verbName: VerbsCommon.Remove, nounName: "Iteration", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [OutputType(typeof(Microsoft.TeamFoundation.Server.NodeInfo))]
    public class RemoveIteration : RemoveAreaIterationCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [SupportsWildcards]
        [Alias("Iteration")]
        public override object Path { get; set; }
    }

    public abstract class RemoveAreaIterationCmdletBase : AreaIterationCmdletBase
    {
        [Parameter(Position = 1)]
        [Alias("NewPath")]
        public object MoveTo { get; set; } = @"\";

        protected override void ProcessRecord()
        {
            var nodes = GetNodes(Path).OrderByDescending(o => o.Path);
            var newNode = GetNodes(MoveTo).First();
            var cssService = GetCssService();

            foreach (var node in nodes)
            {
                if (!ShouldProcess(node.Path, "Delete Area")) continue;

                cssService.DeleteBranches(new[] { node.Uri }, newNode.Uri);
            }
        }
    }
}

