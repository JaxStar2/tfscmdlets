using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class TeamFoundationTeamAdapter: AdapterBase<TeamFoundationTeam>, ITeamFoundationTeamAdapter
    {
        public TeamFoundationTeamAdapter(TeamFoundationTeam innerInstance) : base(innerInstance)
        {
        }

        public string Name => InnerInstance.Name;
        public string Description => InnerInstance.Description;
        public ITeamFoundationIdentityAdapter IdentityDescriptor => new TeamFoundationIdentityAdapter(InnerInstance.Identity);
    }
}
