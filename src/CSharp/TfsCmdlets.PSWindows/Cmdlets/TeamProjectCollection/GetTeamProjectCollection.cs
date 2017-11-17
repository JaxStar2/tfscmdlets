using Microsoft.TeamFoundation.Core.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using TfsCmdlets.Cmdlets.ConfigurationServer;
using TfsCmdlets.Cmdlets.Connection;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    [Cmdlet(VerbsCommon.Get, "TeamProjectCollection", DefaultParameterSetName = "Get by collection")]
    [OutputType(typeof(TfsTeamProjectCollection))]
    public class GetTeamProjectCollection : Cmdlet
    {
        [Parameter(Position = 0, ParameterSetName = "Get by collection")]
        [SupportsWildcards()]
        public object Collection { get; set; } = "*";

        [Parameter(ValueFromPipeline = true, ParameterSetName = "Get by collection")]
        public object Server { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Get current")]
        public SwitchParameter Current { get; set; }

        [Parameter(ParameterSetName = "Get by collection")]
        public object Credential { get; set; }

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

            foreach (var tpc in Get(Collection, Server, Credential))
            {
                WriteObject(tpc);
            }
        }

        protected static IEnumerable<TfsTeamProjectCollection> Get(object collection, object server = null, object credential = null)
        {
            var cred = GetCredential.Get(credential);

            switch (collection)
            {
                case PSObject pso:
                    {
                        foreach (var tpc in Get(pso.BaseObject))
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
                case string s when ((server != null || CurrentConnections.ConfigurationServer != null) && !string.IsNullOrWhiteSpace(s)):
                    {
                        var configServer = GetConfigurationServer.Get(server, credential).First();
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