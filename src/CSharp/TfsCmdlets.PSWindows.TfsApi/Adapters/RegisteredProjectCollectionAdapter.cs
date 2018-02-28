using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class RegisteredProjectCollectionAdapter: AdapterBase<RegisteredProjectCollection>, IRegisteredProjectCollectionAdapter
    {
        public RegisteredProjectCollectionAdapter(RegisteredProjectCollection innerInstance) : base(innerInstance)
        {
        }

        public Uri Uri => InnerInstance.Uri;
    }
}
