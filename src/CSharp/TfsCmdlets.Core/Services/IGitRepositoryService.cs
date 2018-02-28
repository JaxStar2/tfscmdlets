using System.Collections.Generic;
using System.Threading.Tasks;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IGitRepositoryService
    {
        IGitRepositoryAdapter GetRepository(object repository, object project, object collection, object server, object credential);
        IEnumerable<IGitRepositoryAdapter> GetRepositories(object repository, object project, object collection, object server, object credential);
        IGitRepositoryAdapter CreateRepository(string repository, object project, object collection, object server, object credential);
        void DeleteRepository(object repository, object project, object collection, object server, object credential);
        IGitRepositoryAdapter RenameRepository(object repository, string newName, object project, object collection, object server, object credential);
    }
}