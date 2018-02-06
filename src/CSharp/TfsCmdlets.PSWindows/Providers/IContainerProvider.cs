using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Providers
{
    public interface IContainerProvider
    {
        TfsConfigurationServer GetServer(object server, object credential);

        IEnumerable<TfsConfigurationServer> GetServers(object servers, object credential);

        TfsTeamProjectCollection GetCollection(object collection, object server, object credential);

        IEnumerable<TfsTeamProjectCollection> GetCollections(object collections, object server, object credential);

        Project GetProject(object project, object collection, object server, object credential);

        IEnumerable<Project> GetProjects(object projects, object collection, object server, object credential);

    }
}
