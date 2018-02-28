using System;
using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Cmdlets.Connection;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.TeamProject
{
    [Cmdlet(VerbsCommon.Remove, "TeamProject", DefaultParameterSetName = "Get by project", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    public class RemoveTeamProject : ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var project = GetProject();

            if (!ShouldProcess(project.Name, "Delete team project"))
                return;

            ProjectService.DeleteProject(project, Collection, Server, Credential);
        }

        [Parameter(Position = 0, ValueFromPipeline = true)]
        public override object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }
    }
}