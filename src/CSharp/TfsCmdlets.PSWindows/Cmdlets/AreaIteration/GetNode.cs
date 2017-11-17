using System.Management.Automation;
using Microsoft.TeamFoundation.Server;
using TfsCmdlets.Cmdlets.AreaIteration;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    [Cmdlet(verbName: VerbsCommon.Get, nounName: "Area")]
    [OutputType(typeof(Microsoft.TeamFoundation.Server.NodeInfo))]
    public class GetArea : GetAreaIterationCmdletBase
    {
        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Area")]
        public override object Path { get; set; } = @"\**";
    }

    [Cmdlet(verbName: VerbsCommon.Get, nounName: "Iteration")]
    [OutputType(typeof(Microsoft.TeamFoundation.Server.NodeInfo))]
    public class GetIteration : GetAreaIterationCmdletBase
    {
        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Iteration")]
        public override object Path { get; set; } = @"\**";
    }

    public abstract class GetAreaIterationCmdletBase : AreaIterationCmdletBase
    {
        protected override void ProcessRecord()
        {
            foreach (var n in GetNodes(Path))
            {
                WriteObject(n);
            }
        }

    }
}