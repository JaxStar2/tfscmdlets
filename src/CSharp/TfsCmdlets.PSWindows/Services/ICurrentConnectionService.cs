using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Services
{
    public interface ICurrentConnectionService
    {
        TfsConfigurationServer ConfigurationServer { get; set; }
        TfsTeamProjectCollection TeamProjectCollection { get; set; }
        Project TeamProject { get; set; }
        TeamFoundationTeam Team { get; set; }
        void DisconnectAll();
    }
}
