using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IRegisteredConnectionService))]
    public class RegisteredConnectionService : IRegisteredConnectionService
    {
        public IEnumerable<IRegisteredConfigurationServerAdapter> GetRegisteredConfigurationServers(string serverName = "*")
        {
            var pattern = serverName.Equals("localhost", StringComparison.OrdinalIgnoreCase) || serverName.Equals(".")
                ? Environment.MachineName
                : serverName;

            foreach (var r in RegisteredTfsConnections.GetConfigurationServers().Where(o => o.Name.IsLike(pattern)))
            {
                yield return new RegisteredConfigurationServerAdapter(r);
            }
        }

        public IEnumerable<IRegisteredProjectCollectionAdapter> GetRegisteredProjectCollections(string collectionName)
        {
            foreach (var r in RegisteredTfsConnections.GetProjectCollections()
                .Where(o => o.Name.IsLike(collectionName)))
            {
                yield return new RegisteredProjectCollectionAdapter(r);
            }
        }

        public void RegisterConfigurationServer(ITfsConfigurationServerAdapter server)
        {
            throw new NotImplementedException();
        }

        public void RegisterProjectCollection(ITfsTeamProjectCollectionAdapter collection)
        {
            throw new NotImplementedException();
        }

        public void UnregisterConfigurationServer(string name)
        {
            throw new NotImplementedException();
        }

        public void UnregisterProjectCollection(string name)
        {
            throw new NotImplementedException();
        }
    }
}
