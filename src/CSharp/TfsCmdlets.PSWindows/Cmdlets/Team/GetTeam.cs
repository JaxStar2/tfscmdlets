using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.Team
{
    [Cmdlet(VerbsCommon.Get, "Team", DefaultParameterSetName = "Get by name")]
    [OutputType("Microsoft.TeamFoundation.Client.TeamFoundationTeam,Microsoft.TeamFoundation.Client")]
    public class GetTeam : TeamLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (Default)
            {
                WriteObject(GetDefaultTeam());
                return;
            }

            WriteObject(GetTeams(Team), true);
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