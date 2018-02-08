using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TfsCmdlets.Services;

namespace TfsCmdlets.UnitTests
{
    [TestClass]
    public class ServiceLocatorTests
    {
        [TestMethod]
        public void CanComposePublicProperty()
        {
            var obj = new ComposableObject();
            obj.Compose();

            Assert.IsNotNull(obj.PublicContainer);
        }

        [TestMethod]
        public void CanComposeInternalProperty()
        {
            var obj = new ComposableObject();
            obj.Compose();

            Assert.IsNotNull(obj.InternalContainer);
        }

        [TestMethod]
        public void CanComposeProtectedProperty()
        {
            var obj = new ComposableObject();
            obj.Compose();

            var fi = obj.GetType().GetProperty("ProtectedContainer", BindingFlags.NonPublic | BindingFlags.Instance);

            //ReSharper disable once PossibleNullReferenceException
            Assert.IsNotNull(fi.GetValue(obj));
        }

        [TestMethod]
        public void CanComposePrivateProperty()
        {
            var obj = new ComposableObject();
            obj.Compose();

            var fi = obj.GetType().GetProperty("PrivateContainer", BindingFlags.NonPublic | BindingFlags.Instance);

            //ReSharper disable once PossibleNullReferenceException
            Assert.IsNotNull(fi.GetValue(obj));
        }

        [TestMethod]
        public void CanUseCustomContainer()
        {
            var customContainer = new CompositionContainer(new AssemblyCatalog(GetType().Assembly));
            var oldContainer = ServiceLocator.SetContainer(customContainer);
            var svc = ServiceLocator.GetInstance<ICustomService>();

            Assert.IsNotNull(svc);

            ServiceLocator.SetContainer(oldContainer);
        }

        [TestMethod]
        public void CanComposeServerProvider()
        {
            var obj = new ComposableObject();
            obj.Compose();

            Assert.IsNotNull(obj.ConfigurationServerService);
        }

        [TestMethod]
        public void CanComposeCollectionProvider()
        {
            var obj = new ComposableObject();
            obj.Compose();

            Assert.IsNotNull(obj.TeamProjectCollectionService);
        }

        [TestMethod]
        public void CanComposeProjectProvider()
        {
            var obj = new ComposableObject();
            obj.Compose();

            Assert.IsNotNull(obj.ProjectService);
        }

        [TestMethod]
        public void CanComposeProcessTemplateProvider()
        {
            var obj = new ComposableObject();
            obj.Compose();

            Assert.IsNotNull(obj.ProcessTemplateService);
        }

        [TestMethod]
        public void CanComposeNestedComponent()
        {
            var obj = new ComposableObject();
            obj.Compose();

            var provider = obj.ProjectService;
            var pi = provider.GetType().GetProperty("TeamProjectCollectionService", BindingFlags.Instance | BindingFlags.NonPublic);
            var nestedProvider = pi?.GetValue(provider);

            Assert.IsNotNull(nestedProvider);
        }

        #region Support types

        public class ComposableObject
        {
            [Import(typeof(IConfigurationServerService))]
            public IConfigurationServerService PublicContainer { get; set; }

            [Import(typeof(IConfigurationServerService))]
            internal IConfigurationServerService InternalContainer { get; set; }

            [Import(typeof(IConfigurationServerService))]
            protected IConfigurationServerService ProtectedContainer { get; set; }

            [Import(typeof(IConfigurationServerService))]
            private IConfigurationServerService PrivateContainer { get; set; }

            [Import(typeof(IConfigurationServerService))]
            public IConfigurationServerService ConfigurationServerService { get; set; }

            [Import(typeof(ITeamProjectCollectionService))]
            public ITeamProjectCollectionService TeamProjectCollectionService { get; set; }

            [Import(typeof(IProjectService))]
            public IProjectService ProjectService { get; set; }

            [Import(typeof(IProcessTemplateService))]
            public IProcessTemplateService ProcessTemplateService { get; set; }

        }

        public interface ICustomService
        {
            void Foo();
        }

        [Export(typeof(ICustomService))]
        public class CustomService : ICustomService
        {
            public void Foo()
            {
                throw new NotImplementedException();
            }
        }


        #endregion    }
    }
}
