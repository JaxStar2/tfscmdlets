using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.Connection
{
    /// <summary>
    /// <para type="synopsis">Connects to a configuration server.</para>
    /// <para type="description">The Connect-TfsConfigurationServer function connects to a TFS configuration server. 
    ///     Functions that operate on a server level (as opposed to those operation on a team project collection level) 
    ///     will use by default a connection opened by this function.</para>
    /// </summary>
    [Cmdlet(VerbsCommunications.Connect, "ConfigurationServer", DefaultParameterSetName = "Explicit credentials")]
    [OutputType("Microsoft.TeamFoundation.Client.TfsConfigurationServer,Microsoft.TeamFoundation.Client")]
    public class ConnectConfigurationServer : ServerLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (Interactive.IsPresent)
            {
                Credential = CredentialService.GetCredential(true);
            }

            var configServer = GetServer(true);
            CurrentConnectionService.ConfigurationServer = configServer;

            if (Passthru)
            {
                WriteObject(configServer);
            }
        }

        /// <summary>
        /// <para type="description">${HelpParam_Server}</para>
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateNotNull]
        public override object Server { get; set; }

        /// <summary>
        /// <para type="description">${HelpParam_Credential}</para>
        /// </summary>
        [Parameter(ParameterSetName = "Explicit credentials")]
        public override object Credential { get; set; }

        /// <summary>
        /// <para type="description">${HelpParam_Interactive}</para>
        /// </summary>
        [Parameter(ParameterSetName = "Prompt for credentials", Mandatory = true)]
        public SwitchParameter Interactive { get; set; }

        /// <summary>
        /// <para type="description">${HelpParam_Passthru}</para>
        /// </summary>
        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }
    }
}