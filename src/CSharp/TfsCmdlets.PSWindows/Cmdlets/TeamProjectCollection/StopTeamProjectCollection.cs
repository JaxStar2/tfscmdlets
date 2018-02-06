using System;
using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsLifecycle.Stop, "TeamProjectCollection", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    public class StopTeamProjectCollection : CollectionLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            throw new NotImplementedException();
        }

        [Parameter]
        public string Reason { get; set; }

        [Parameter]
        public override object Collection { get; set; }
    }
}