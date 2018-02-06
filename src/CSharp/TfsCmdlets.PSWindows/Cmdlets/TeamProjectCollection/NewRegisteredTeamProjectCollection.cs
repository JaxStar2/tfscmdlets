using System.Management.Automation;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsCommon.New, "RegisteredTeamProjectCollection", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    public class NewRegisteredTeamProjectCollection: CollectionLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var collection = GetCollection();

            if (ShouldProcess(collection?.Name, "Add server to list of registered servers"))
                RegisteredTfsConnections.RegisterProjectCollection(collection);
        }

        [Parameter(Position=0, Mandatory=true, ValueFromPipeline = true)]
        public override object Collection { get; set; }
    }
}
