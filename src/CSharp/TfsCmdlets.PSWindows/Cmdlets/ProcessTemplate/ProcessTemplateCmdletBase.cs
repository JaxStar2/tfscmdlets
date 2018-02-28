using System.Collections.Generic;
using System.ComponentModel.Composition;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.ProcessTemplate
{
    public abstract class ProcessTemplateCmdletBase : CollectionLevelCmdlet
    {
        [Import(typeof(IProcessTemplateService))]
        protected IProcessTemplateService ProcessTemplateService { get; set; }

        protected ITemplateHeaderAdapter GetProcessTemplate()
        {
            return ProcessTemplateService.GetTemplate(ProcessTemplate, Collection, Server, Credential);
        }

        protected IEnumerable<ITemplateHeaderAdapter> GetProcessTemplates()
        {
            return ProcessTemplateService.GetTemplates(ProcessTemplate, Collection, Server, Credential);
        }

        public abstract object ProcessTemplate { get; set; }
    }
}