using System;
using System.Linq;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Client.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TfsCmdlets.Cmdlets.ConfigurationServer;

namespace TfsCmdlets.UnitTests.Cmdlets.ConfigurationServer
{
    [TestClass]
    public class GetRegisteredConfigurationServerTests
    {
        [TestMethod]
        public void GetFromExistingName()
        {
            using (ShimsContext.Create())
            {
                MockHelper.MockRegisteredTfsConnections();

                var cmdlet = new GetRegisteredConfigurationServer
                {
                    Name = "fooserver1"
                };

                var result = cmdlet.Invoke<RegisteredConfigurationServer>().ToList();

                Assert.AreEqual(result[0].Uri, SampleData.GetRegisteredServers().ToList()[0].Uri);
            }
        }

        [TestMethod]
        public void GetAllRegisteredServers()
        {
            using (ShimsContext.Create())
            {
                MockHelper.MockRegisteredTfsConnections();

                var cmdlet = new GetRegisteredConfigurationServer();

                var result = cmdlet.Invoke<RegisteredConfigurationServer>().ToList();

                CollectionAssert.AreEqual(result, SampleData.GetRegisteredServers(), new RegisteredConfigurationServerComparer());
            }
        }
    }
}
