using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Cmdlets.Connection;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsCommon.Get, "TeamProjectCollection", DefaultParameterSetName = "Get by collection")]
    [OutputType(typeof(TfsTeamProjectCollection))]
    public class GetTeamProjectCollection : CollectionLevelCmdlet
    {
        [Parameter(Position = 0, ParameterSetName = "Get by collection")]
        [SupportsWildcards]
        public override object Collection { get; set; } = "*";

        [Parameter(ValueFromPipeline = true, ParameterSetName = "Get by collection")]
        public override object Server { get; set; }

        [Parameter(ParameterSetName = "Get by collection")]
        public override object Credential { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Get current")]
        public SwitchParameter Current { get; set; }

        protected override void ProcessRecord()
        {
            if (Current.IsPresent)
            {
                WriteObject(CurrentConnections.TeamProjectCollection);
                return;
            }

            if (Collection is TfsTeamProjectCollection)
            {
                WriteObject(Collection);
                return;
            }

            WriteObject(GetCollections(Collection, Server, Credential), true);
        }

        //protected static IEnumerable<TfsTeamProjectCollection> Get(object collection, object server = null, object credential = null)
        //{
        //    var cred = GetCredential.Get(credential);

        //    switch (collection)
        //    {
        //        case PSObject pso:
        //            {
        //                foreach (var tpc in Get(pso.BaseObject))
        //                {
        //                    yield return tpc;
        //                }
        //                break;
        //            }
        //        case TfsTeamProjectCollection tpc:
        //            {
        //                yield return tpc;
        //                break;
        //            }
        //        case Uri u:
        //            {
        //                yield return new TfsTeamProjectCollection(u, cred);
        //                break;
        //            }
        //        case string s when (Uri.IsWellFormedUriString(s, UriKind.Absolute)):
        //            {
        //                yield return new TfsTeamProjectCollection(new Uri(s), cred);
        //                break;
        //            }
        //        case string s when ((server != null || CurrentConnections.ConfigurationServer != null) && !string.IsNullOrWhiteSpace(s)):
        //            {
        //                var configServer = GetConfigurationServer.Get(server, credential).First();
        //                var pattern = new WildcardPattern(s);
        //                var filter = new[] { CatalogResourceTypes.ProjectCollection };
        //                var collections = configServer.CatalogNode.QueryChildren(filter, false,
        //                    CatalogQueryOptions.None);
        //                var result = collections.Select(o => o.Resource).Where(o => pattern.IsMatch(o.DisplayName))
        //                    .ToList();

        //                if (result.Count == 0)
        //                {
        //                    throw new PSArgumentException(
        //                        $"Invalid or non-existent Team Project Collection(s): {s}",
        //                        "Collection");
        //                }

        //                foreach (var resource in result)
        //                {
        //                    yield return configServer.GetTeamProjectCollection(
        //                        new Guid(resource.Properties["InstanceId"]));
        //                }

        //                break;
        //            }
        //        case string s when (!string.IsNullOrWhiteSpace(s)):
        //            {
        //                var registeredCollection = GetRegisteredTeamProjectCollection.Get(s);

        //                foreach (var tpc in registeredCollection)
        //                {
        //                    yield return new TfsTeamProjectCollection(tpc.Uri, cred);
        //                }

        //                break;
        //            }
        //        case null when (CurrentConnections.TeamProjectCollection != null):
        //            {
        //                yield return CurrentConnections.TeamProjectCollection;
        //                break;
        //            }
        //        default:
        //            {
        //                throw new PSArgumentException(
        //                    "No TFS connection information available. Either supply a valid -Collection argument or use Connect-TfsTeamProjectCollection prior to invoking this cmdlet.");
        //            }
        //    }
        //}
    }
}