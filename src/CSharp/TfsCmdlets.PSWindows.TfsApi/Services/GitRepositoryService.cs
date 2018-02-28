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
    public class GitRepositoryService : IGitRepositoryService
    {
        public IGitRepositoryAdapter GetRepository(object repository, object project, object collection, object server,
            object credential)
        {
            var repositories = GetRepositories(repository, project, collection, server, credential).ToList();

            if (repositories.Count == 0)
                throw new Exception($"Invalid repository name '{repository}'");

            if (repositories.Count == 1)
                return repositories[0];

            var names = string.Join(", ", repositories.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{repository}' matches {repositories.Count} repositories: {names}. Please choose a more specific value for the -Repository argument and try again");
        }

        public IEnumerable<IGitRepositoryAdapter> GetRepositories(object repository, object project, object collection, object server, object credential)
        {
            while (true)
            {
                switch (repository)
                {
                    case PSObject pso:
                        {
                            repository = pso.BaseObject;
                            continue;
                        }
                    case IGitRepositoryAdapter r:
                        {
                            yield return r;
                            break;
                        }
                    case string s:
                        {
                            var tp = (Project)ProjectService.GetProject(project, collection, server, credential).Instance;
                            var tpc = tp.Store.TeamProjectCollection;
                            var gitService = tpc.GetService<Microsoft.TeamFoundation.Git.Client.GitRepositoryService>();

                            foreach (var r in gitService.QueryRepositories(tp.Name).Where(o => o.Name.IsLike(s)))
                            {
                                yield return new GitRepositoryAdapter(r);
                            }
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException($"Invalid git repository name {repository}");
                        }
                }
                break;
            }
        }

        public IGitRepositoryAdapter CreateRepository(string repository, object project, object collection, object server,
            object credential)
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

            return new GitRepositoryAdapter(gitClient.CreateRepositoryAsync(repoToCreate, tp.Name).Result);
        }

        public void DeleteRepository(object repository, object project, object collection, object server, object credential)
        {
            var repo = GetRepository(repository, project, collection, server, credential);
            var tp = (Project)ProjectService.GetProject(project, collection, server, credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var gitClient = tpc.GetClient<GitHttpClient>();

            gitClient.DeleteRepositoryAsync(tp.Guid, repo.Id).Wait();
        }

        public IGitRepositoryAdapter RenameRepository(object repository, string newName, object project, object collection,
            object server, object credential)
        {
            var repo = (GitRepository)GetRepository(repository, project, collection, server, credential).Instance;
            var tp = (Project)ProjectService.GetProject(project, collection, server, credential).Instance;
            var tpc = tp.Store.TeamProjectCollection;
            var gitClient = tpc.GetClient<GitHttpClient>();

            return new GitRepositoryAdapter(gitClient.RenameRepositoryAsync(repo, newName).Result);
        }

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }
    }
}
