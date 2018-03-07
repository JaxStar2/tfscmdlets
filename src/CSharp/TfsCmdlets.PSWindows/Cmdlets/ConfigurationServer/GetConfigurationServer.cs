using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.ConfigurationServer
{
    /*
    <#
    .SYNOPSIS
    Gets information about a configuration server.

    .PARAMETER Server
    {HelpParam_Server
    }

    .PARAMETER Current
    Returns the configuration server specified in the last call to Connect-TfsConfigurationServer(i.e.the "current" configuration server)

    .PARAMETER Credential
    {HelpParam_TfsCredential}

    .INPUTS
    Microsoft.TeamFoundation.Client.TfsConfigurationServer
    System.String
    System.Uri
    #>
    */

    [Cmdlet(VerbsCommon.Get, "ConfigurationServer", DefaultParameterSetName = "Get by server")]
    [OutputType("Microsoft.TeamFoundation.Client.TfsConfigurationServer,Microsoft.TeamFoundation.Client")]
    public class GetConfigurationServer : ServerLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (Current)
            {
                WriteObject(CurrentConnectionService.ConfigurationServer);
                return;
            }

            WriteObject(GetServers(Server, Credential), true);
        }

        [Parameter(Position = 0, ParameterSetName = "Get by server", ValueFromPipeline = true)]
        [ValidateNotNull]
        public override object Server { get; set; } = "*";

        [Parameter(Position = 0, ParameterSetName = "Get current")]
        public SwitchParameter Current { get; set; }

        [Parameter(Position = 1, ParameterSetName = "Get by server")]
        public override object Credential { get; set; }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }
    }
}