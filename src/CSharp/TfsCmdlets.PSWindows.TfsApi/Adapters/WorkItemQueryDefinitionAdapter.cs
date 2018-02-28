using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class WorkItemQueryDefinitionAdapter: AdapterBase<QueryDefinition2>, IWorkItemQueryDefinitionAdapter
    {
        public WorkItemQueryDefinitionAdapter(QueryDefinition2 innerInstance) : base(innerInstance)
        {
        }

        public string Name => InnerInstance.Name;
        public Guid Id => InnerInstance.Id;
        public string Path => InnerInstance.Path;
        public string QueryText => InnerInstance.QueryText;
    }
}
