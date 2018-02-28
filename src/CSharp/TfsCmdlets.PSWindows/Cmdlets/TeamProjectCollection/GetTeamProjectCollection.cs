using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Cmdlets.Connection;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsCommon.Get, "TeamProjectCollection", DefaultParameterSetName = "Get by collection")]
    [OutputType("Microsoft.TeamFoundation.Client.TfsTeamProjectCollection")]
    public class GetTeamProjectCollection : CollectionLevelCmdlet
    {
        [Parameter(Position = 0, ParameterSetName = "Get by collection")]
        [SupportsWildcards]
        public override object Collection { get; set; } = "*";

        [Parameter(ValueFromPipeline = true, ParameterSetName = "Get by collection")]
        public override object Server { get; set; }

        [Parameter(ParameterSetName = "Get by collection")]
        public override object Credential { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Get current")]
        public SwitchParameter Current { get; set; }

        protected override void ProcessRecord()
        {
            if (Current.IsPresent)
            {
                WriteObject(CurrentConnectionService.TeamProjectCollection);
                return;
            }

            WriteObject(GetCollections(Collection, Server, Credential), true);
        }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }
    }
}