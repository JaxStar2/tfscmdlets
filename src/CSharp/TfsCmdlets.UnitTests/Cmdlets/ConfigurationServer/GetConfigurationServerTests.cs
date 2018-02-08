using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TfsCmdlets.Cmdlets.ConfigurationServer;
using TfsCmdlets.Cmdlets.Connection;
using TfsCmdlets.Services;

namespace TfsCmdlets.UnitTests.Cmdlets.ConfigurationServer
{
    [TestClass]
    public class GetConfigurationServerTests
    {
        [TestMethod]
        public void ReturnsCurrentConnection()
        {
            var currentConnections = ServiceLocator.GetInstance<ICurrentConnectionService>();
            var configServer = new TfsConfigurationServer(new Uri("http://foo:8080/tfs"));

            currentConnections.ConfigurationServer = configServer;

            var cmdlet = new GetConfigurationServer
            {
                Current = true
            };

            var result = cmdlet.Invoke<TfsConfigurationServer>().ToList();

            Assert.AreEqual(result.Count, 1);
            Assert.AreSame(result[0], configServer);
        }
    }
}
