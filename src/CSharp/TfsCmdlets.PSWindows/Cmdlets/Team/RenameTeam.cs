using System;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Cmdlets.Team
{
    [Cmdlet(VerbsCommon.Rename, "Team", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType(typeof(TeamFoundationTeam))]
    public class RenameTeam : TeamLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            throw new NotImplementedException();
            //var result = SetTeam(Team, NewName);

            //if (!Passthru) return;

            //WriteObject(result);
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
