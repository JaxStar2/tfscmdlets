using System.Management.Automation;
using TfsCmdlets.Core;

namespace TfsCmdlets.Cmdlets.WorkItemQuery
{
    [Cmdlet(VerbsCommon.Get, "WorkItemQueryFolder")]
    [OutputType("Microsoft.TeamFoundation.WorkItemTracking.Client.QueryFolder2")]
    public class GetWorkItemQueryFolder : WorkItemQueryCmdletBase
    {
        protected override void ProcessRecord()
        {
            WriteObject(WorkItemQueryService.GetFolders(Folder, Scope, Project, Collection, Server, Credential), true);
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
