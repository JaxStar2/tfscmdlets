using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Services;

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
            return ConfigurationServerService.GetServer(server, credential);
        }

        protected IEnumerable<TfsConfigurationServer> GetServers(object server, object credential)
        {
            return ConfigurationServerService.GetServers(server, credential);
        }

        public abstract object Server { get; set; }

        public abstract object Credential { get; set; }

        [Import(typeof(IConfigurationServerService))]
        private IConfigurationServerService ConfigurationServerService { get; set; }

    }
}
