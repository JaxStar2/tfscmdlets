using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.Team
{
    [Cmdlet(VerbsCommon.New, "Team", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Client.TeamFoundationTeam,Microsoft.TeamFoundation.Client")]
    public class NewTeam : TeamLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(Name, "Create team")) return;

            var team = TeamService.CreateTeam(Name, Description, Project, Collection, Server, Credential);

            if (Passthru)
                WriteObject(team);
        }

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [Alias("Team")]
        public string Name { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Parameter]
        public override object Project { get; set; }

        // Hidden
        public override object Team { get; set; }
    }
}