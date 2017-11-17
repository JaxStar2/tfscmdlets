using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using TfsCmdlets.Cmdlets.Connection;

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

    [Cmdlet(verbName: VerbsCommon.Get, nounName: "ConfigurationServer", DefaultParameterSetName = "Get by server")]
    [OutputType(typeof(Microsoft.TeamFoundation.Client.TfsConfigurationServer))]
    public class GetConfigurationServer : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, ParameterSetName = "Get by server", ValueFromPipeline = true)]
        [AllowNull]
        public object Server { get; set; } = "*";

        [Parameter(Position = 0, ParameterSetName = "Get current")]
        public SwitchParameter Current { get; set; }

        [Parameter(Position = 1, ParameterSetName = "Get by server")]
        public object Credential { get; set; }

        #endregion

        protected override void ProcessRecord()
        {
            if (Current.IsPresent)
            {
                WriteObject(CurrentConnections.ConfigurationServer);
                return;
            }

            var progressId = (new Random()).Next();

            foreach (var server in Get(Server, Credential))
            {
                WriteProgress(new ProgressRecord(progressId, "Getting configuration servers", $"Getting {server.Name}"));
                WriteObject(server);
            }
        }

        internal static IEnumerable<TfsConfigurationServer> Get(object server = null, object credential = null)
        {
            var cred = GetCredential.Get(credential);

            switch (server)
            {
                case PSObject pso:
                {
                    foreach (var svr in Get(pso.BaseObject))
                    {
                        yield return svr;
                    }
                    break;
                }
                case TfsConfigurationServer s:
                {
                    yield return s;
                    break;
                }
                case Uri u:
                {
                    yield return new TfsConfigurationServer(u, cred);
                    break;
                }
                case string s when Uri.IsWellFormedUriString(s, UriKind.Absolute):
                {
                    yield return new TfsConfigurationServer(new Uri(s), cred);
                    break;
                }
                case string s when !string.IsNullOrWhiteSpace(s):
                {
                    var serverNames = GetRegisteredConfigurationServer.Get(s);

                    foreach (var svr in serverNames)
                    {
                        yield return new TfsConfigurationServer(svr.Uri, cred);
                    }
                    break;
                }
                case null when (CurrentConnections.ConfigurationServer != null):
                {
                    yield return CurrentConnection;
                    break;
                }
                default:
                {
                    throw new PSArgumentException(
                        "No TFS connection information available. Either supply a valid -Server argument or use Connect-TfsConfigurationServer prior to invoking this cmdlet.");
                }
            }
        }

        public static TfsConfigurationServer CurrentConnection
        {
            get => CurrentConnections.ConfigurationServer;
            set => CurrentConnections.ConfigurationServer = value;
        }
    }
}