using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Cmdlets
{
    public abstract class TeamLevelCmdlet : ProjectLevelCmdlet
    {
        public abstract object Team { get; set; }

        protected TeamFoundationTeam GetTeam(bool defaultTeam = false)
        {
            return GetTeam(Team, defaultTeam, Project, Collection, Server, Credential);
        }

        protected TeamFoundationTeam GetTeam(object team, bool defaultTeam, object project, object collection, object server, object credential)
        {
            var teams = GetTeams(team, defaultTeam, project, collection, server, credential).ToList();

            if (teams.Count == 0)
                throw new PSArgumentException($"Invalid team name '{Team}'", nameof(Team));

            if (teams.Count == 1)
                return teams[0];

            var names = string.Join(", ", teams.Select(o => o.Name).ToArray());
            throw new PSArgumentException($"Ambiguous name '{Team}' matches {teams.Count} teams: {names}. Please choose a more specific value for the {nameof(Team)} argument and try again", nameof(Team));
        }

        protected IEnumerable<TeamFoundationTeam> GetTeams(object team, bool defaultTeam, object project, object collection, object server, object credential)
        {
            var tp = GetProject(project, collection, server, credential);
            var tpc = tp.Store.TeamProjectCollection;
            var teamService = tpc.GetService<TfsTeamService>();

            switch (team)
            {
                case TeamFoundationTeam t:
                {
                    yield return t;

                    break;
                }
                case string s:
                {
                    foreach (var t1 in teamService.QueryTeams(tp.Uri.AbsoluteUri).Where(t => t.Name.IsLike(s)))
                    {
                        yield return t1;
                    }

                    break;
                }
                case null when defaultTeam:
                {
                    yield return teamService.GetDefaultTeam(tp.Uri.AbsoluteUri, null);

                    break;
                }
            }
        }
        //protected TeamFoundationTeam SetTeam(object team, string newName = null, string description = null, bool? defaultTeam)
        //{
        //    var t = GetTeam(team, false);
        //    var tp = GetProject();
        //    var tpc = tp.Store.TeamProjectCollection;
        //    var teamService = tpc.GetService<Microsoft.TeamFoundation.Client.TfsTeamService>();

        //    bool isDirty = false;

        //    if (!string.IsNullOrWhiteSpace(newName) && ShouldProcess(t.Name, "Rename team to 'NewName'"))
        //    {
        //        isDirty = true;
        //        t.Name = newName;
        //    }

        //    if ( - and PSCmdlet.ShouldProcess(Team, "Set team's description to 'Description'"))
        //    {
        //        isDirty = true
        //            t.Description = Description
        //    }

        //    if (Default - and PSCmdlet.ShouldProcess(Team, "Set team to project's default team"))
        //    {
        //        teamService.SetDefaultTeam(t)
        //    }

        //    if (isDirty)
        //    {
        //        teamService.UpdateTeam(t)
        //    }

        //    return t
        //}
    }
}