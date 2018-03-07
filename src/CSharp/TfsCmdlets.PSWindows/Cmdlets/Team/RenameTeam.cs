using System;
using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.Team
{
    [Cmdlet(VerbsCommon.Rename, "Team", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Client.TeamFoundationTeam,Microsoft.TeamFoundation.Client")]
    public class RenameTeam : TeamLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var teams = GetTeams(Team);

            foreach (var team in teams)
            {
                if (!ShouldProcess(team.Name, $"Rename team to {NewName}")) continue;

                TeamService.RenameTeam(team, NewName, Project, Collection, Server, Credential);
            }
        }

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [Alias("Name")]
        [SupportsWildcards]
        public override object Team { get; set; } = "*";

        [Parameter(Mandatory = true, Position = 1)]
        public string NewName { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Parameter]
        public override object Project { get; set; }
    }
}
