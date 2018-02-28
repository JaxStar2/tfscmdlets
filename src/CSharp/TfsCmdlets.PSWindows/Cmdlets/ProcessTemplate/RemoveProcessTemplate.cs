using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.ProcessTemplate
{
    [Cmdlet(VerbsCommon.Remove, "ProcessTemplate", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    public class RemoveProcessTemplate : ProcessTemplateCmdletBase
    {
        protected override void ProcessRecord()
        {
            var template = GetProcessTemplate();

            if (!ShouldProcess(template.Name, "Delete process template from server")) return;

            var tpc = GetCollection();

            ProcessTemplateService.DeleteTemplate(template.TemplateId);
        }

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        public override object ProcessTemplate { get; set; }

        [Parameter]
        public override object Collection { get; set; }
    }
}
