using System.Management.Automation;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Cmdlets.ConfigurationServer
{
    [Cmdlet(VerbsCommon.Remove, "RegisteredConfigurationServer", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess=true)]
    public class RemoveRegisteredConfigurationServer: Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(Name, "Remove registered server"))
                return;

            RegisteredTfsConnections.UnregisterConfigurationServer(Name);
        }
    }
}
