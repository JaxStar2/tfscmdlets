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
    public class TeamService: ITeamService
    {
        public ITeamFoundationTeamAdapter GetTeam(object team, object project, object collection, object server, object credential)
        {
            var lists = GetTeams(team, project, collection, server, credential).ToList();

            if (lists.Count == 0)
                throw new Exception($"Invalid team '{team}'");

            if (lists.Count == 1)
                return lists[0];

            var names = string.Join(", ", lists.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{team}' matches {lists.Count} teams: {names}. " +
                                "Please choose a more specific value for the -Team argument and try again");
        }

        public IEnumerable<ITeamFoundationTeamAdapter> GetTeams(object team, object project, object collection, object server, object credential)
        {
            var teamSvc = GetTeamService(project, collection, server, credential, out var projectId);

            switch (team)
            {
                case ITeamFoundationTeamAdapter t:
                {
                    yield return t;

                    break;
                }
                case string s:
                {
                    foreach (var t1 in teamSvc.QueryTeams(projectId).Where(t => t.Name.IsLike(s)))
                    {
                        yield return new TeamFoundationTeamAdapter(t1);
                    }

                    break;
                }
                default:
                {
                    throw new ArgumentException($"Invalid team name {team}");
                }
            }
        }

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
            var t = (TeamFoundationTeam) GetTeam(team, project, collection, server, credential).Instance;

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
            var tp = (Project) ProjectService.GetProject(project, collection, server, credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var teamSvc = tpc.GetService<TfsTeamService>();
            projectId = tp.Uri.AbsoluteUri;
            return teamSvc;
        }

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }

        [Import(typeof(IIdentityManagementService))]
        private IIdentityManagementService IdentityManagementService { get; set; }
    }
}
