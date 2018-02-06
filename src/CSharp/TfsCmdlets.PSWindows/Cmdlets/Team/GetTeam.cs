using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.Team
{
    [Cmdlet(VerbsCommon.Get, "Team", DefaultParameterSetName = "Get by name")]
    [OutputType(typeof(Microsoft.TeamFoundation.Client.TeamFoundationTeam))]
    public class GetTeam : TeamLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(GetTeams(Team, Default, Project, Collection, Server, Credential), true);
        }

        [Parameter(Position = 0, ParameterSetName = "Get by name")]
        [Alias("Name")]
        [SupportsWildcards]
        public override object Team { get; set; } = "*";

        [Parameter(Position = 0, ParameterSetName = "Get default team", Mandatory = true)]
        public SwitchParameter Default { get; set; }

        public override object Project { get; set; }
    }
}