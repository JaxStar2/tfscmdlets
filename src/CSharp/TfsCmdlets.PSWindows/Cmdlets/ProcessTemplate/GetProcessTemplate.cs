using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Server;

namespace TfsCmdlets.Cmdlets.ProcessTemplate
{

    [Cmdlet(verbName: VerbsCommon.Get, nounName: "ProcessTemplate")]
    [OutputType(typeof(Microsoft.TeamFoundation.Server.TemplateHeader))]
    public class GetProcessTemplate : CollectionLevelCmdlet
    {
        [Parameter(Position = 0)]
        [SupportsWildcards()]
        public string Name { get; set; } = "*";

        protected override void ProcessRecord()
        {
            var tpc = GetCollection();
            var templateHeaders = GetTemplates(Name, tpc);

            WriteObject(templateHeaders, true);
        }

        internal static TemplateHeader GetTemplate(object name, object collection, object credential = null)
        {
            var templates = GetTemplates(name, collection, credential).ToList();

            if (templates.Count == 0)
            {
                throw new PSArgumentException($"Invalid process template name '{name}'", nameof(Name));
            }
            else if (templates.Count > 1)
            {
                var names = string.Join(", ", templates.Select(o => o.Name).ToArray());
                throw new PSArgumentException($"Ambiguous name '{name}' matches {templates.Count} team project collections: {names}. Please choose a more specific value for the {nameof(Name)} argument and try again", nameof(Name));
            }

            return templates[0];
        }

        internal static IEnumerable<TemplateHeader> GetTemplates(object template, object collection, object credential = null)
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
                    var tpc = CollectionLevelCmdlet.GetCollection(collection, credential);
                    var processTemplateSvc = tpc.GetService<Microsoft.TeamFoundation.Server.IProcessTemplates>();
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
    }
}