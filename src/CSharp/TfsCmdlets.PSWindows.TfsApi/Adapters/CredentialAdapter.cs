using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Services.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class CredentialAdapter: AdapterBase<VssClientCredentials>, ICredentialAdapter
    {
        public CredentialAdapter(VssClientCredentials innerInstance) : base(innerInstance)
        {
        }
    }
}
