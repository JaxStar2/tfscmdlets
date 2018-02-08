using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TfsCmdlets.Cmdlets.ConfigurationServer;
using TfsCmdlets.Services.Impl;

namespace TfsCmdlets.UnitTests.Providers
{
    [TestClass]
    public class ServerProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetServerThrowsForNullArguments()
        {
            var newContainer = new CompositionContainer(new AssemblyCatalog(typeof(ServiceLocator).Assembly));
            var oldContainer = ServiceLocator.SetContainer(newContainer);

            try
            {
                var provider = new ConfigurationServerServiceImpl();
                provider.Compose();

                provider.GetServer(null, null);
            }
            finally
            {
                ServiceLocator.SetContainer(oldContainer);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetServersThrowsForNullArguments()
        {
            var newContainer = new CompositionContainer(new AssemblyCatalog(typeof(ServiceLocator).Assembly));
            var oldContainer = ServiceLocator.SetContainer(newContainer);

            try
            {
                var provider = new ConfigurationServerServiceImpl();
                provider.Compose();

                provider.GetServers(null, null).ToList();
            }
            finally
            {
                ServiceLocator.SetContainer(oldContainer);
            }
        }

        [TestMethodAttribute]
        public void GetFromTfsConfigServerObject()
        {
            var configServer = new TfsConfigurationServer(new Uri("http://foo:8080/tfs"));
            var provider = new ConfigurationServerServiceImpl();

            var result = provider.GetServer(configServer, null);

            Assert.AreEqual(result, configServer);
        }

        [TestMethod]
        public void GetFromUri()
        {
            var configServerUrl = new Uri("http://foo:8080/tfs");
            var provider = new ConfigurationServerServiceImpl();

            var result = provider.GetServer(configServerUrl, null);

            Assert.AreEqual(result.Uri, configServerUrl);
        }

        [TestMethod]
        public void GetFromStringUri()
        {
            const string configServerUrl = "http://foo:8080/tfs";
            var provider = new ConfigurationServerServiceImpl();

            var result = provider.GetServer(configServerUrl, null);

            Assert.AreEqual(result.Uri, new Uri(configServerUrl));
        }

        [TestMethodAttribute]
        public void UnwindPsObject()
        {
            var configServer = new TfsConfigurationServer(new Uri("http://foo:8080/tfs"));
            var pso = new PSObject(configServer);

            var cmdlet = new GetConfigurationServer
            {
                Server = pso
            };

            var result = cmdlet.Invoke<TfsConfigurationServer>().ToList();

            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0], configServer);
        }
    }
}
