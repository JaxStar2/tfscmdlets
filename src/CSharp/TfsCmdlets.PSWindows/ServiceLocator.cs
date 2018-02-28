using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace TfsCmdlets
{
    public static class ServiceLocator
    {
        private static CompositionContainer _container;

        static ServiceLocator()
        {
            AssemblyResolver.Register();
        }

        private static CompositionContainer Container
        {
            get => _container ?? (_container = CreateContainer());
            set => _container = value;
        }

        private static CompositionContainer CreateContainer()
        {
            var baseDir = Path.GetDirectoryName(typeof(ServiceLocator).Assembly.Location);

            if (baseDir == null)
                throw new InvalidOperationException();

            return new CompositionContainer(new DirectoryCatalog(baseDir, "TfsCmdlets.*.dll"));
        }

        public static CompositionContainer SetContainer(CompositionContainer container)
        {
            var oldContainer = Container;

            Container = container;

            return oldContainer;
        }

        public static T GetInstance<T>()
        {
            return Container.GetExportedValue<T>();
        }

        public static void Compose(this object composableObject)
        {
            Container.ComposeParts(composableObject);
        }
    }
}
