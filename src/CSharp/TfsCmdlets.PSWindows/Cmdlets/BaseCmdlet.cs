using System.Management.Automation;

namespace TfsCmdlets.Cmdlets
{
    public class BaseCmdlet: Cmdlet
    {
        protected override void BeginProcessing()
        {
            this.Compose();
            base.BeginProcessing();
        }
    }
}
