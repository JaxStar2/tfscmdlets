using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Server;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class NodeInfoAdapter: AdapterBase<NodeInfo>, INodeInfoAdapter
    {
        public NodeInfoAdapter(NodeInfo innerInstance) 
            : base(innerInstance)
        {
        }

        public string Uri => InnerInstance.Uri;
        public string Path => InnerInstance.Path;
    }
}
