using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Providers
{
    public interface IServerProvider
    {
        TfsConfigurationServer GetServer(object server, object credential);

        IEnumerable<TfsConfigurationServer> GetServers(object servers, object credential);
    }
}
