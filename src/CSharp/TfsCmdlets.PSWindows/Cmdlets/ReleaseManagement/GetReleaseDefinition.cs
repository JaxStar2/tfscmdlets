using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.ReleaseManagement
{
    [Cmdlet(VerbsCommon.Get, "ReleaseDefinition")]
    [OutputType("Microsoft.TeamFoundation.Release.WebApi.ReleaseDefinition,Microsoft.TeamFoundation.Release.WebApi")]
    public class GetReleaseDefinition : ReleaseCmdletBase
    {
        protected override void ProcessRecord()
        {
            WriteObject(GetReleaseDefinitions(), true);
        }

        [Parameter(Position = 0)]
        [ValidateNotNull]
        [SupportsWildcards]
        [Alias("Path")]
        public override object Definition { get; set; } = "**";

        [Parameter(ValueFromPipeline = true)]
        public override object Project { get; set; }
    }
}
