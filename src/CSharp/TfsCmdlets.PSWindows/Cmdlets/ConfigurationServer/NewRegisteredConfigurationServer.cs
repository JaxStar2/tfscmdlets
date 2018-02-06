using System.Management.Automation;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Cmdlets.ConfigurationServer
{
    [Cmdlet(VerbsCommon.New, "RegisteredConfigurationServer", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    public class NewRegisteredConfigurationServer: ServerLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var server = GetServer();

            if (ShouldProcess(server?.Name, "Add server to list of registered servers"))
                RegisteredTfsConnections.RegisterConfigurationServer(server);
        }

        [Parameter]
        public override object Server { get; set; }

        [Parameter]
        public override object Credential { get; set; }
    }
}
