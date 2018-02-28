using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.Services.Common;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IConfigurationServerService))]
    public sealed class ConfigurationServerService : IConfigurationServerService
    {
        public ITfsConfigurationServerAdapter GetServer(object server, object credential, bool ensureAuthenticated)
        {
            var servers = GetServers(server, credential).ToList();

            if (servers.Count == 0)
                throw new Exception($"Invalid server name '{server}'");

            if (servers.Count == 1)
                return servers[0];

            var names = string.Join(", ", servers.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{server}' matches {servers.Count} servers: {names}. " +
                                "Please choose a more specific value for the -Server argument and try again");
        }

        public IEnumerable<ITfsConfigurationServerAdapter> GetServers(object server, object credential)
        {
            while (true)
            {
                var cred = (VssCredentials) CredentialService.GetCredential(credential).Instance;

                switch (server)
                {
                    case PSObject pso:
                    {
                        server = pso.BaseObject;
                        continue;
                    }
                    case TfsConfigurationServer s:
                    {
                        yield return new TfsConfigurationServerAdapter(s);
                        break;
                    }
                    case Uri u:
                    {
                        yield return new TfsConfigurationServerAdapter(new TfsConfigurationServer(u, cred));
                        break;
                    }
                    case string s when Uri.IsWellFormedUriString(s, UriKind.Absolute):
                    {
                        yield return new TfsConfigurationServerAdapter(new TfsConfigurationServer(new Uri(s), cred));
                        break;
                    }
                    case string s when !string.IsNullOrWhiteSpace(s):
                    {
                        var servers = RegisteredConnectionService.GetRegisteredConfigurationServers(s);

                        foreach (var svr in servers)
                        {
                            yield return new TfsConfigurationServerAdapter(new TfsConfigurationServer(svr.Uri, cred));
                        }
                        break;
                    }
                    case null when CurrentConnectionService.ConfigurationServer != null:
                    {
                        yield return CurrentConnectionService.ConfigurationServer;
                        break;
                    }
                    default:
                    {
                        throw new Exception("No connection information available. " +
                            "Either supply a valid -Server argument or use Connect-TfsConfigurationServer " +
                            "prior to invoking this cmdlet.");
                    }
                }
                break;
            }
        }

        [Import(typeof(IRegisteredConnectionService))]
        private IRegisteredConnectionService RegisteredConnectionService { get; set; }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }

        [Import(typeof(ICredentialService))]
        private ICredentialService CredentialService { get; set; }
    }
}