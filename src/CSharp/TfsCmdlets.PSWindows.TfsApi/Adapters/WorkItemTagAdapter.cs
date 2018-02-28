using Microsoft.TeamFoundation.Core.WebApi;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class WorkItemTagAdapter: AdapterBase<WebApiTagDefinition>, IWorkItemTagAdapter
    {
        public WorkItemTagAdapter(WebApiTagDefinition innerInstance) : base(innerInstance)
        {
        }

        public string Name => InnerInstance.Name;
    }
}
