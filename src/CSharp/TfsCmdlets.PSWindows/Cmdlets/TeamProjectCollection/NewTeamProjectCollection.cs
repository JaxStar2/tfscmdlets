using System;
using System.Collections.Generic;
using System.Management.Automation;
using TfsCmdlets.Core;
using TfsCmdlets.DscResources;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsCommon.New, "TeamProjectCollection", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType(typeof(TfsTeamProjectCollection))]
    public class NewTeamProjectCollection : ServerLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(Name, "Create team project collection")) return;

            var configServer = GetServer();

            throw new NotImplementedException();
            
            //var tpcService = configServer.GetService<ITeamProjectCollectionService>();
            //var servicingTokens = new Dictionary<string, string>
            //{
            //    ["SharePointAction"] = "None",
            //    ["ReportingAction"] = "None"
            //};

            //if (!string.IsNullOrWhiteSpace(DatabaseName))
            //    servicingTokens["CollectionDatabaseName"] = DatabaseName;

            //if (UseExistingDatabase.IsPresent)
            //    servicingTokens["UseExistingDatabase"] = UseExistingDatabase.IsPresent.ToString();

            //if (string.IsNullOrWhiteSpace(ConnectionString))
            //    ConnectionString = $"Data source={DatabaseServer}; Integrated Security=true";

            //var tpcJob = tpcService.QueueCreateCollection(Name, Description, Default.IsPresent, $"~/{Name}/",
            //    InitialState, servicingTokens, ConnectionString,
            //    null,   // Default connection string
            //    null);  // Default category connection strings

            //var result = tpcService.WaitForCollectionServicingToComplete(tpcJob, Timeout);

            //if (Passthru)
            //{
            //    WriteObject(GetServer().GetTeamProjectCollection(result.Id));
            //}
        }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public string Name { get; set; }

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
        public ServiceHostStatus InitialState { get; set; }

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
