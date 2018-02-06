using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Cmdlets.ConfigurationServer;
using TfsCmdlets.Cmdlets.Connection;
using TfsCmdlets.Cmdlets.TeamProjectCollection;

namespace TfsCmdlets.Providers.Impl
{
    [Export(typeof(IContainerProvider))]
    internal sealed class ContainerProviderImpl : IContainerProvider
    {
        public TfsConfigurationServer GetServer(object server, object credential)
        {
            var servers = GetServers(server, credential).ToList();

            if (servers.Count == 0)
                throw new Exception($"Invalid server name '{server}'");

            if (servers.Count == 1)
                return servers[0];

            var names = string.Join(", ", servers.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{server}' matches {servers.Count} servers: {names}. " +
                                "Please choose a more specific value for the -Server argument and try again");
        }

        public IEnumerable<TfsConfigurationServer> GetServers(object server, object credential)
        {
            while (true)
            {
                var cred = GetCredential.Get(credential);

                switch (server)
                {
                    case PSObject pso:
                    {
                        server = pso.BaseObject;
                        continue;
                    }
                    case TfsConfigurationServer s:
                    {
                        yield return s;
                        break;
                    }
                    case Uri u:
                    {
                        yield return new TfsConfigurationServer(u, cred);
                        break;
                    }
                    case string s when Uri.IsWellFormedUriString(s, UriKind.Absolute):
                    {
                        yield return new TfsConfigurationServer(new Uri(s), cred);
                        break;
                    }
                    case string s when !string.IsNullOrWhiteSpace(s):
                    {
                        var serverNames = GetRegisteredConfigurationServer.Get(s);

                        foreach (var svr in serverNames)
                        {
                            yield return new TfsConfigurationServer(svr.Uri, cred);
                        }
                        break;
                    }
                    case null when CurrentConnections.ConfigurationServer != null:
                    {
                        yield return CurrentConnections.ConfigurationServer;
                        break;
                    }
                    default:
                    {
                        throw new PSArgumentException("No connection information available. " +
                            "Either supply a valid -Server argument or use Connect-TfsConfigurationServer " +
                            "prior to invoking this cmdlet.");
                    }
                }
                break;
            }
        }

        public TfsTeamProjectCollection GetCollection(object collection, object server, object credential)
        {
            var collections = GetCollections(collection, server, credential).ToList();

            if (collections.Count == 0)
                throw new Exception($"Invalid team project collection name '{collection}'");

            if (collections.Count == 1)
                return collections[0];

            var names = string.Join(", ", collections.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{collection}' matches {collections.Count} team project collections: {names}. Please choose a more specific value for the -Collection argument and try again");
        }

        public IEnumerable<TfsTeamProjectCollection> GetCollections(object collection, object server, object credential)
        {
            while (true)
            {
                var cred = GetCredential.Get(credential);

                switch (collection)
                {
                    case PSObject pso:
                        {
                            collection = pso.BaseObject;
                            continue;
                        }
                    case TfsTeamProjectCollection tpc:
                        {
                            yield return tpc;
                            break;
                        }
                    case Uri u:
                        {
                            yield return new TfsTeamProjectCollection(u, cred);
                            break;
                        }
                    case string s when Uri.IsWellFormedUriString(s, UriKind.Absolute):
                        {
                            yield return new TfsTeamProjectCollection(new Uri(s), cred);
                            break;
                        }
                    case string s when CurrentConnections.ConfigurationServer != null && !string.IsNullOrWhiteSpace(s):
                        {
                            var configServer = CurrentConnections.ConfigurationServer;
                            var filter = new[] { CatalogResourceTypes.ProjectCollection };
                            var collections = configServer.CatalogNode.QueryChildren(filter, false, CatalogQueryOptions.None);
                            var result = collections.Select(o => o.Resource).Where(o => o.DisplayName.IsLike(s)).ToList();

                            if (result.Count == 0)
                            {
                                throw new PSArgumentException($"Invalid or non-existent Team Project Collection(s): {s}", "Collection");
                            }

                            foreach (var resource in result)
                            {
                                yield return configServer.GetTeamProjectCollection(new Guid(resource.Properties["InstanceId"]));
                            }

                            break;
                        }
                    case string s when !string.IsNullOrWhiteSpace(s):
                        {
                            var registeredCollection = GetRegisteredTeamProjectCollection.Get(s);

                            foreach (var tpc in registeredCollection)
                            {
                                yield return new TfsTeamProjectCollection(tpc.Uri, cred);
                            }

                            break;
                        }
                    case null when CurrentConnections.TeamProjectCollection != null:
                        {
                            yield return CurrentConnections.TeamProjectCollection;
                            break;
                        }
                    default:
                        {
                            throw new PSArgumentException("No TFS connection information available. Either supply a valid -Collection argument or use Connect-TfsTeamProjectCollection prior to invoking this cmdlet.");
                        }
                }
                break;
            }
        }

        public Project GetProject(object project, object collection, object server, object credential)
        {
            var projects = GetProjects(project, collection, server, credential).ToList();

            if (projects.Count == 0)
                throw new Exception($"Invalid project name '{project}'");

            if (projects.Count == 1)
                return projects[0];

            var names = string.Join(", ", projects.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{project}' matches {projects.Count} team projects: {names}. Please choose a more specific value for the -Project argument and try again");
        }

        public IEnumerable<Project> GetProjects(object project, object collection, object server, object credential)
        {
            while (true)
            {
                switch (project)
                {
                    case PSObject pso:
                        {
                            project = pso.BaseObject;
                            continue;
                        }
                    case Project p:
                        {
                            yield return p;
                            break;
                        }
                    case Uri u:
                        {
                            foreach (var p in GetProjectByUrl(u, collection, server, credential))
                            {
                                yield return p;
                            }
                            break;
                        }
                    case string s when Uri.IsWellFormedUriString(s, UriKind.Absolute):
                        {
                            foreach (var p in GetProjectByUrl(new Uri(s), collection, server, credential))
                            {
                                yield return p;
                            }
                            break;
                        }
                    case string s when !string.IsNullOrWhiteSpace(s):
                        {
                            foreach (var p in GetProjectByName(s, collection, server, credential))
                            {
                                yield return p;
                            }
                            break;
                        }
                    case null when CurrentConnections.TeamProject != null:
                        {
                            yield return CurrentConnections.TeamProject;
                            break;
                        }
                    default:
                        {
                            throw new PSArgumentException("No TFS team project information available. Either supply a valid -Project argument or use Connect-TfsTeamProject prior to invoking this cmdlet.", nameof(Project));
                        }
                }
                break;
            }
        }

        private IEnumerable<Project> GetProjectByUrl(Uri uri, object collection, object server, object credential)
        {
            var tpc = GetCollection(collection, server, credential);
            var css = tpc.GetService<ICommonStructureService>();
            var projInfo = css.GetProject(uri.AbsoluteUri);
            var projectName = projInfo.Name;

            return GetProjectByName(projectName, tpc, server, credential);
        }

        private IEnumerable<Project> GetProjectByName(string name, object collection, object server, object credential)
        {
            var tpc = GetCollection(collection, server, credential);
            var css = tpc.GetService<ICommonStructureService>();
            var pattern = new WildcardPattern(name);
            var projectInfos = css.ListAllProjects().Where(o => o.Status == ProjectState.WellFormed && pattern.IsMatch(o.Name));
            var store = tpc.GetService<WorkItemStore>();

            foreach (var pi in projectInfos)
            {
                yield return store.Projects[pi.Name];
            }
        }
    }
}