using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Services
{
    public interface IProjectService
    {
        Project GetProject(object project, object collection, object server, object credential);

        IEnumerable<Project> GetProjects(object projects, object collection, object server, object credential);
    }
}
