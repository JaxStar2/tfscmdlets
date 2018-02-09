using System.Management.Automation;

namespace TfsCmdlets.Cmdlets
{
    public abstract class BaseCmdlet: Cmdlet
    {
        protected override void BeginProcessing()
        {
            this.Compose();
            base.BeginProcessing();
        }
    }
}
