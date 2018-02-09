using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Services
{
    public interface IWorkItemService
    {
        WorkItem GetWorkItem(object workItem, object revision, object asof, string query,
            string filter, string text, Dictionary<string, object> macros, object project, object collection,
            object server, object credential);

        IEnumerable<WorkItem> GetWorkItems(object workItem, object revision, object asof, string query, 
            string filter, string text, Dictionary<string, object> macros, object project, object collection, 
            object server, object credential);
    }
}
