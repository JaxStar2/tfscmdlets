using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Client.Fakes;

namespace TfsCmdlets.UnitTests
{
    internal static class SampleData
    {
        public static RegisteredConfigurationServer[] GetRegisteredServers()
        {
            return new RegisteredConfigurationServer[]
            {
                new ShimRegisteredConfigurationServer
                {
                    NameGet = () => "FooServer1",
                    UriGet = () => new Uri("http://fooserver1:8080/tfs"),
                    InstanceIdGet = () => new Guid("6B204078-55BF-4B00-A648-E2C3A2030976")
                }.Instance,
                new ShimRegisteredConfigurationServer
                {
                    NameGet = () => "BarServer1",
                    UriGet = () => new Uri("http://barserver1:8080/tfs"),
                    InstanceIdGet = () => new Guid("9EF7A4FF-6FD9-4958-8F85-A368FF79938A")
                }.Instance,
            };
        }
    }
}
