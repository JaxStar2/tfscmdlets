using System;
using System.Collections.Generic;
using System.Management.Automation;
using TfsCmdlets.PSWindows.TfsApi.Services;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsData.Mount, "TeamProjectCollection", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    public class MountTeamProjectCollection : CollectionLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!(Collection is string name))
            {
                throw new Exception($"Invalid team project collection name {Collection}");
            }

            if (!ShouldProcess(name, "Attach team project collection")) return;

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                ConnectionString = $"Data source={DatabaseServer}; Integrated Security=true; " +
                                   "Initial Catalog={DatabaseName}";
            }

            WriteObject(CollectionService.AttachCollection(name, Description, ConnectionString, Clone, Timeout, Server, Credential));
        }

        [Parameter(Mandatory = true, Position = 0)]
        [Alias("Name")]
        public override object Collection { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter(ParameterSetName = "Use database server", Mandatory = true)]
        public string DatabaseServer { get; set; }

        [Parameter(ParameterSetName = "Use database server", Mandatory = true)]
        public string DatabaseName { get; set; }

        [Parameter(ParameterSetName = "Use connection string", Mandatory = true)]
        public string ConnectionString { get; set; }

        [Parameter]
        [ValidateSet("Started", "Stopped")]
        public string InitialState { get; set; } = "Started";

        [Parameter]
        public SwitchParameter Clone { get; set; }

        [Parameter]
        public int PollingInterval { get; set; } = 5;

        [Parameter]
        public TimeSpan Timeout { get; set; } = TimeSpan.MaxValue;

        [Parameter(ValueFromPipeline = true)]
        public override object Server { get; set; }

        [Parameter]
        public override object Credential { get; set; }
    }
}
