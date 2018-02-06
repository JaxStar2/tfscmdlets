using Microsoft.VisualStudio.Services.WebApi;
using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.Connection
{
    [Cmdlet(VerbsCommunications.Connect, "TeamProjectCollection", DefaultParameterSetName = "Explicit credentials")]
    [OutputType(typeof(VssConnection))]
    public class ConnectTeamProjectCollection : CollectionLevelCmdlet
    {
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

        protected override void ProcessRecord()
        {
            if (Interactive.IsPresent)
            {
                Credential = GetCredential.Get(true);
            }

            var tpc = GetCollection();
            tpc.EnsureAuthenticated();

            CurrentConnections.TeamProjectCollection = tpc;

            if (Passthru)
            {
                WriteObject(tpc);
            }
        }
    }
}
