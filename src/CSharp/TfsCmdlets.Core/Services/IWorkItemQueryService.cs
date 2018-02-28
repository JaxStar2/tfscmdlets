using System.Collections.Generic;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IWorkItemQueryService
    {
        IWorkItemQueryDefinitionAdapter GetDefinition(object item, WorkItemQueryScope scope, object project,
            object collection, object server, object credential);

        IEnumerable<IWorkItemQueryDefinitionAdapter> GetDefinitions(object item, WorkItemQueryScope scope,
            object project, object collection, object server, object credential);

        IWorkItemQueryFolderAdapter GetFolder(object item, WorkItemQueryScope scope, object project,
            object collection, object server, object credential);

        IEnumerable<IWorkItemQueryFolderAdapter> GetFolders(object item, WorkItemQueryScope scope,
            object project, object collection, object server, object credential);
    }
}
