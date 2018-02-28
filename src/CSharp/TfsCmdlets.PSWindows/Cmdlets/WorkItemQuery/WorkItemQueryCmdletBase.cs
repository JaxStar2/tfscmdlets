using System.ComponentModel.Composition;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.WorkItemQuery
{
    public abstract class WorkItemQueryCmdletBase : ProjectLevelCmdlet
    {
        [Import(typeof(IWorkItemQueryService))]
        protected IWorkItemQueryService WorkItemQueryService { get; set; }
    }
}