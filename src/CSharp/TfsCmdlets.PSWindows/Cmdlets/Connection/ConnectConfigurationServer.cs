using System.ComponentModel.Composition;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Services;

namespace TfsCmdlets.Cmdlets.Connection
{
    [Cmdlet(VerbsCommunications.Connect, "ConfigurationServer", DefaultParameterSetName = "Explicit credentials")]
    [OutputType(typeof(TfsConfigurationServer))]
    public class ConnectConfigurationServer : ServerLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (Interactive.IsPresent)
            {
                Credential = GetCredential.Get(true);
            }

            var configServer = GetServer();
            configServer.EnsureAuthenticated();

            CurrentConnectionService.ConfigurationServer = configServer;

            if (Passthru)
            {
                WriteObject(configServer);
            }
        }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateNotNull]
        public override object Server { get; set; }

        [Parameter(ParameterSetName = "Explicit credentials")]
        public override object Credential { get; set; }

        [Parameter(ParameterSetName = "Prompt for credentials", Mandatory = true)]
        public SwitchParameter Interactive { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }
    }
}