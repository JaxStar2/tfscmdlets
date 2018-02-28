using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Cmdlets.Connection;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.TeamProject
{
    [Cmdlet(VerbsCommon.Get, "TeamProject", DefaultParameterSetName = "Get by project")]
    [OutputType("Microsoft.TeamFoundation.WorkItemTracking.Client.Project")]
    public class GetTeamProject : ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (Current.IsPresent)
            {
                WriteObject(CurrentConnectionService.TeamProject);
                return;
            }

            WriteObject(GetProjects(Project, Collection, Server, Credential), true);
        }

        [Parameter(Position = 0, ParameterSetName = "Get by project")]
        public override object Project { get; set; } = "*";

        [Parameter(ValueFromPipeline = true, Position = 1, ParameterSetName = "Get by project")]
        public override object Collection { get; set; }

        [Parameter(Position = 0, ParameterSetName = "Get current")]
        public SwitchParameter Current { get; set; }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }
    }
}