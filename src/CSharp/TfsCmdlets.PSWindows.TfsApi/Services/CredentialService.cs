using System;
using System.Collections.Generic;
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
    public class CredentialService : ServiceBase<VssCredentials, ICredentialAdapter>, ICredentialService
    {
        protected override IEnumerable<VssCredentials> GetAllItems(object item, ScopeObjects so)
        {
            while (true)
            {
                switch (item)
                {
                    case bool interactive:
                        {
                            item = new VssClientCredentials(new WindowsCredential(!interactive),
                                new VssFederatedCredential(!interactive),
                                interactive ? CredentialPromptType.PromptIfNeeded : CredentialPromptType.DoNotPrompt);
                            continue;
                        }
                    case PSCredential psc:
                        {
                            var netCred = psc.GetNetworkCredential();
                            item = new VssClientCredentials(new WindowsCredential(netCred), new VssBasicCredential(netCred));
                            continue;
                        }
                    case NetworkCredential netc:
                        {
                            var netCred = netc;
                            item = new VssClientCredentials(new WindowsCredential(netCred), new VssBasicCredential(netCred));
                            continue;
                        }
                    case null:
                        {
                            item = false;
                            continue;
                        }
                    case VssCredentials vssc:
                        {
                            yield return vssc;
                            break;
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
        }

        public ICredentialAdapter GetCredential(object credential)
        {
            return new CredentialAdapter(GetItem(credential));
        }

        protected override bool SupportsEmptySearch => true;
    }
}
