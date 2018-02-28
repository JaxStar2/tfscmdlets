using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Server;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class TemplateHeaderAdapter: AdapterBase<TemplateHeader>, ITemplateHeaderAdapter
    {
        public TemplateHeaderAdapter(TemplateHeader innerInstance) : base(innerInstance)
        {
        }

        public string Name => InnerInstance.Name;
        public string Description => InnerInstance.Description;
        public int TemplateId => InnerInstance.TemplateId;
        public string Metadata => InnerInstance.Metadata;
    }
}
