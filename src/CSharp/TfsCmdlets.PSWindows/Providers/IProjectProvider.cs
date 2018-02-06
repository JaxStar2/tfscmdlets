using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Providers
{
    public interface IProjectProvider
    {
        Project GetProject(object project, object collection, object server, object credential);

        IEnumerable<Project> GetProjects(object projects, object collection, object server, object credential);
    }
}
