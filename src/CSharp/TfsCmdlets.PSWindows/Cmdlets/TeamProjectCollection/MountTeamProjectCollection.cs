using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.TeamFoundation.Framework.Client;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsData.Mount, "TeamProjectCollection", ConfirmImpact = ConfirmImpact.Medium)]
    public class MountTeamProjectCollection : ServerLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var configServer = GetServer();
            var tpcService = configServer.GetService<ITeamProjectCollectionService>();
            var servicingTokens = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(DatabaseName))
                servicingTokens["CollectionDatabaseName"] = DatabaseName;

            if (string.IsNullOrWhiteSpace(ConnectionString))
                ConnectionString =
                    $"Data source={DatabaseServer}; Integrated Security=true; Initial Catalog={DatabaseName}";

            var tpcJob = tpcService.QueueAttachCollection(ConnectionString, servicingTokens, Clone.IsPresent, Name, Description, $"~/{Name}/");
            var result = tpcService.WaitForCollectionServicingToComplete(tpcJob, Timeout);

            WriteObject(GetServer().GetTeamProjectCollection(result.Id));
        }

        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; }

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
