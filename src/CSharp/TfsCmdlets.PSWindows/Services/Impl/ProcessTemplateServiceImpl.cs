using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.Server;

namespace TfsCmdlets.Services.Impl
{
    [Export(typeof(IProcessTemplateService))]
    internal class ProcessTemplateServiceImpl: IProcessTemplateService
    {
        public TemplateHeader GetTemplate(object name, object collection, object server, object credential)
        {
            var templates = GetTemplates(name, collection, server, credential).ToList();

            if (templates.Count == 0)
                throw new Exception($"Invalid process template name '{name}'");

            if (templates.Count == 1)
                return templates[0];

            var names = string.Join(", ", templates.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{name}' matches {templates.Count} team project collections: {names}. Please choose a more specific value for the Name argument and try again");
        }

        public IEnumerable<TemplateHeader> GetTemplates(object template, object collection, object server, object credential)
        {
            switch (template)
            {
                case TemplateHeader h:
                {
                    yield return h;
                    break;
                }
                case string s:
                {
                    var tpc = TeamProjectCollectionService.GetCollection(collection, server, credential);
                    var processTemplateSvc = tpc.GetService<IProcessTemplates>();
                    foreach (var t in processTemplateSvc.TemplateHeaders().Where(o => o.Name.IsLike(s)))
                    {
                        yield return t;
                    }
                    break;
                }
                default:
                {
                    throw new Exception($"Invalid process template name {template}");
                }
            }
        }

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService TeamProjectCollectionService { get; set; }
    }
}
