using System;
using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.Team
{
    [Cmdlet(VerbsCommon.Set, "Team", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Client.TeamFoundationTeam")]
    public class SetTeam : TeamLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var teams = GetTeams(Team);

            foreach (var team in teams)
            {
                if (Default && ShouldProcess(team.Name, "Set team as the default team"))
                {
                    TeamService.SetDefaultTeam(team, Project, Collection, Server, Credential);
                }
                else if (ShouldProcess(team.Name, "Set team properties"))
                {
                    TeamService.SetTeam(team, NewName, Description ?? team.Description, Project, Collection, Server, Credential);
                }
                else
                {
                    continue;
                }

                if (Passthru)
                    WriteObject(team);
            }
        }

        [Parameter(ParameterSetName = "Set properties", Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [Alias("Name")]
        [SupportsWildcards]
        public override object Team { get; set; } = "*";

        [Parameter(ParameterSetName = "Set properties", Mandatory = true, Position = 1)]
        public string NewName { get; set; }

        [Parameter(ParameterSetName = "Set properties")]
        public string Description { get; set; }

        [Parameter(ParameterSetName = "Set as default", Mandatory = true)]
        public SwitchParameter Default { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Parameter]
        public override object Project { get; set; }
    }
}
