using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Services
{
    public interface ITeamProjectCollectionService
    {
        TfsTeamProjectCollection GetCollection(object collection, object server, object credential);

        IEnumerable<TfsTeamProjectCollection> GetCollections(object collections, object server, object credential);
    }
}
