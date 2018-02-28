using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.Build
{
    [Cmdlet(VerbsCommon.Get, "BuildDefinition")]
    [OutputType("Microsoft.TeamFoundation.Build.WebApi.BuildDefinition")]
    public class GetBuildDefinition : BuildCmdletBase
    {
        protected override void ProcessRecord()
        {
            WriteObject(GetBuildDefinitions(), true);
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
