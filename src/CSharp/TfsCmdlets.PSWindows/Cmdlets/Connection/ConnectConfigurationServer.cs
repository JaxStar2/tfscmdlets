using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Cmdlets.ConfigurationServer;

namespace TfsCmdlets.Cmdlets.Connection
{
    [Cmdlet(VerbsCommunications.Connect, "ConfigurationServer", DefaultParameterSetName = "Explicit credentials")]
    [OutputType(typeof(TfsConfigurationServer))]
    public class ConnectConfigurationServer : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            if (Interactive.IsPresent)
            {
                Credential = GetCredential.Get(true);
            }

            var configServers = GetConfigurationServer.Get(Server, Credential).ToList();

            if (configServers.Count == 0)
            {
                throw new PSArgumentException(
                    $"Invalid server name and/or URL '{Server}'", "Server");
            }            
            else if (configServers.Count > 1)
            {
                var serverList = string.Join(", ", configServers.Select(svr => $"{svr.Name} ({svr.Uri})").ToArray());
                throw new PSArgumentException($"Ambiguous name '{Server}' matches {configServers.Count} servers: {serverList}. Please choose a more specific value for the Server argument and try again", nameof(Server));
            }

            var configServer = configServers[0];
            configServer.EnsureAuthenticated();

            CurrentConnections.ConfigurationServer = configServer;

            if (Passthru)
            {
                WriteObject(configServer);
            }
        }

        #region Parameters

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateNotNull]
        public object Server { get; set; }

        [Parameter(ParameterSetName = "Explicit credentials")]
        public object Credential { get; set; }

        [Parameter(ParameterSetName = "Prompt for credentials", Mandatory = true)]
        public SwitchParameter Interactive { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        #endregion
    }
}