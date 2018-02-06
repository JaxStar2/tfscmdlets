using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Providers;

namespace TfsCmdlets.Cmdlets
{
    public class BaseCmdlet: Cmdlet
    {
        protected override void BeginProcessing()
        {
            this.Compose();
            base.BeginProcessing();
        }

        [Import(typeof(IContainerProvider))]
        protected IContainerProvider Provider { get; set; }

    }
}
