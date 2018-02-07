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

namespace TfsCmdlets.UnitTests.Cmdlets.ConfigurationServer
{
    [TestClass]
    public class GetConfigurationServerTests
    {
        [TestMethod]
        public void ReturnsCurrentConnection()
        {
            try
            {
                var configServer = new TfsConfigurationServer(new Uri("http://foo:8080/tfs"));
                CurrentConnections.ConfigurationServer = configServer;

                var cmdlet = new GetConfigurationServer
                {
                    Current = true
                };

                var result = cmdlet.Invoke<TfsConfigurationServer>().ToList();

                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(result[0], configServer);

            }
            finally
            {
                CurrentConnections.ConfigurationServer = null;
            }
        }

    }
}
