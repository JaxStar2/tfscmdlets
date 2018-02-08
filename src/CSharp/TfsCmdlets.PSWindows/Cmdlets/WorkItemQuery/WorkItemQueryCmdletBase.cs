using System.ComponentModel.Composition;
using TfsCmdlets.Services;

namespace TfsCmdlets.Cmdlets.WorkItemQuery
{
    public abstract class WorkItemQueryCmdletBase : ProjectLevelCmdlet
    {
        [Import(typeof(IWorkItemQueryService))]
        protected IWorkItemQueryService WorkItemQueryService { get; set; }
    }
}