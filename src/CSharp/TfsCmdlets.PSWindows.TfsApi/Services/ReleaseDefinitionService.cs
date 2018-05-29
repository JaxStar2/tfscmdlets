using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Clients;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IReleaseDefinitionService))]
    public class ReleaseDefinitionService : ServiceBase<ReleaseDefinition, ReleaseDefinitionAdapter>, IReleaseDefinitionService
    {
        #region Get Items

        public IReleaseDefinitionAdapter GetReleaseDefinition(object build, object project, object collection, object server, object credential)
            => new ReleaseDefinitionAdapter(GetItem(build, project, collection, server, credential));

        public IEnumerable<IReleaseDefinitionAdapter> GetReleaseDefinitions(object build, object project, object collection, object server, object credential)
            => GetItems(build, project, collection, server, credential).Select(b => new ReleaseDefinitionAdapter(b));

        protected override IEnumerable<ReleaseDefinition> GetAllItems(object item, ScopeObjects so)
        {
            var tp = (Project)ProjectService.GetProject(so.Project, so.Collection, so.Server, so.Credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var rmSvc = tpc.GetClient<ReleaseHttpClient>();

            return rmSvc.GetReleaseDefinitionsAsync(tp.Guid).Result;
        }

        protected override string ItemName => "build definition";

        protected override Func<ReleaseDefinition, string> ItemDescriptor => (b => b.Name);

        #endregion

        #region Imports

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService CollectionService { get; set; }

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }

        #endregion
    }
}
