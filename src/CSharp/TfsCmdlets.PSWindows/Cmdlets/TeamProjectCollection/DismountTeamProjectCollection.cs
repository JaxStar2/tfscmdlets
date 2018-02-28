using System;
using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsData.Dismount, "TeamProjectCollection", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [OutputType(typeof(string))]
    public class DismountTeamProjectCollection : CollectionLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var tpc = GetCollection();

            if (!ShouldProcess(tpc.Name, "Detach team project collection")) return;

            WriteObject(CollectionService.DetachCollection(Collection, Reason, Timeout, Server, Credential));

        }

        [Parameter(Mandatory = true, Position = 0)]
        public override object Collection { get; set; }

        [Parameter]
        public string Reason { get; set; }

        [Parameter]
        public TimeSpan Timeout { get; set; } = TimeSpan.MaxValue;
    }
}