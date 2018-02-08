using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Services;

namespace TfsCmdlets.UnitTests.Stubs.Services
{
    [Export(typeof(ICurrentConnectionService))]
    public class CurrentConnectionServiceImpl: ICurrentConnectionService
    {
        private TfsConfigurationServer _configurationServer;
        private TfsTeamProjectCollection _teamProjectCollection;
        private Project _teamProject;
        private TeamFoundationTeam _team;

        public TfsConfigurationServer ConfigurationServer
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

        public TfsTeamProjectCollection TeamProjectCollection
        {
            get => _teamProjectCollection;

            set
            {
                if (Equals(_teamProjectCollection, value)) return;

                DisconnectAll();
                if (value == null) return;

                _configurationServer = value.ConfigurationServer;
                _teamProjectCollection = value;
            }
        }

        public Project TeamProject
        {
            get => _teamProject;

            set
            {
                if (Equals(_teamProject, value)) return;

                DisconnectAll();
                if (value == null) return;

                var tpc = value.Store.TeamProjectCollection;
                _configurationServer = tpc.ConfigurationServer;
                _teamProjectCollection = tpc;
                _teamProject = value;
            }
        }

        public TeamFoundationTeam Team
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
