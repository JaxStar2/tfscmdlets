using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.VersionControl.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class WorkspaceAdapter: AdapterBase<Workspace>, IWorkspaceAdapter
    {
        public WorkspaceAdapter(Workspace innerInstance) : base(innerInstance)
        {
        }

        public string Name
        {
            get => InnerInstance.Name;
        }
    }
}
