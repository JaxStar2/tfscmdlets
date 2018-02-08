using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Client.Fakes;
using TfsCmdlets.Services;

namespace TfsCmdlets.UnitTests.Stubs.Services
{
    [Export(typeof(ITeamProjectCollectionService))]
    internal sealed class TeamProjectCollectionServiceStub : ITeamProjectCollectionService
    {
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
            return _collections.ToList();
        }

        private readonly List<TfsTeamProjectCollection> _collections = new List<TfsTeamProjectCollection>
        {
            new ShimTfsTeamProjectCollection
            {
                NameGet = () => "FooCollection",
                DisplayNameGet = () => "http://foo:8080/tfs/FooCollection"
            }.Instance,
            new ShimTfsTeamProjectCollection
            {
                NameGet = () => "BarCollection",
                DisplayNameGet = () => "http://bar:8080/tfs/BarCollection",
            }.Instance,
            new ShimTfsTeamProjectCollection
            {
                NameGet = () => "BazCollection",
                DisplayNameGet = () => "http://baz:8080/tfs/BazCollection",
            }.Instance
        };
    }
}