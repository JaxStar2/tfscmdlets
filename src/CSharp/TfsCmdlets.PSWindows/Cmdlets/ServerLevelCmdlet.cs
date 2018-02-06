using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Providers;

namespace TfsCmdlets.Cmdlets
{
    public abstract class ServerLevelCmdlet : BaseCmdlet
    {
        protected TfsConfigurationServer GetServer()
        {
            return GetServer(Server, Credential);
        }

        protected IEnumerable<TfsConfigurationServer> GetServers()
        {
            return GetServers(Server, Credential);
        }

        protected TfsConfigurationServer GetServer(object server, object credential)
        {
            return ServerProvider.GetServer(server, credential);
        }

        protected IEnumerable<TfsConfigurationServer> GetServers(object server, object credential)
        {
            return ServerProvider.GetServers(server, credential);
        }

        public abstract object Server { get; set; }

        public abstract object Credential { get; set; }

        private IServerProvider ServerProvider { get; set; }

    }
}
