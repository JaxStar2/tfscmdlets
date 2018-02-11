using System.Collections.Generic;
using Microsoft.TeamFoundation.Server;

namespace TfsCmdlets.Services
{
    public interface IProcessTemplateService
    {
        TemplateHeader GetTemplate(object template, object collection, object server, object credential);

        IEnumerable<TemplateHeader> GetTemplates(object templates, object collection, object server, object credential);

        string GetTemplateData(TemplateHeader processTemplate, object collection, object server, object credential);
    }
}