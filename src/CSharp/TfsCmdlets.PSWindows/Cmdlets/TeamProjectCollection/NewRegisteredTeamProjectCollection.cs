using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsCommon.New, "RegisteredTeamProjectCollection", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Client.RegisteredProjectCollection")]
    public class NewRegisteredTeamProjectCollection: CollectionLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var collection = GetCollection();

            if (!ShouldProcess(collection?.Name, "Add server to list of registered servers")) return;

            RegisteredConnectionService.RegisterProjectCollection(collection);

            if (Passthru)
            {
                WriteObject(RegisteredConnectionService.GetRegisteredProjectCollections("*")
                    .First(o => o.Uri == collection.Uri));
            }
        }

        [Parameter(Position=0, Mandatory=true, ValueFromPipeline = true)]
        public override object Collection { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Import(typeof(IRegisteredConnectionService))]
        private IRegisteredConnectionService RegisteredConnectionService { get; set; }
    }
}
