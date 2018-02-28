using System;
using System.Collections.Generic;
using System.Text;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface ICredentialService
    {
        ICredentialAdapter GetCredential(object credential);
    }
}
