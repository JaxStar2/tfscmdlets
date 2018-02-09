using System;
using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Cmdlets.Connection;
using TfsCmdlets.Services;

namespace TfsCmdlets.Cmdlets.TeamProject
{
    [Cmdlet(VerbsCommon.Remove, "TeamProject", DefaultParameterSetName = "Get by project", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
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
            throw new NotImplementedException();
        }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }
    }
}