using System.Collections.Generic;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IWorkItemService
    {
        IWorkItemAdapter GetWorkItem(object workItem, object revision, object asof, string query,
            string filter, string text, Dictionary<string, object> macros, object project, object collection,
            object server, object credential);

        IEnumerable<IWorkItemAdapter> GetWorkItems(object workItem, object revision, object asof, string query, 
            string filter, string text, Dictionary<string, object> macros, object project, object collection, 
            object server, object credential);

        IWorkItemAdapter GetWorkItem(int id, bool bypassRules);
        IWorkItemAdapter NewWorkItem(IWorkItemTypeAdapter wit);
        IWorkItemAdapter NewWorkItem(IWorkItemTypeAdapter wit, bool bypassRules);
        void DestroyWorkItems(IEnumerable<int> ids);
        void Save(IWorkItemAdapter wi);
        IEnumerable<IWorkItemFieldAdapter> Validate(IWorkItemAdapter wi);
        IWorkItemAdapter Copy(IWorkItemAdapter wi, IWorkItemTypeAdapter targetType, WorkItemCopyFlags flags);

    }
}
