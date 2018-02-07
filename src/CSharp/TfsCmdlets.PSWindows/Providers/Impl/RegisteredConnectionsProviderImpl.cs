using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Providers.Impl
{
    [Export(typeof(IRegisteredConnectionsProvider))]
    internal sealed class RegisteredConnectionsProviderImpl: IRegisteredConnectionsProvider
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
            var pattern = collectionName.Equals("localhost", StringComparison.OrdinalIgnoreCase) || collectionName.Equals(".")
                ? Environment.MachineName
                : collectionName;

            return RegisteredTfsConnections.GetProjectCollections().Where(o => o.Name.IsLike(pattern));
        }
    }
}
