using System.Management.Automation;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Cmdlets.Team
{
    [Cmdlet(VerbsCommon.New, "Team", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType(typeof(TeamFoundationTeam))]
    public class NewTeam : ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(Name, "Create team")) return;

            var tp = GetProject();
            var tpc = tp.Store.TeamProjectCollection;
            var teamService = tpc.GetService<TfsTeamService>();
            var team = teamService.CreateTeam(tp.Uri.AbsoluteUri, Name, Description, null);

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
    }
}