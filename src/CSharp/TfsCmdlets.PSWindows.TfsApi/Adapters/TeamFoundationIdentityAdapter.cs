using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Framework.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class TeamFoundationIdentityAdapter: AdapterBase<TeamFoundationIdentity>, ITeamFoundationIdentityAdapter
    {
        public TeamFoundationIdentityAdapter(TeamFoundationIdentity innerInstance) : base(innerInstance)
        {
        }

        public string DisplayName => InnerInstance.DisplayName;
        public string UniqueName => InnerInstance.UniqueName;
        public bool IsActive => InnerInstance.IsActive;
        public bool IsContainer => InnerInstance.IsContainer;
        public object Descriptor => InnerInstance.Descriptor;
    }
}
