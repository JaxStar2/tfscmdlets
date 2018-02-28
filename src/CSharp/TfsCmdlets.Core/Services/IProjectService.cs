using System.Collections.Generic;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IProjectService
    {
        IProjectAdapter GetProject(object project, object collection, object server, object credential);

        IEnumerable<IProjectAdapter> GetProjects(object projects, object collection, object server, object credential);

        IProjectAdapter CreateProject(string name, string description, object processTemplate, SourceControlType sourceControlType,
            object collection, object server, object credential);

        void DeleteProject(object project, object collection, object server, object credential);
    }
}
