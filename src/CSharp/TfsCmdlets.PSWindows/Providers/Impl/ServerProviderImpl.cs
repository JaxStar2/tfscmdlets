using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Cmdlets.ConfigurationServer;
using TfsCmdlets.Cmdlets.Connection;
using TfsCmdlets.Cmdlets.TeamProjectCollection;

namespace TfsCmdlets.Providers.Impl
{
    [Export(typeof(IServerProvider))]
    internal sealed class ServerProviderImpl : IServerProvider
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
                        var serverNames = GetRegisteredConfigurationServer.Get(s);

                        foreach (var svr in serverNames)
                        {
                            yield return new TfsConfigurationServer(svr.Uri, cred);
                        }
                        break;
                    }
                    case null when CurrentConnections.ConfigurationServer != null:
                    {
                        yield return CurrentConnections.ConfigurationServer;
                        break;
                    }
                    default:
                    {
                        throw new PSArgumentException("No connection information available. " +
                            "Either supply a valid -Server argument or use Connect-TfsConfigurationServer " +
                            "prior to invoking this cmdlet.");
                    }
                }
                break;
            }
        }
    }
}