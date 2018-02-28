using System;
using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsCommon.Remove, "TeamProjectCollection", ConfirmImpact = ConfirmImpact.High,
        SupportsShouldProcess = true)]
    public class RemoveTeamProjectCollection : CollectionLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var collections = GetCollections();

            foreach (var tpc in collections)
            {
                if (!ShouldProcess(tpc.Name, "Delete team project collection")) continue;

                CollectionService.DeleteCollection(Collection, Server, Credential);
            }
        }

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [SupportsWildcards]
        public override object Collection { get; set; }
    }
}