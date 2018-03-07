using System.Management.Automation;
using TfsCmdlets.Cmdlets.WorkItem;

namespace TfsCmdlets.Cmdlets.WorkItemLink
{
    [Cmdlet(VerbsCommon.Get, "WorkItemLink")]
    [OutputType("Microsoft.TeamFoundation.WorkItemTracking.Client.Link,Microsoft.TeamFoundation.WorkItemTracking.Client")]
    public class GetWorkItemLink : WorkItemCmdletBase
    {
        protected override void ProcessRecord()
        {
            var wi = GetWorkItem();

            WriteObject(wi.Links, true);
        }

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [Alias("id")]
        [ValidateNotNull()]
        public override object WorkItem { get; set; }

        // Hidden
        public override object Project { get; set; }
    }
}
