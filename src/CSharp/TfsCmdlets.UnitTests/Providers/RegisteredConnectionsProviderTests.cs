using System;
using System.Linq;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Client.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TfsCmdlets.Services.Impl;

namespace TfsCmdlets.UnitTests.Providers
{
    [TestClass]
    public class RegisteredConnectionsProviderTests
    {
        [TestMethod]
        public void GetAllRegisteredServers()
        {
            using (ShimsContext.Create())
            {
                var registeredServers = SampleData.GetRegisteredServers();

                ShimRegisteredTfsConnections.GetConfigurationServers = () => registeredServers;

                var provider = new RegisteredConnectionServiceImpl();

                CollectionAssert.AreEqual(provider.GetRegisteredConfigurationServers().ToArray(), registeredServers);
            }
        }

        [TestMethod]
        public void GetOneRegisteredServer()
        {
            using (ShimsContext.Create())
            {
                var registeredServers = SampleData.GetRegisteredServers();

                ShimRegisteredTfsConnections.GetConfigurationServers = () => registeredServers;

                var provider = new RegisteredConnectionServiceImpl();

                var actual = provider.GetRegisteredConfigurationServers("BarServer1").ToList();

                Assert.AreEqual(actual.Count, 1);
                Assert.AreEqual(actual[0], registeredServers[1]);
            }
        }
    }
}
