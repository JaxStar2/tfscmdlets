using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Build.WebApi;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class BuildDefinitionAdapter: AdapterBase<BuildDefinition>, IBuildDefinitionAdapter
    {
        public BuildDefinitionAdapter(BuildDefinition innerInstance) : base(innerInstance)
        {
        }

        public string Name => InnerInstance.Name;
    }
}
