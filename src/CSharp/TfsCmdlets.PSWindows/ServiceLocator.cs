using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace TfsCmdlets
{
    public static class ServiceLocator
    {
        private static CompositionContainer _container;

        private static CompositionContainer Container
        {
            get => _container ??
                   (_container = new CompositionContainer(new AssemblyCatalog(typeof(ServiceLocator).Assembly)));
            set => _container = value; }

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
