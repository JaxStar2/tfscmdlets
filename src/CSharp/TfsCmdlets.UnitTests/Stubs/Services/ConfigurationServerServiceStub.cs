using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Services;

namespace TfsCmdlets.UnitTests.Stubs.Services
{
    [Export(typeof(IConfigurationServerService))]
    internal sealed class ConfigurationServerServiceStub : IConfigurationServerService
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
            return new List<TfsConfigurationServer>
            {
                new TfsConfigurationServer(new Uri("http://foo:8080/tfs")),
                new TfsConfigurationServer(new Uri("http://bar:8080/tfs")),
                new TfsConfigurationServer(new Uri("http://baz:8080/tfs"))
            };
        }
    }
}