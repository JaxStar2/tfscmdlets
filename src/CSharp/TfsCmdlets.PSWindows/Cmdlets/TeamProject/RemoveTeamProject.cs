using System.Management.Automation;
using TfsCmdlets.Cmdlets.Connection;

namespace TfsCmdlets.Cmdlets.TeamProject
{
    [Cmdlet(VerbsCommon.Remove, "TeamProject", DefaultParameterSetName = "Get by project")]
    [OutputType(typeof(Microsoft.TeamFoundation.WorkItemTracking.Client.Project))]
    public class RemoveTeamProject : ProjectLevelCmdlet
    {
        [Parameter(Position = 0, ParameterSetName = "Get by project")]
        public override object Project { get; set; } = "*";

        [Parameter(ValueFromPipeline = true, Position = 1, ParameterSetName = "Get by project")]
        public override object Collection { get; set; }

        [Parameter(Position = 0, ParameterSetName = "Get current")]
        public SwitchParameter Current { get; set; }

        protected override void ProcessRecord()
        {
            if (Current.IsPresent)
            {
                WriteObject(CurrentConnections.TeamProject);
                return;
            }

            foreach (var p in GetProjects(Project, Collection, Server, Credential))
            {
               WriteObject(p);
            }
        }
    }
}