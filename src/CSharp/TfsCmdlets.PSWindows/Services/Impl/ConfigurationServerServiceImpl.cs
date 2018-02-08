using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Cmdlets.Connection;

namespace TfsCmdlets.Services.Impl
{
    [Export(typeof(IConfigurationServerService))]
    internal sealed class ConfigurationServerServiceImpl : IConfigurationServerService
    {
        public TfsConfigurationServer GetServer(object server, object credential)
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

        public IEnumerable<TfsConfigurationServer> GetServers(object server, object credential)
        {
            while (true)
            {
                var cred = GetCredential.Get(credential);

                switch (server)
                {
                    case PSObject pso:
                    {
                        server = pso.BaseObject;
                        continue;
                    }
                    case TfsConfigurationServer s:
                    {
                        yield return s;
                        break;
                    }
                    case Uri u:
                    {
                        yield return new TfsConfigurationServer(u, cred);
                        break;
                    }
                    case string s when Uri.IsWellFormedUriString(s, UriKind.Absolute):
                    {
                        yield return new TfsConfigurationServer(new Uri(s), cred);
                        break;
                    }
                    case string s when !string.IsNullOrWhiteSpace(s):
                    {
                        var servers = RegisteredConnectionService.GetRegisteredConfigurationServers(s);

                        foreach (var svr in servers)
                        {
                            yield return new TfsConfigurationServer(svr.Uri, cred);
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
    }
}