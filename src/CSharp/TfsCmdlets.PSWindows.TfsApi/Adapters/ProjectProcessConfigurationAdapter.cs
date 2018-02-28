using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.ProcessConfiguration.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class ProjectProcessConfigurationAdapter: AdapterBase<ProjectProcessConfiguration>, IProjectProcessConfigurationAdapter
    {
        public ProjectProcessConfigurationAdapter(ProjectProcessConfiguration innerInstance) : base(innerInstance)
        {
        }
    }
}
