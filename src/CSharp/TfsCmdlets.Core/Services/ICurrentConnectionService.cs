using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface ICurrentConnectionService
    {
        ITfsConfigurationServerAdapter ConfigurationServer { get; set; }
        ITfsTeamProjectCollectionAdapter TeamProjectCollection { get; set; }
        IProjectAdapter TeamProject { get; set; }
        ITeamFoundationTeamAdapter Team { get; set; }
        void DisconnectAll();
    }
}
