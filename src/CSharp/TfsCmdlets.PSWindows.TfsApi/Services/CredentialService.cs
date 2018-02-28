using System.ComponentModel.Composition;
using System.Management.Automation;
using System.Net;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(ICredentialService))]
    public class CredentialService: ICredentialService
    {
        public ICredentialAdapter GetCredential(object credential)
        {
            NetworkCredential netCred;
            object creds;

            while (true)
            {
                switch (credential)
                {
                    case bool interactive:
                    {
                        creds = new VssClientCredentials(new WindowsCredential(!interactive),
                            new VssFederatedCredential(!interactive),
                            interactive ? CredentialPromptType.PromptIfNeeded : CredentialPromptType.DoNotPrompt);
                        break;
                    }
                    case PSObject pso:
                    {
                        credential = pso.BaseObject;
                        continue;
                    }
                    case VssCredentials vssc:
                    {
                        creds = vssc;
                        break;
                    }
                    case PSCredential psc:
                    {
                        netCred = psc.GetNetworkCredential();
                        creds = new VssClientCredentials(new WindowsCredential(netCred), new VssBasicCredential(netCred));
                        break;
                    }
                    case NetworkCredential netc:
                    {
                        netCred = netc;
                        creds = new VssClientCredentials(new WindowsCredential(netCred), new VssBasicCredential(netCred));
                        break;
                    }
                    case null:
                    {
                        credential = false;
                        continue;
                    }
                    default:
                    {
                        throw new PSArgumentException(
                            "Invalid argument Credential. Supply either a PowerShell credential (PSCredential object) or a System.Net.NetworkCredential object.",
                            "Credential");
                    }
                }
                break;
            }

            return new CredentialAdapter((VssClientCredentials) creds);
        }
    }
}
