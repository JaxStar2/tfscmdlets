using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IGitRepositoryService))]
    public class GitRepositoryService : ServiceBase<GitRepository, IGitRepositoryAdapter>, IGitRepositoryService
    {
        public IGitRepositoryAdapter CreateRepository(string repository, object project, object collection, object server, object credential)
        {
            var tp = (Project)ProjectService.GetProject(project, collection, server, credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var gitClient = tpc.GetClient<GitHttpClient>();
            var repoToCreate = new GitRepository
            {
                Name = repository,
                ProjectReference = new TeamProjectReference
                {
                    Id = tp.Guid,
                    Name = tp.Name
                }
            };
            var repo = gitClient.CreateRepositoryAsync(repoToCreate, tp.Name).Result;

            return new GitRepositoryAdapter(repo);
        }

        public void DeleteRepository(object repository, object project, object collection, object server, object credential)
        {
            var repo = GetRepository(repository, project, collection, server, credential);
            var tp = (Project)ProjectService.GetProject(project, collection, server, credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var gitClient = tpc.GetClient<GitHttpClient>();

            gitClient.DeleteRepositoryAsync(tp.Guid, repo.Id).Wait();
        }

        public IGitRepositoryAdapter RenameRepository(object repository, string newName, object project, object collection, object server, object credential)
        {
            var repo = (GitRepository)GetRepository(repository, project, collection, server, credential).Instance;
            var tp = (Project)ProjectService.GetProject(project, collection, server, credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var gitClient = tpc.GetClient<GitHttpClient>();

            return new GitRepositoryAdapter(gitClient.RenameRepositoryAsync(repo, newName).Result);
        }

        #region Get Items

        protected override string ItemName => "repository";
        protected override Func<GitRepository, string> ItemDescriptor => (r => r.Name);

        public IGitRepositoryAdapter GetRepository(object repository, object project, object collection, object server, object credential)
        {
            return new GitRepositoryAdapter(GetItem(repository, project, collection, server, credential));
        }

        public IEnumerable<IGitRepositoryAdapter> GetRepositories(object repository, object project, object collection, object server, object credential)
        {
            return GetItems(repository, project, collection, server, credential).Select(item => new GitRepositoryAdapter(item));
        }

        protected override IEnumerable<GitRepository> GetAllItems(object item, ScopeObjects so)
        {
            var tp = (Project)ProjectService.GetProject(so.Project, so.Collection, so.Server, so.Credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var gitService = tpc.GetService<Microsoft.TeamFoundation.Git.Client.GitRepositoryService>();
            var repos = gitService.QueryRepositories(tp.Name).ToList();

            return repos;
        }

        #endregion

        #region Imports

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }

        #endregion
    }
}
