using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IWorkItemTagService))]
    public class WorkItemTagService: IWorkItemTagService
    {
        public IWorkItemTagAdapter GetTag(object tag, object project, object collection, object server, object credential)
        {
            var items = GetTags(tag, project, collection, server, credential).ToList();

            if (items.Count == 0)
                throw new Exception($"Invalid work item tag '{tag}'");

            if (items.Count == 1)
                return items[0];

            var names = string.Join(", ", items.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{tag}' matches {items.Count} work item tags: {names}. Please choose a more specific value for the -Tag argument and try again");
        }

        public IEnumerable<IWorkItemTagAdapter> GetTags(object tag, object project, object collection, object server, object credential)
        {
            while (true)
            {
                switch (tag)
                {
                    case PSObject pso:
                    {
                        tag = pso.BaseObject;
                        continue;
                    }
                    case IWorkItemTagAdapter a:
                    {
                        yield return a;
                        break;
                    }
                    case string s:
                    {
                        var tp = (Project)ProjectService.GetProject(project, collection, server, credential).Instance;
                        var tpc = tp.Store.TeamProjectCollection;
                        var tagClient = tpc.GetClient<TaggingHttpClient>();
                        var allTags = tagClient.GetTagsAsync(tp.Guid, true).Result;

                        foreach (var t in allTags.Where(o => o.Name.IsLike(s)))
                        {
                            yield return new WorkItemTagAdapter(t);
                        }
                        break;
                    }
                    default:
                    {
                        throw new ArgumentException($"Invalid work item tag {tag}");
                    }
                }
                break;
            }
        }

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }
    }
}
