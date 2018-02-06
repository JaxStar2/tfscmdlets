using Microsoft.VisualStudio.Services.Common;
using System.Management.Automation;
using System.Net;
using System.Security;
using Microsoft.VisualStudio.Services.Client;

namespace TfsCmdlets.Cmdlets.Connection
{
/*
    <#
.SYNOPSIS
    Provides credentials to use when you connect to a Team Foundation Server or Visual Studio Team Services account.

.DESCRIPTION

.NOTES

.INPUTS

#>*/
    [Cmdlet(VerbsCommon.Get, "Credential", DefaultParameterSetName = "Prompt for credential")]
    [OutputType(typeof(VssCredentials))]
    public class GetCredential : PSCmdlet
    {
        [Parameter(ParameterSetName = "Cached Credential", Mandatory = true)]
        public SwitchParameter Cached { get; set; }

        [Parameter(ParameterSetName = "User name and password", Mandatory = true, Position = 1)]
        [ValidateNotNullOrEmpty]
        public string UserName { get; set; }

        [Parameter(ParameterSetName = "User name and password", Mandatory = true, Position = 2)]
        [ValidateNotNull]
        public SecureString Password { get; set; }

        [Parameter(ParameterSetName = "Credential object")]
        [ValidateNotNull]
        public object Credential { get; set; }

        [Parameter(ParameterSetName = "Personal Access Token")]
        [ValidateNotNullOrEmpty]
        [Alias("Pat")]
        public string PersonalAccessToken { get; set; }

        [Parameter(ParameterSetName = "Prompt for credential")]
        public SwitchParameter Interactive { get; set; }

        protected override void ProcessRecord()
        {
            var parameterSetName = ParameterSetName;
            VssCredentials creds;

            switch (parameterSetName)
            {
                case "Cached Credential":
                {
                    creds = Get(false);
                    break;
                }

                case "User name and password":
                {
                    var netCred = new NetworkCredential(UserName, Password);
                    creds = Get(netCred);
                    break;
                }

                case "Credential object":
                {
                    creds = Get(Credential);
                    break;
                }
                case "Personal Access Token":
                {
                    var netCred = new NetworkCredential("dummy-pat-user", PersonalAccessToken);
                    creds = Get(netCred);
                    break;
                }

                case "Prompt for credential":
                {
                    creds = Get(true);
                    break;
                }
                default:
                {
                    throw new PSArgumentException($"Invalid parameter set {ParameterSetName}");
                }
            }

            WriteObject(creds);
        }

        internal static VssCredentials Get(object credentials = null)
        {
            NetworkCredential netCred;
            VssCredentials creds;

            switch (credentials)
            {
                case bool interactive:
                {
                    creds = new VssClientCredentials(new WindowsCredential(!interactive), new VssFederatedCredential(!interactive),
                        interactive ? CredentialPromptType.PromptIfNeeded : CredentialPromptType.DoNotPrompt);
                    break;
                }
                case PSObject pso:
                {
                    creds = Get(pso.BaseObject);
                    break;
                }
                case VssCredentials vssc:
                {
                    creds = vssc;
                    break;
                }
                case PSCredential psc:
                {
                    netCred = psc.GetNetworkCredential();
                    creds = new VssCredentials(new WindowsCredential(netCred), new VssBasicCredential(netCred));
                    break;
                }
                case NetworkCredential netc:
                {
                    netCred = netc;
                    creds = new VssCredentials(new WindowsCredential(netCred), new VssBasicCredential(netCred));
                    break;
                }
                case null:
                {
                    creds = Get(false);
                    break;
                }
                default:
                {
                    throw new PSArgumentException(
                        "Invalid argument Credential. Supply either a PowerShell credential (PSCredential object) or a System.Net.NetworkCredential object.",
                        "Credential");
                }
            }

            return creds;
        }
    }
}