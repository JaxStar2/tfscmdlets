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
    [Export(typeof(ICollectionProvider))]
    internal sealed class CollectionProviderImpl : ICollectionProvider
    {
        [Import(typeof(IServerProvider))]
        private IServerProvider ServerProvider { get; set; }

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
    }
}