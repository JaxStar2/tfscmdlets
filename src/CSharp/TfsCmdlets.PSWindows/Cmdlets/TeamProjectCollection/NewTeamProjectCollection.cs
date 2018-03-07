using System;
using System.Collections.Generic;
using System.Management.Automation;
using TfsCmdlets.Core;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsCommon.New, "TeamProjectCollection", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Client.TfsTeamProjectCollection,Microsoft.TeamFoundation.Client")]
    public class NewTeamProjectCollection : CollectionLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!(Collection is string name))
            {
                throw new Exception($"Invalid team project collection name {Collection}");
            }

            if (!ShouldProcess(name, "Create team project collection")) return;

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                ConnectionString = $"Data source={DatabaseServer}; Integrated Security=true; " +
                                   "Initial Catalog={DatabaseName}";
            }

            WriteObject(CollectionService.CreateCollection(name, Description, ConnectionString, Default, 
                UseExistingDatabase, InitialState, Timeout, Server, Credential));
        }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [Alias("Name")]
        public override object Collection { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter(ParameterSetName = "Use database server", Mandatory = true)]
        public string DatabaseServer { get; set; }

        [Parameter(ParameterSetName = "Use database server")]
        public string DatabaseName { get; set; }

        [Parameter(ParameterSetName = "Use connection string", Mandatory = true)]
        public string ConnectionString { get; set; }

        [Parameter]
        public SwitchParameter Default { get; set; }

        [Parameter]
        public SwitchParameter UseExistingDatabase { get; set; }

        [Parameter]
        [ValidateSet("Started", "Stopped")]
        public ServiceHostStatus InitialState { get; set; } = ServiceHostStatus.Started;

        [Parameter]
        public int PollingInterval { get; set; } = 5;

        [Parameter]
        public TimeSpan Timeout { get; set; } = TimeSpan.MaxValue;

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Parameter]
        public override object Server { get; set; }

        public override object Credential { get; set; }
    }
}
