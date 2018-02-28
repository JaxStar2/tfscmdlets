using System.Collections.Generic;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IProcessTemplateService
    {
        ITemplateHeaderAdapter GetTemplate(object template, object collection, object server, object credential);

        IEnumerable<ITemplateHeaderAdapter> GetTemplates(object templates, object collection, object server, object credential);

        string GetTemplateData(ITemplateHeaderAdapter processTemplate, object collection, object server, object credential);

        void AddUpdateTemplate(string name, string description, string metadata, string state, string zipFile);

        void DeleteTemplate(int templateId);
    }
}