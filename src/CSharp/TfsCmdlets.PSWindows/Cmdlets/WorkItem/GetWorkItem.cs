using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.WorkItem
{
    [Cmdlet(VerbsCommon.Get, "WorkItem")]
    public class GetWorkItem : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("Hello world!");
        }
    }
}
