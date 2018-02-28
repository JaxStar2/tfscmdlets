using System.Management.Automation;
using TfsCmdlets.Core.Models;

namespace TfsCmdlets.Cmdlets.WorkItem
{
    [Cmdlet(VerbsCommon.Get, "WorkItemHistory")]
    [OutputType(typeof(WorkItemHistoryRevision))]
    public class GetWorkItemHistory: WorkItemCmdletBase
    {
        protected override void ProcessRecord()
        {
            var wi = GetWorkItem();

            foreach (var r in wi.Revisions)
            {
                WriteObject(new WorkItemHistoryRevision(r));
            }
        }

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [Alias("id")]
        [ValidateNotNull()]
        public override object WorkItem { get; set; }

        [Parameter]
        public override object Project { get; set; }
    }
}
