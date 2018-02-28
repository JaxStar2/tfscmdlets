using System.Collections.Generic;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface ITeamProjectCollectionService
    {
        ITfsTeamProjectCollectionAdapter GetCollection(object collection, object server, object credential,
            bool ensureAuthenticated = false);

        IEnumerable<ITfsTeamProjectCollectionAdapter> GetCollections(object collections, object server, object credential);
    }
}
