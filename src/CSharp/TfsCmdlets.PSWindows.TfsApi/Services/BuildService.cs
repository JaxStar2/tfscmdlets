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
    [Export(typeof(IBuildService))]
    public class BuildService: IBuildService
    {
        public IBuildDefinitionAdapter GetBuildDefinition(object build, object project, object collection, object server,
            object credential)
        {
            var builds = GetBuildDefinitions(build, project, collection, server, credential).ToList();

            if (builds.Count == 0)
                throw new Exception($"Invalid build definition '{build}'");

            if (builds.Count == 1)
                return builds[0];

            var names = string.Join(", ", builds.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{build}' matches {builds.Count} build definitions: {names}. Please choose a more specific value for the -Definition argument and try again");
        }

        public IEnumerable<IBuildDefinitionAdapter> GetBuildDefinitions(object build, object project, object collection, object server,
            object credential)
        {
            var tp = (Project) ProjectService.GetProject(project, collection, server, credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var buildSvc = tpc.GetClient<BuildHttpClient>();

            var builds = buildSvc.GetFullDefinitionsAsync(tp.Guid).Result;

            foreach (var b in builds)
            {
                yield return new BuildDefinitionAdapter(b);
            }
        }

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService CollectionService { get; set; }

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }
    }
}
