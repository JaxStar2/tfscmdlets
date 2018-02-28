using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class TfsTeamProjectCollectionAdapter: AdapterBase<TfsTeamProjectCollection>, ITfsTeamProjectCollectionAdapter
    {
        public static implicit operator TfsTeamProjectCollectionAdapter(TfsTeamProjectCollection innerInstance)
        {
            return new TfsTeamProjectCollectionAdapter(innerInstance);
        }

        public static implicit operator TfsTeamProjectCollection(TfsTeamProjectCollectionAdapter adapter)
        {
            return adapter.InnerInstance;
        }

        public TfsTeamProjectCollectionAdapter(TfsTeamProjectCollection innerInstance) 
            : base(innerInstance)
        {
        }

        public string Name => InnerInstance.Name;
        public Uri Uri => InnerInstance.Uri;
    }
}
