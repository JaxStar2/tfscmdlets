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
    /// <list type="alertSet">
    ///   <item>
    ///     <description>A TFS Configuration Server represents the server that is running Team Foundation Server. 
    ///         On a database level, it is represented by the Tfs_Configuration database. Operations that should be 
    ///         performed on a server level (such as setting server-level permissions) require a connection to a TFS 
    ///         configuration server. Internally, this connection is represented by an instance of the 
    ///         Microsoft.TeamFoundation.Client.TfsConfigurationServer class and is kept in a PowerShell global variable 
    ///         caled TfsServerConnection.</description>
    ///   </item>
    /// </list>
    /// <example>
    ///   <code>PS&gt; Connect-TfsConfigurationServer -Server http://vsalm:8080/tfs </code>
    ///   <para>Connects to the TFS server specified by the URL in the Server argument</para>
    /// </example>
    /// <example>
    ///   <code>PS&gt; Connect-TfsConfigurationServer -Server vsalm</code>
    ///   <para>Connects to a previously registered TFS server by its user-defined name "vsalm". For more information, see Get-TfsRegisteredConfigurationServer</para>
    /// </example>
    /// <para type="link">Microsoft.TeamFoundation.Client.TfsConfigurationServer</para>
    /// <para type="link">https://blogs.msdn.microsoft.com/taylaf/2010/02/23/introducing-the-tfsconnection-tfsconfigurationserver-and-tfsteamprojectcollection-classes/</para>
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
        /// <para type="inputType">Microsoft.TeamFoundation.Client.TfsConfigurationServer, System.String, System.Uri</para>
        /// <para type="inputType">You can pipe a TfsConfigurationServer object, a registered server name or a URL pointing to a TFS instance (either as a String or Uri object)</para>
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