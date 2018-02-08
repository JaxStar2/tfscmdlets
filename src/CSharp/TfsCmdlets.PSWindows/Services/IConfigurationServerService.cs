using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Services
{
    public interface IConfigurationServerService
    {
        TfsConfigurationServer GetServer(object server, object credential);

        IEnumerable<TfsConfigurationServer> GetServers(object servers, object credential);
    }
}
