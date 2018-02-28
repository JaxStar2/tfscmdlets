using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IProcessTemplateService))]
    public class ProcessTemplateService : IProcessTemplateService
    {
        public ITemplateHeaderAdapter GetTemplate(object name, object collection, object server, object credential)
        {
            var templates = GetTemplates(name, collection, server, credential).ToList();

            if (templates.Count == 0)
                throw new Exception($"Invalid process template name '{name}'");

            if (templates.Count == 1)
                return templates[0];

            var names = string.Join(", ", templates.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{name}' matches {templates.Count} team project collections: {names}. Please choose a more specific value for the Name argument and try again");
        }

        public IEnumerable<ITemplateHeaderAdapter> GetTemplates(object template, object collection, object server, object credential)
        {
            while (true)
            {
                switch (template)
                {
                    case PSObject pso:
                        {
                            template = pso.BaseObject;
                            continue;
                        }
                    case TemplateHeader h:
                        {
                            yield return new TemplateHeaderAdapter(h);
                            break;
                        }
                    case string s:
                        {
                            var tpc = ((TfsTeamProjectCollection) TeamProjectCollectionService.GetCollection(collection, server, credential).Instance);
                            var svc = tpc.GetService<IProcessTemplates>();

                            foreach (var t in svc.TemplateHeaders().Where(o => o.Name.IsLike(s)))
                            {
                                yield return new TemplateHeaderAdapter(t);
                            }

                            break;
                        }
                    default:
                        {
                            throw new Exception($"Invalid process template name {template}");
                        }
                }

                break;
            }
        }

        public string GetTemplateData(ITemplateHeaderAdapter processTemplate, object collection, object server, object credential)
        {
            throw new NotImplementedException();
        }

        public void AddUpdateTemplate(string name, string description, string metadata, string state, string zipFile)
        {
            throw new NotImplementedException();
        }

        public void DeleteTemplate(int templateId)
        {
            throw new NotImplementedException();
        }

        public string GetTemplateData(TemplateHeader processTemplate, object collection, object server, object credential)
        {
            var tpc = ((TfsTeamProjectCollection) TeamProjectCollectionService.GetCollection(collection, server, credential).Instance);
            var svc = tpc.GetService<IProcessTemplates>();

            return svc.GetTemplateData(processTemplate.TemplateId);
        }

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService TeamProjectCollectionService { get; set; }
    }
}
