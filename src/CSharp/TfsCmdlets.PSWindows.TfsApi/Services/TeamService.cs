using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(ITeamService))]
    public class TeamService : ServiceBase<TeamFoundationTeam, TeamFoundationTeamAdapter>, ITeamService
    {
        public ITeamFoundationTeamAdapter GetDefaultTeam(object project, object collection, object server, object credential)
        {
            var teamSvc = GetTeamService(project, collection, server, credential, out var projectId);

            return new TeamFoundationTeamAdapter(teamSvc.GetDefaultTeam(projectId, null));
        }

        public ITeamFoundationTeamAdapter CreateTeam(string name, string description, object project, object collection, object server,
            object credential)
        {
            var teamSvc = GetTeamService(project, collection, server, credential, out var projectId);

            return new TeamFoundationTeamAdapter(teamSvc.CreateTeam(projectId, name, description, null));
        }

        public void DeleteTeam(object team, object project, object collection, object server, object credential)
        {
            var id = GetTeam(team, project, collection, server, credential).IdentityDescriptor.Descriptor;

            IdentityManagementService.DeleteGroup(id, collection, server, credential);
        }

        public ITeamFoundationTeamAdapter RenameTeam(object team, string newName, object project, object collection, object server,
            object credential)
        {
            var t = GetTeam(team, project, collection, server, credential);

            return SetTeam(team, newName, t.Description, project, collection, server, credential);
        }

        public ITeamFoundationTeamAdapter SetTeam(object team, string newName, string description, object project, object collection,
            object server, object credential)
        {
            var t = (TeamFoundationTeam)GetTeam(team, project, collection, server, credential).Instance;

            var teamSvc = GetTeamService(project, collection, server, credential, out var _);

            t.Name = newName;
            t.Description = description;
            teamSvc.UpdateTeam(t);

            return new TeamFoundationTeamAdapter(t);
        }

        public void SetDefaultTeam(object team, object project, object collection, object server, object credential)
        {

            var t = (TeamFoundationTeam)GetTeam(team, project, collection, server, credential).Instance;
            var teamSvc = GetTeamService(project, collection, server, credential, out var _);

            teamSvc.SetDefaultTeam(t);
        }

        private TfsTeamService GetTeamService(object project, object collection, object server, object credential,
            out string projectId)
        {
            var tp = (Project)ProjectService.GetProject(project, collection, server, credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var teamSvc = tpc.GetService<TfsTeamService>();
            projectId = tp.Uri.AbsoluteUri;
            return teamSvc;
        }

        #region Get Items

        protected override string ItemName => "team";
        protected override Func<TeamFoundationTeam, string> ItemDescriptor => (t => t.Name);

        public ITeamFoundationTeamAdapter GetTeam(object team, object project, object collection, object server, object credential)
            => new TeamFoundationTeamAdapter(GetItem(team, project, collection, server, credential));

        public IEnumerable<ITeamFoundationTeamAdapter> GetTeams(object team, object project, object collection, object server, object credential)
            => GetItems(team, project, collection, server, credential).Select(t => new TeamFoundationTeamAdapter(t));

        protected override IEnumerable<TeamFoundationTeam> GetAllItems(object item, ScopeObjects so)
        {
            var teamSvc = GetTeamService(so.Project, so.Collection, so.Server, so.Credential, out var projectId);
            return teamSvc.QueryTeams(projectId);
        }

        #endregion

        #region Imports

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }

        [Import(typeof(IIdentityManagementService))]
        private IIdentityManagementService IdentityManagementService { get; set; }

        #endregion
    }
}
