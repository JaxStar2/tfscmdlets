using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IWorkItemTagService))]
    public class WorkItemTagService : ServiceBase<WebApiTagDefinition, IWorkItemTagAdapter>, IWorkItemTagService
    {
        #region Get Items

        protected override string ItemName => "work item tag";
        protected override Func<WebApiTagDefinition, string> ItemDescriptor => (t => t.Name);

        public IWorkItemTagAdapter GetTag(object tag, object project, object collection, object server, object credential)
            => new WorkItemTagAdapter(GetItem(tag, project, collection, server, credential));

        public IEnumerable<IWorkItemTagAdapter> GetTags(object tag, object project, object collection, object server, object credential)
            => GetItems(tag, project, collection, server, credential).Select(t => new WorkItemTagAdapter(t));

        protected override IEnumerable<WebApiTagDefinition> GetAllItems(object item, ScopeObjects so)
        {
            var tp = (Project)ProjectService.GetProject(so.Project, so.Collection, so.Server, so.Credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var tagClient = tpc.GetClient<TaggingHttpClient>();

            return tagClient.GetTagsAsync(tp.Guid, true).Result;
        }
        
        #endregion

        #region Imports

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }

        #endregion
    }
}
