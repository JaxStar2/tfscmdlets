using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Cmdlets.ConfigurationServer;
using TfsCmdlets.Cmdlets.Connection;
using TfsCmdlets.Cmdlets.TeamProjectCollection;

namespace TfsCmdlets.Cmdlets
{
    public abstract class CollectionLevelCmdlet : Cmdlet
    {
        [Parameter(ValueFromPipeline = true)]
        public virtual object Collection { get; set; }

        [Parameter()]
        public virtual object Credential { get; set; }

        protected TfsTeamProjectCollection GetCollection()
        {
            return GetCollection(Collection, Credential);
        }

        internal static TfsTeamProjectCollection GetCollection(object collection, object credential = null)
        {
            var collections = GetCollections(collection, credential).ToList();

            if (collections.Count == 0)
            {
                throw new PSArgumentException($"Invalid team project collection name '{collection}'", nameof(Collection));
            }
            else if (collections.Count > 1)
            {
                var names = string.Join(", ", collections.Select(o => o.Name).ToArray());
                throw new PSArgumentException($"Ambiguous name '{collection}' matches {collections.Count} team project collections: {names}. Please choose a more specific value for the {nameof(Collection)} argument and try again", nameof(Collection));
            }

            return collections[0];
        }

        internal static IEnumerable<TfsTeamProjectCollection> GetCollections(object collection, object credential = null)
        {
            var cred = GetCredential.Get(credential);

            switch (collection)
            {
                case PSObject pso:
                    {
                        foreach (var tpc in GetCollections(pso.BaseObject))
                        {
                            yield return tpc;
                        }
                        break;
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
                case string s when (Uri.IsWellFormedUriString(s, UriKind.Absolute)):
                    {
                        yield return new TfsTeamProjectCollection(new Uri(s), cred);
                        break;
                    }
                case string s when ((CurrentConnections.ConfigurationServer != null) && !string.IsNullOrWhiteSpace(s)):
                    {
                        var configServer = GetConfigurationServer.Get().First();
                        var pattern = new WildcardPattern(s);
                        var filter = new[] { CatalogResourceTypes.ProjectCollection };
                        var collections = configServer.CatalogNode.QueryChildren(filter, false,
                            CatalogQueryOptions.None);
                        var result = collections.Select(o => o.Resource).Where(o => pattern.IsMatch(o.DisplayName))
                            .ToList();

                        if (result.Count == 0)
                        {
                            throw new PSArgumentException(
                                $"Invalid or non-existent Team Project Collection(s): {s}",
                                "Collection");
                        }

                        foreach (var resource in result)
                        {
                            yield return configServer.GetTeamProjectCollection(
                                new Guid(resource.Properties["InstanceId"]));
                        }

                        break;
                    }
                case string s when (!string.IsNullOrWhiteSpace(s)):
                    {
                        var registeredCollection = GetRegisteredTeamProjectCollection.Get(s);

                        foreach (var tpc in registeredCollection)
                        {
                            yield return new TfsTeamProjectCollection(tpc.Uri, cred);
                        }

                        break;
                    }
                case null when (CurrentConnections.TeamProjectCollection != null):
                    {
                        yield return CurrentConnections.TeamProjectCollection;
                        break;
                    }
                default:
                    {
                        throw new PSArgumentException(
                            "No TFS connection information available. Either supply a valid -Collection argument or use Connect-TfsTeamProjectCollection prior to invoking this cmdlet.");
                    }
            }
        }
    }
}
