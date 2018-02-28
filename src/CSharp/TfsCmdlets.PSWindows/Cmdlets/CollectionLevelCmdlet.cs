using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets
{
    public abstract class CollectionLevelCmdlet : ServerLevelCmdlet
    {
        protected ITfsTeamProjectCollectionAdapter GetCollection(bool ensureAuthenticated = false)
        {
            return GetCollection(Collection, Server, Credential, ensureAuthenticated);
        }

        protected IEnumerable<ITfsTeamProjectCollectionAdapter> GetCollections()
        {
            return GetCollections(Collection, Server, Credential);
        }

        protected ITfsTeamProjectCollectionAdapter GetCollection(object collection, object server, object credential,
            bool ensureAuthenticated = false)
        {
            return TeamProjectCollectionService.GetCollection(collection, server, credential, ensureAuthenticated);
        }

        protected IEnumerable<ITfsTeamProjectCollectionAdapter> GetCollections(object collection, object server, object credential)
        {
            return TeamProjectCollectionService.GetCollections(collection, server, credential);
        }

        public abstract object Collection { get; set; }

        [Parameter]
        public override object Server { get; set; }

        [Parameter]
        public override object Credential { get; set; }

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService TeamProjectCollectionService { get; set; }
    }
}
