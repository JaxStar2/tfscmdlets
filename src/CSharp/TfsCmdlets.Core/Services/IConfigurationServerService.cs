using System.Collections.Generic;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IConfigurationServerService
    {
        ITfsConfigurationServerAdapter GetServer(object server, object credential, bool ensureAuthenticated);

        IEnumerable<ITfsConfigurationServerAdapter> GetServers(object servers, object credential);
    }
}
