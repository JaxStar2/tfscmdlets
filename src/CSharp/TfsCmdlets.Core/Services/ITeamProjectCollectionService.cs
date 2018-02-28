using System;
using System.Collections.Generic;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface ITeamProjectCollectionService
    {
        ITfsTeamProjectCollectionAdapter GetCollection(object collection, object server, object credential,
            bool ensureAuthenticated = false);

        IEnumerable<ITfsTeamProjectCollectionAdapter> GetCollections(object collections, object server, object credential);

        ITfsTeamProjectCollectionAdapter CreateCollection(string name, string description, string connectionString,
            bool isDefault, bool useExistingDatabase, ServiceHostStatus initialState, TimeSpan timeout, object server,
            object credential);

        void DeleteCollection(object collection, object server, object credential);

        ITfsTeamProjectCollectionAdapter AttachCollection(string name, string description, string connectionString, bool clone, TimeSpan timeout, object server, object credential);

        string DetachCollection(object collection, string reason, TimeSpan timeout, object server, object credential);
    }
}
