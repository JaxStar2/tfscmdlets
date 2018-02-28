using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.VisualStudio.Services.Common;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(ITeamProjectCollectionService))]
    public sealed class TeamProjectCollectionService : ITeamProjectCollectionService
    {
        public ITfsTeamProjectCollectionAdapter GetCollection(object collection, object server, object credential, bool ensureAuthenticated)
        {
            var collections = GetCollections(collection, server, credential).ToList();

            if (collections.Count == 0)
                throw new Exception($"Invalid team project collection name '{collection}'");

            if (collections.Count == 1)
                return collections[0];

            var names = string.Join(", ", collections.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{collection}' matches {collections.Count} team project collections: {names}. Please choose a more specific value for the -Collection argument and try again");
        }

        public IEnumerable<ITfsTeamProjectCollectionAdapter> GetCollections(object collection, object server, object credential)
        {
            var cred = (VssCredentials) CredentialService.GetCredential(credential).Instance;

            while (true)
            {
                switch (collection)
                {
                    case PSObject pso:
                        {
                            collection = pso.BaseObject;
                            continue;
                        }
                    case TfsTeamProjectCollection tpc:
                        {
                            yield return new TfsTeamProjectCollectionAdapter(tpc);
                            break;
                        }
                    case Uri u:
                        {
                            yield return new TfsTeamProjectCollectionAdapter(new TfsTeamProjectCollection(u, cred));
                            break;
                        }
                    case string s when Uri.IsWellFormedUriString(s, UriKind.Absolute):
                        {
                            yield return new TfsTeamProjectCollectionAdapter(new TfsTeamProjectCollection(new Uri(s), cred));
                            break;
                        }
                    case string s when CurrentConnectionService.ConfigurationServer != null && !string.IsNullOrWhiteSpace(s):
                        {
                            var configServer = (TfsConfigurationServer) CurrentConnectionService.ConfigurationServer.Instance;
                            var filter = new[] { CatalogResourceTypes.ProjectCollection };
                            var collections = configServer.CatalogNode.QueryChildren(filter, false, CatalogQueryOptions.None);
                            var result = collections.Select(o => o.Resource).Where(o => o.DisplayName.IsLike(s)).ToList();

                            if (result.Count == 0)
                            {
                                throw new PSArgumentException($"Invalid or non-existent Team Project Collection(s): {s}", "Collection");
                            }

                            foreach (var resource in result)
                            {
                                yield return new TfsTeamProjectCollectionAdapter(configServer.GetTeamProjectCollection(new Guid(resource.Properties["InstanceId"])));
                            }

                            break;
                        }
                    case string s when !string.IsNullOrWhiteSpace(s):
                        {
                            var registeredCollection = RegisteredConnectionService.GetRegisteredProjectCollections(s);

                            foreach (var tpc in registeredCollection)
                            {
                                yield return new TfsTeamProjectCollectionAdapter(new TfsTeamProjectCollection(tpc.Uri, cred));
                            }

                            break;
                        }
                    case null when CurrentConnectionService.TeamProjectCollection != null:
                        {
                            yield return CurrentConnectionService.TeamProjectCollection;
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

        [Import(typeof(IRegisteredConnectionService))]
        private IRegisteredConnectionService RegisteredConnectionService { get; set; }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }

        [Import(typeof(IConfigurationServerService))]
        private IConfigurationServerService ConfigurationServerService { get; set; }

        [Import(typeof(ICredentialService))]
        private ICredentialService CredentialService { get; set; }
    }
}