using System.Management.Automation;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsCommon.Remove, "RegisteredTeamProjectCollection", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    public class RemoveRegisteredTeamProjectCollection:Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(Name, "Remove registered collection"))
                return;

            RegisteredTfsConnections.UnregisterProjectCollection(Name);
        }
    }
}
