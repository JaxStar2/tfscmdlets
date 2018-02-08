using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Services.Impl
{
    [Export(typeof(IRegisteredConnectionService))]
    internal sealed class RegisteredConnectionServiceImpl: IRegisteredConnectionService
    {
        public IEnumerable<RegisteredConfigurationServer> GetRegisteredConfigurationServers(string serverName = "*")
        {
            var pattern = serverName.Equals("localhost", StringComparison.OrdinalIgnoreCase) || serverName.Equals(".")
                ? Environment.MachineName
                : serverName;

            return RegisteredTfsConnections.GetConfigurationServers().Where(o => o.Name.IsLike(pattern));
        }

        public IEnumerable<RegisteredProjectCollection> GetRegisteredProjectCollections(string collectionName)
        {
            return RegisteredTfsConnections.GetProjectCollections().Where(o => o.Name.IsLike(collectionName));
        }
    }
}
