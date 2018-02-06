using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Cmdlets
{
    public abstract class ServerLevelCmdlet : BaseCmdlet
    {
        public abstract object Server { get; set; }

        public abstract object Credential { get; set; }

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
            return Provider.GetServer(server, credential);
        }

        protected IEnumerable<TfsConfigurationServer> GetServers(object server, object credential)
        {
            return Provider.GetServers(server, credential);
        }
    }
}
