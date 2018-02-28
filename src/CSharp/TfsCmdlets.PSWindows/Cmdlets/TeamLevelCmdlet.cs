using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets
{
    public abstract class TeamLevelCmdlet : ProjectLevelCmdlet
    {
        public abstract object Team { get; set; }

        protected ITeamFoundationTeamAdapter GetTeam()
        {
            return TeamService.GetTeam(Team, Project, Collection, Server, Credential);
        }

        protected ITeamFoundationTeamAdapter GetDefaultTeam()
        {
            return TeamService.GetDefaultTeam(Project, Collection, Server, Credential);
        }

        protected IEnumerable<ITeamFoundationTeamAdapter> GetTeams(object team)
        {
            return TeamService.GetTeams(team, Project, Collection, Server, Credential);
        }

        [Import(typeof(ITeamService))]
        protected ITeamService TeamService { get; set; }

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