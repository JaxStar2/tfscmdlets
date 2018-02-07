using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Providers
{
    public interface IRegisteredConnectionsProvider
    {
        IEnumerable<RegisteredConfigurationServer> GetRegisteredConfigurationServers(string pattern);

        IEnumerable<RegisteredProjectCollection> GetRegisteredProjectCollections(string pattern);
    }
}
