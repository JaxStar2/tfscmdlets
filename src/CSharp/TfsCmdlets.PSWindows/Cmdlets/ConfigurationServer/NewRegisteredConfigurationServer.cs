using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Services;

namespace TfsCmdlets.Cmdlets.ConfigurationServer
{
    [Cmdlet(VerbsCommon.New, "RegisteredConfigurationServer", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType(typeof(RegisteredConfigurationServer))]
    public class NewRegisteredConfigurationServer : ServerLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var server = GetServer();

            if (!ShouldProcess(server?.Name, "Add server to list of registered servers")) return;

            RegisteredTfsConnections.RegisterConfigurationServer(server);

            if (Passthru)
            {
                WriteObject(RegisteredConnectionService.GetRegisteredConfigurationServers("*")
                    .First(o => o.Uri == server.Uri));
            }
        }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Parameter]
        public override object Server { get; set; }

        [Parameter]
        public override object Credential { get; set; }

        [Import(typeof(IRegisteredConnectionService))]
        private IRegisteredConnectionService RegisteredConnectionService { get; set; }
    }
}
