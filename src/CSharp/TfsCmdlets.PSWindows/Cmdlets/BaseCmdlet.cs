using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Cmdlets
{
    public abstract class BaseCmdlet: Cmdlet
    {
        protected override void BeginProcessing()
        {
            this.Compose();
            base.BeginProcessing();
        }

        protected void WriteObject(IAdapter adapter)
        {
            WriteObject(adapter.Instance);
        }

        protected void WriteObject(IEnumerable<IAdapter> adapters, bool enumerateCollection)
        {
            WriteObject(adapters.Select(a => a.Instance), enumerateCollection);
        }
    }
}
