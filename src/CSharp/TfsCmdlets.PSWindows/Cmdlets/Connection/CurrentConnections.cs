using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Cmdlets.Connection
{
    internal static class CurrentConnections
    {
        private static TfsConfigurationServer _configurationServer;
        private static TfsTeamProjectCollection _teamProjectCollection;
        private static Project _teamProject;
        private static TeamFoundationTeam _team;

        public static TfsConfigurationServer ConfigurationServer
        {
            get => _configurationServer;

            set
            {
                if (object.Equals(_configurationServer, value)) return;

                DisconnectAll();
                if (value == null) return;

                _configurationServer = value;
            }
        }

        public static TfsTeamProjectCollection TeamProjectCollection
        {
            get => _teamProjectCollection;

            set
            {
                if (object.Equals(_teamProjectCollection, value)) return;

                DisconnectAll();
                if (value == null) return;

                _configurationServer = value.ConfigurationServer;
                _teamProjectCollection = value;
            }
        }

        public static Project TeamProject
        {
            get => _teamProject;

            set
            {
                if (object.Equals(_teamProject, value)) return;

                DisconnectAll();
                if (value == null) return;

                var tpc = value.Store.TeamProjectCollection;
                _configurationServer = tpc.ConfigurationServer;
                _teamProjectCollection = tpc;
                _teamProject = value;
            }
        }

        public static TeamFoundationTeam Team
        {
            get => _team;
            set
            {
                if (object.Equals(_team, value)) return;

                DisconnectAll();
                if (value == null) return;

                _team = value;
            }
        }

        public static void DisconnectAll()
        {
            _configurationServer = null;
            _teamProjectCollection = null;
            _teamProject = null;
            _team = null;
        }

    }
}
