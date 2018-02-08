using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TfsCmdlets.UnitTests
{
    public class TestSetup
    {
        [AssemblyInitialize]
        public void Initialize()
        {
            var container = new CompositionContainer(new AssemblyCatalog(GetType().Assembly));
            ServiceLocator.SetContainer(container);
        }

        [AssemblyCleanup]
        public void Cleanup()
        {
            
        }
    }
}
