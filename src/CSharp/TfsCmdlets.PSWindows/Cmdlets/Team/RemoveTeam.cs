using System.Management.Automation;
using Microsoft.TeamFoundation.Framework.Client;

namespace TfsCmdlets.Cmdlets.Team
{
    [Cmdlet(VerbsCommon.Remove, "Team", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    public class RemoveTeam : TeamLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var teams = GetTeams(Team, false, Project, Collection, Server, Credential);
            var tp = GetProject();
            var tpc = tp.Store.TeamProjectCollection;
            var identityService = tpc.GetService<IIdentityManagementService>();

            foreach (var team in teams)
            {
                if (!ShouldProcess(team.Name, "Delete team")) continue;

                identityService.DeleteApplicationGroup(team.Identity.Descriptor);
            }
        }

        [Parameter(Position = 0, ValueFromPipeline = true)]
        public override object Team { get; set; }

        [Parameter]
        public override object Project { get; set; }
    }
}