using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.ProcessConfiguration
{
    [Cmdlet(VerbsCommon.Get, "ProcessConfiguration")]
    [OutputType("Microsoft.TeamFoundation.ProcessConfiguration.Client.ProjectProcessConfiguration")]
    public class GetProcessConfiguration: ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(ProcessConfigurationService.GetProcessConfiguration(Project, Collection, Server, Credential));
        }

        [Parameter(ValueFromPipeline = true)]
        public override object Project { get; set; }

        [Import(typeof(IProcessConfigurationService))]
        private IProcessConfigurationService ProcessConfigurationService { get; set; }
    }
}
