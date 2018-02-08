using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Services.WebApi;
using System.Management.Automation;
using TfsCmdlets.Services;

namespace TfsCmdlets.Cmdlets.Connection
{
    [Cmdlet(VerbsCommunications.Connect, "TeamProjectCollection", DefaultParameterSetName = "Explicit credentials")]
    [OutputType(typeof(VssConnection))]
    public class ConnectTeamProjectCollection : CollectionLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (Interactive.IsPresent)
            {
                Credential = GetCredential.Get(true);
            }

            var tpc = GetCollection();
            tpc.EnsureAuthenticated();

            CurrentConnectionService.TeamProjectCollection = tpc;

            if (Passthru)
            {
                WriteObject(tpc);
            }
        }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateNotNull]
        public override object Collection { get; set; }

        [Parameter]
        public override object Server { get; set; }

        [Parameter(ParameterSetName = "Explicit credentials")]
        public override object Credential { get; set; }

        [Parameter(ParameterSetName = "Prompt for credentials", Mandatory = true)]
        public SwitchParameter Interactive { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }
    }
}
