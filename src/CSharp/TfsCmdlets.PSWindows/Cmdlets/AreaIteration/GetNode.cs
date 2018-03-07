using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(VerbsCommon.Get, "Area")]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo,Microsoft.TeamFoundation.Client")]
    public class GetArea : GetNodeCmdletBase
    {
        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Area")]
        public override object Path { get; set; } = @"\**";
    }

    [Cmdlet(VerbsCommon.Get, "Iteration")]
    [OutputType("Microsoft.TeamFoundation.Server.NodeInfo,Microsoft.TeamFoundation.Client")]
    public class GetIteration : GetNodeCmdletBase
    {
        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Iteration")]
        public override object Path { get; set; } = @"\**";
    }

    public abstract class GetNodeCmdletBase : NodeCmdletBase
    {
        protected override void ProcessRecord()
        {
            foreach (var n in GetNodes(Path))
            {
                WriteObject(n);
            }
        }

        [Parameter(ValueFromPipeline=true)]
        public override object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        [Parameter]
        public override object Server { get; set; }

        [Parameter]
        public override object Credential { get; set; }
    }
}