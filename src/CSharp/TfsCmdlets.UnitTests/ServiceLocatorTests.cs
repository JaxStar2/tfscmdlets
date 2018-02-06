using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Microsoft.TeamFoundation.Client;
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

            // ReSharper disable once PossibleNullReferenceException
            Assert.IsNotNull(fi.GetValue(obj));
        }

        [TestMethod]
        public void CanComposePrivateProperty()
        {
            var obj = new ComposableObject();
            obj.Compose();

            var fi = obj.GetType().GetProperty("PrivateContainer", BindingFlags.NonPublic | BindingFlags.Instance);

            // ReSharper disable once PossibleNullReferenceException
            Assert.IsNotNull(fi.GetValue(obj));
        }

        [TestMethod]
        public void CanUseCustomContainer()
        {
            var customContainer = new CompositionContainer(new AssemblyCatalog(GetType().Assembly));
            ServiceLocator.SetContainer(customContainer);

            var svc = ServiceLocator.GetInstance<ICustomService>();

            Assert.IsNotNull(svc);
        }

        #region Support types

        public class ComposableObject
        {
            [Import(typeof(IContainerProvider))]
            public IContainerProvider PublicContainer { get; set; }

            [Import(typeof(IContainerProvider))]
            internal IContainerProvider InternalContainer { get; set; }

            [Import(typeof(IContainerProvider))]
            protected IContainerProvider ProtectedContainer { get; set; }

            [Import(typeof(IContainerProvider))]
            private IContainerProvider PrivateContainer { get; set; }
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
