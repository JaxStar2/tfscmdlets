using System.Management.Automation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Models;

namespace TfsCmdlets.Cmdlets.WorkItem
{
    [Cmdlet(VerbsCommon.Get, "WorkItemHistory")]
    [OutputType(typeof(WorkItemHistoryRevision))]
    public class GetWorkItemHistory: WorkItemCmdletBase
    {
        protected override void ProcessRecord()
        {
            var wi = GetWorkItem();

            var latestRev = wi.Revisions.Count - 1;

            foreach (Revision r in wi.Revisions)
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
