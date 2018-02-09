using System.Collections.Generic;
using System.ComponentModel.Composition;
using TfsCmdlets.Services;

namespace TfsCmdlets.Cmdlets.WorkItem
{
    public abstract class WorkItemCmdletBase : ProjectLevelCmdlet
    {
        protected Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem GetWorkItem()
        {
            return WorkItemService.GetWorkItem(WorkItem, null, null, null, null, null, null, Project, Collection,
                Server, Credential);
        }

        protected IEnumerable<Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem> GetWorkItems()
        {
            return WorkItemService.GetWorkItems(WorkItem, null, null, null, null, null, null, Project, Collection,
                Server, Credential);
        }

        protected Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem GetWorkItem(object revision, object asof, string query,
            string filter, string text, Dictionary<string, object> macros)
        {
            return WorkItemService.GetWorkItem(WorkItem, revision, asof, query, filter, text, macros, Project, Collection,
                Server, Credential);
        }

        protected IEnumerable<Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem> GetWorkItems(object revision, object asof, string query,
            string filter, string text, Dictionary<string, object> macros)
        {
            return WorkItemService.GetWorkItems(WorkItem, revision, asof, query, filter, text, macros, Project, Collection,
                Server, Credential);
        }

        public abstract object WorkItem { get; set; }

        [Import(typeof(IWorkItemService))]
        protected IWorkItemService WorkItemService { get; private set; }
    }
}
