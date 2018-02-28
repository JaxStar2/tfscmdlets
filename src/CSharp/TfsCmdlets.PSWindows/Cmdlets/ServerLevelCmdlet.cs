using System.Collections.Generic;
using System.ComponentModel.Composition;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets
{
    public abstract class ServerLevelCmdlet : BaseCmdlet
    {
        protected ITfsConfigurationServerAdapter GetServer(bool ensureAuthenticated = false)
        {
            return GetServer(Server, Credential, ensureAuthenticated);
        }

        protected IEnumerable<ITfsConfigurationServerAdapter> GetServers()
        {
            return GetServers(Server, Credential);
        }

        protected ITfsConfigurationServerAdapter GetServer(object server, object credential, bool ensureAuthenticated = false)
        {
            return ConfigurationServerService.GetServer(server, credential, ensureAuthenticated);
        }

        protected IEnumerable<ITfsConfigurationServerAdapter> GetServers(object server, object credential)
        {
            return ConfigurationServerService.GetServers(server, credential);
        }

        public abstract object Server { get; set; }

        public abstract object Credential { get; set; }

        [Import(typeof(IConfigurationServerService))]
        private IConfigurationServerService ConfigurationServerService { get; set; }

        [Import(typeof(ICredentialService))]
        protected ICredentialService CredentialService { get; set; }
    }
}
