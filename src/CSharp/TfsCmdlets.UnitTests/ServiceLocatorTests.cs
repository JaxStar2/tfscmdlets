using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TfsCmdlets.Providers;

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

            lock (this)
            {
                var oldContainer = ServiceLocator.SetContainer(customContainer);

                var svc = ServiceLocator.GetInstance<ICustomService>();

                Assert.IsNotNull(svc);

                ServiceLocator.SetContainer(oldContainer);
            }
        }

        [TestMethod]
        public void CanComposeServerProvider()
        {
            var obj = new ComposableObject();
            obj.Compose();

            Assert.IsNotNull(obj.ServerProvider);
        }

        [TestMethod]
        public void CanComposeCollectionProvider()
        {
            var obj = new ComposableObject();
            obj.Compose();

            Assert.IsNotNull(obj.CollectionProvider);
        }

        [TestMethod]
        public void CanComposeProjectProvider()
        {
            var obj = new ComposableObject();
            obj.Compose();

            Assert.IsNotNull(obj.ProjectProvider);
        }

        [TestMethod]
        public void CanComposeProcessTemplateProvider()
        {
            var obj = new ComposableObject();
            obj.Compose();

            Assert.IsNotNull(obj.ProcessTemplateProvider);
        }

        [TestMethod]
        public void CanComposeNestedComponent()
        {
            var obj = new ComposableObject();
            obj.Compose();

            var provider = obj.ProjectProvider;
            var pi = provider.GetType().GetProperty("CollectionProvider", BindingFlags.Instance | BindingFlags.NonPublic);
            var nestedProvider = pi?.GetValue(provider);

            Assert.IsNotNull(nestedProvider);
        }

        #region Support types

        public class ComposableObject
        {
            [Import(typeof(IServerProvider))]
            public IServerProvider PublicContainer { get; set; }

            [Import(typeof(IServerProvider))]
            internal IServerProvider InternalContainer { get; set; }

            [Import(typeof(IServerProvider))]
            protected IServerProvider ProtectedContainer { get; set; }

            [Import(typeof(IServerProvider))]
            private IServerProvider PrivateContainer { get; set; }

            [Import(typeof(IServerProvider))]
            public IServerProvider ServerProvider { get; set; }

            [Import(typeof(ICollectionProvider))]
            public ICollectionProvider CollectionProvider { get; set; }

            [Import(typeof(IProjectProvider))]
            public IProjectProvider ProjectProvider { get; set; }

            [Import(typeof(IProcessTemplateProvider))]
            public IProcessTemplateProvider ProcessTemplateProvider { get; set; }

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
