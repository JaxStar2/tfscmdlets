using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IBuildDefinitionService))]
    public class BuildDefinitionService : ServiceBase<BuildDefinition, BuildDefinitionAdapter>, IBuildDefinitionService
    {
        #region Get Items

        public IBuildDefinitionAdapter GetBuildDefinition(object build, object project, object collection, object server, object credential)
            => new BuildDefinitionAdapter(GetItem(build, project, collection, server, credential));

        public IEnumerable<IBuildDefinitionAdapter> GetBuildDefinitions(object build, object project, object collection, object server, object credential)
            => GetItems(build, project, collection, server, credential).Select(b => new BuildDefinitionAdapter(b));

        protected override IEnumerable<BuildDefinition> GetAllItems(object item, ScopeObjects so)
        {
            var tp = (Project)ProjectService.GetProject(so.Project, so.Collection, so.Server, so.Credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var buildSvc = tpc.GetClient<BuildHttpClient>();

            return buildSvc.GetFullDefinitionsAsync(tp.Guid).Result;
        }

        protected override string ItemName => "build definition";

        protected override Func<BuildDefinition, string> ItemDescriptor => (b => b.Name);

        #endregion

        #region Imports

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService CollectionService { get; set; }

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }

        #endregion
    }
}
