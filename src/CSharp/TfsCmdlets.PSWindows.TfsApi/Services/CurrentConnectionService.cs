using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(ICurrentConnectionService))]
    public class CurrentConnectionService: ICurrentConnectionService
    {
        private ITfsConfigurationServerAdapter _configurationServer;
        private ITfsTeamProjectCollectionAdapter _teamProjectCollection;
        private IProjectAdapter _teamProject;
        private ITeamFoundationTeamAdapter _team;

        public ITfsConfigurationServerAdapter ConfigurationServer
        {
            get => _configurationServer;

            set
            {
                if (Equals(_configurationServer, value)) return;

                DisconnectAll();
                if (value == null) return;

                _configurationServer = value;
            }
        }

        public ITfsTeamProjectCollectionAdapter TeamProjectCollection
        {
            get => _teamProjectCollection;

            set
            {
                if (Equals(_teamProjectCollection, value)) return;

                DisconnectAll();
                if (value == null) return;

                var tpc = (TfsTeamProjectCollection)value.Instance;
                _configurationServer = new TfsConfigurationServerAdapter(tpc.ConfigurationServer);
                _teamProjectCollection = value;
            }
        }

        public IProjectAdapter TeamProject
        {
            get => _teamProject;

            set
            {
                if (Equals(_teamProject, value)) return;

                DisconnectAll();
                if (value == null) return;

                var tp = ((Project) value.Instance);
                var tpc = tp.Store.TeamProjectCollection;
                _configurationServer = new TfsConfigurationServerAdapter(tpc.ConfigurationServer);
                _teamProjectCollection = new TfsTeamProjectCollectionAdapter(tpc);
                _teamProject = value;
            }
        }

        public ITeamFoundationTeamAdapter Team
        {
            get => _team;
            set
            {
                if (Equals(_team, value)) return;

                DisconnectAll();
                if (value == null) return;

                _team = value;
            }
        }

        public void DisconnectAll()
        {
            _configurationServer = null;
            _teamProjectCollection = null;
            _teamProject = null;
            _team = null;
        }

    }
}
