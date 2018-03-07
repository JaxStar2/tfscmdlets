using System;
using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.TeamProject
{
    [Cmdlet(VerbsCommon.New, "TeamProject", ConfirmImpact = ConfirmImpact.Medium,
        SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.WorkItemTracking.Client.Project,Microsoft.TeamFoundation.WorkItemTracking.Client")]
    public class NewTeamProject : ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!(Project is string project))
            {
                throw new ArgumentException($"Invalid team project '{Project}'");
            }

            if (!ShouldProcess(project, "Create team project"))
                return;

            var tp = ProjectService.CreateProject(project, Description, ProcessTemplate, SourceControl, Collection, Server, Credential);

            if (Passthru)
                WriteObject(tp);
        }

        [Parameter(Position = 0, Mandatory = true)]
        [Alias("Name")]
        public override object Project { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public SourceControlType SourceControl { get; set; }

        [Parameter]
        public object ProcessTemplate { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Import(typeof(IProcessTemplateService))]
        private IProcessTemplateService ProcessTemplateService { get; set; }
    }
}