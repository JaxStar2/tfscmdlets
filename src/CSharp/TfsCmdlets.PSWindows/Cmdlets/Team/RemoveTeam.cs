using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.Team
{
    [Cmdlet(VerbsCommon.Remove, "Team", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    public class RemoveTeam : TeamLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var teams = GetTeams(Team);

            foreach (var team in teams)
            {
                if (!ShouldProcess(team.Name, "Delete team")) continue;

                IdentityManagementService.DeleteGroup(team.IdentityDescriptor.Descriptor, Collection, Server, Credential);
            }
        }

        [Parameter(Position = 0, ValueFromPipeline = true)]
        public override object Team { get; set; }

        [Parameter]
        public override object Project { get; set; }

        [Import(typeof(IIdentityManagementService))]
        private IIdentityManagementService IdentityManagementService { get; set; }
    }
}