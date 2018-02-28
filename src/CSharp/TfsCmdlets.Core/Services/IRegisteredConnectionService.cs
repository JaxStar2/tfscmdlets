using System.Collections.Generic;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IRegisteredConnectionService
    {
        IEnumerable<IRegisteredConfigurationServerAdapter> GetRegisteredConfigurationServers(string pattern);

        IEnumerable<IRegisteredProjectCollectionAdapter> GetRegisteredProjectCollections(string pattern);

        void RegisterConfigurationServer(ITfsConfigurationServerAdapter server);

        void RegisterProjectCollection(ITfsTeamProjectCollectionAdapter collection);

        void UnregisterConfigurationServer(string name);

        void UnregisterProjectCollection(string name);

    }
}
