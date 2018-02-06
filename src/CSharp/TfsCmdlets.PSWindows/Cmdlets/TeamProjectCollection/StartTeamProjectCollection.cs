using System;
using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsLifecycle.Start, "TeamProjectCollection", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    public class StartTeamProjectCollection : ServerLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            throw new NotImplementedException();
        }

        [Parameter]
        [Alias("Name")]
        public string Collection { get; set; }

        [Parameter]
        public override object Server { get; set; }

        [Parameter]
        public override object Credential { get; set; }
    }
}