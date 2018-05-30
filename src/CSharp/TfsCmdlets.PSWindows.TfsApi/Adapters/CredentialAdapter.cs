using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class CredentialAdapter: AdapterBase<VssCredentials>, ICredentialAdapter
    {
        public CredentialAdapter(VssCredentials innerInstance) : base(innerInstance)
        {
        }
    }
}
