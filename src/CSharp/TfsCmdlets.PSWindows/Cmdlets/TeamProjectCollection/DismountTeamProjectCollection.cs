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

            if (!ShouldProcess(tpc.Name, "Detach Project Collection")) return;

            throw new NotImplementedException();

            //tpc.Stop();

            //var configServer = tpc.ConfigurationServer;
            //var tpcService = configServer.GetService<ITeamProjectCollectionService>();
            //var collectionInfo = tpcService.GetCollection(tpc.InstanceId);
            //var tpcJob = tpcService.QueueDetachCollection(collectionInfo.Id, null, Reason, out var connectionString);

            //collectionInfo = tpcService.WaitForCollectionServicingToComplete(tpcJob, Timeout);
            //collectionInfo.Refresh();

            //WriteObject(connectionString);
        }

        [Parameter(Mandatory = true, Position = 0)]
        public override object Collection { get; set; }

        [Parameter]
        public string Reason { get; set; }

        [Parameter]
        public TimeSpan Timeout { get; set; } = TimeSpan.MaxValue;
    }
}