using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Services
{
    public interface IRegisteredConnectionService
    {
        IEnumerable<RegisteredConfigurationServer> GetRegisteredConfigurationServers(string pattern);

        IEnumerable<RegisteredProjectCollection> GetRegisteredProjectCollections(string pattern);
    }
}
