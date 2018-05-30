using System.ComponentModel.Composition;
using System.Management.Automation;
using System.Net;
using System.Security;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

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
    [OutputType("Microsoft.VisualStudio.Services.Common.VssCredentials,Microsoft.VisualStudio.Services.Common")]
    public class GetCredential : BaseCmdlet
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
            ICredentialAdapter creds;

            if (Cached)
            {
                creds = CredentialService.GetCredential(false);
            }
            else if (UserName != null && Password != null)
            {
                var netCred = new NetworkCredential(UserName, Password);
                creds = CredentialService.GetCredential(netCred);
            }
            else if (Credential != null)
            {
                creds = CredentialService.GetCredential(Credential);
            }
            else if (PersonalAccessToken != null)
            {
                var netCred = new NetworkCredential(string.Empty, PersonalAccessToken);
                creds = CredentialService.GetCredential(netCred);
            }
            else if (Interactive)
            {
                creds = CredentialService.GetCredential(true);
            }
            else
            {
                throw new PSArgumentException($"Invalid parameter combination");
            }

            WriteObject(creds);
        }

        [Import(typeof(ICredentialService))]
        private ICredentialService CredentialService { get; set; }
    }
}