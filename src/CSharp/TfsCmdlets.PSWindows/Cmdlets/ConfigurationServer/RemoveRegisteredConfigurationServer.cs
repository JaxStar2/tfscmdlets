using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.ConfigurationServer
{
    [Cmdlet(VerbsCommon.Remove, "RegisteredConfigurationServer", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess=true)]
    public class RemoveRegisteredConfigurationServer: BaseCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(Name, "Remove registered server"))
                return;

            RegisteredConnectionService.UnregisterConfigurationServer(Name);
        }

        [Import(typeof(IRegisteredConnectionService))]
        private IRegisteredConnectionService RegisteredConnectionService { get; set; }
    }
}
