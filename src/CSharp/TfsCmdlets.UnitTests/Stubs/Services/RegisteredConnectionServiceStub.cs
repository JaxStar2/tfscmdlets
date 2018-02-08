using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Client.Fakes;
using TfsCmdlets.Services;

namespace TfsCmdlets.UnitTests.Stubs.Services
{
    [Export(typeof(IRegisteredConnectionService))]
    internal sealed class RegisteredConnectionServiceStub : IRegisteredConnectionService
    {
        public IEnumerable<RegisteredConfigurationServer> GetRegisteredConfigurationServers(string serverName = "*")
        {
            var pattern = serverName.Equals("localhost", StringComparison.OrdinalIgnoreCase) || serverName.Equals(".")
                ? Environment.MachineName
                : serverName;

            return _servers.Where(o => o.Name.IsLike(pattern));
        }

        public IEnumerable<RegisteredProjectCollection> GetRegisteredProjectCollections(string collectionName)
        {
            return _collections.Where(o => o.Name.IsLike(collectionName));
        }

        private readonly List<RegisteredConfigurationServer> _servers = new List<RegisteredConfigurationServer>
        {
            new ShimRegisteredConfigurationServer
            {
                NameGet = () => "foo",
                UriGet = () => new Uri("http://foo:8080/tfs/"),
                InstanceIdGet = () => new Guid("EFF4585C-2C21-4D20-8FC0-42436955364E"),
                IsHostedGet = () => false
            }.Instance,
            new ShimRegisteredConfigurationServer
            {
                NameGet = () => "bar",
                UriGet = () => new Uri("http://bar:8080/tfs/"),
                InstanceIdGet = () => new Guid("18A89AFB-279E-4B7E-869A-28E1AAD21249"),
                IsHostedGet = () => false
            }.Instance,
            new ShimRegisteredConfigurationServer
            {
                NameGet = () => "baz",
                UriGet = () => new Uri("http://baz:8080/tfs/"),
                InstanceIdGet = () => new Guid("D37A0AF3-5B59-4849-AA78-80CCD3081E50"),
                IsHostedGet = () => false
            }.Instance
        };

        private readonly List<RegisteredProjectCollection> _collections = new List<RegisteredProjectCollection>
        {   
            new ShimRegisteredProjectCollection
            {
                NameGet = () => "foo\\FooCollection",
                DisplayNameGet = () => "FooCollection",
                UriGet = () => new Uri("http://foo:8080/tfs/FooCollection"),
                InstanceIdGet = () => new Guid("57DB0D69-EAC8-4240-AC5F-B3B032ED68FA"),
                OfflineGet = () => false,
                AutoReconnectGet = () => true
            }.Instance,
            new ShimRegisteredProjectCollection
            {
                NameGet = () => "bar\\BarCollection",
                DisplayNameGet = () => "BarCollection",
                UriGet = () => new Uri("http://bar:8080/tfs/BarCollection"),
                InstanceIdGet = () => new Guid("2B7CC9D2-9620-49E6-AD06-A6543D347678"),
                OfflineGet = () => false,
                AutoReconnectGet = () => true
            }.Instance,
            new ShimRegisteredProjectCollection
            {
                NameGet = () => "baz\\BazCollection",
                DisplayNameGet = () => "BazCollection",
                UriGet = () => new Uri("http://baz:8080/tfs/BazCollection"),
                InstanceIdGet = () => new Guid("7F6FDA01-00A6-4A30-9F46-2C5C00BB3DC9"),
                OfflineGet = () => false,
                AutoReconnectGet = () => true
            }.Instance
        };
    }
}
