using System.Collections.Generic;
using Microsoft.TeamFoundation.Server;

namespace TfsCmdlets.Providers
{
    public interface IProcessTemplateProvider
    {
        TemplateHeader GetTemplate(object template, object collection, object server, object credential);

        IEnumerable<TemplateHeader> GetTemplates(object templates, object collection, object server, object credential);
    }
}
