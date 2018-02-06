using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Providers
{
    public interface ICollectionProvider
    {
        TfsTeamProjectCollection GetCollection(object collection, object server, object credential);

        IEnumerable<TfsTeamProjectCollection> GetCollections(object collections, object server, object credential);
    }
}
