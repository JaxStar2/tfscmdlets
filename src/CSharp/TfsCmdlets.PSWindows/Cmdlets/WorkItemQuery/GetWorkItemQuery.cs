using System.Management.Automation;
using TfsCmdlets.Core;

namespace TfsCmdlets.Cmdlets.WorkItemQuery
{
    [Cmdlet(VerbsCommon.Get, "WorkItemQuery")]
    [OutputType("Microsoft.TeamFoundation.WorkItemTracking.Client.QueryDefinition2,Microsoft.TeamFoundation.WorkItemTracking.Client")]
    public class GetWorkItemQuery : WorkItemQueryCmdletBase
    {
        protected override void ProcessRecord()
        {
            WriteObject(WorkItemQueryService.GetDefinitions(Query, Scope, Project, Collection, Server, Credential), true);
        }

        [Parameter(Position = 0)]
        [ValidateNotNull]
        [SupportsWildcards]
        [Alias("Path")]
        public object Query { get; set; } = "**";

        [Parameter]
        public WorkItemQueryScope Scope { get; set; } = WorkItemQueryScope.Both;

        [Parameter(ValueFromPipeline = true)]
        public override object Project { get; set; }
    }
}
