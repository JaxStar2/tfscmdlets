using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Server;
using TfsCmdlets.Services;

namespace TfsCmdlets.Cmdlets.ProcessTemplate
{
    public abstract class ProcessTemplateCmdletBase : CollectionLevelCmdlet
    {
        [Import(typeof(IProcessTemplateService))]
        protected IProcessTemplateService ProcessTemplateService { get; set; }

        protected TemplateHeader GetProcessTemplate()
        {
            return ProcessTemplateService.GetTemplate(ProcessTemplate, Collection, Server, Credential);
        }

        protected IEnumerable<TemplateHeader> GetProcessTemplates()
        {
            return ProcessTemplateService.GetTemplates(ProcessTemplate, Collection, Server, Credential);
        }

        public abstract object ProcessTemplate { get; set; }
    }
}