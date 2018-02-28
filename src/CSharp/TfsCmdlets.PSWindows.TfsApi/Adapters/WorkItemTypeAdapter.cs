using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class WorkItemTypeAdapter: AdapterBase<WorkItemType>, IWorkItemTypeAdapter
    {
        public WorkItemTypeAdapter(WorkItemType innerInstance) : base(innerInstance)
        {
        }

        public IProjectAdapter Project => new ProjectAdapter(InnerInstance.Project);
        public string Name => InnerInstance.Name;
    }
}
