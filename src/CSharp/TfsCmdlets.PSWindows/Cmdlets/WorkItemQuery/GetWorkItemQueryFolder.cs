using System.Management.Automation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Cmdlets.WorkItemQuery
{
    [Cmdlet(VerbsCommon.Get, "WorkItemQueryFolder")]
    [OutputType(typeof(QueryFolder2))]
    public class GetWorkItemQueryFolder : WorkItemQueryCmdletBase
    {
        protected override void ProcessRecord()
        {
            WriteObject(WorkItemQueryService.GetItems<QueryFolder2>(Folder, Scope, Project, Collection, Server, Credential), true);
        }

        [Parameter(Position = 0)]
        [ValidateNotNull]
        [SupportsWildcards]
        [Alias("Path")]
        public object Folder { get; set; } = "**";

        [Parameter]
        public WorkItemQueryScope Scope { get; set; } = WorkItemQueryScope.Both;

        [Parameter(ValueFromPipeline = true)]
        public override object Project { get; set; }
    }
}
