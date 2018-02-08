using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Services;

namespace TfsCmdlets.Cmdlets
{
    public abstract class CollectionLevelCmdlet : ServerLevelCmdlet
    {
        protected TfsTeamProjectCollection GetCollection()
        {
            return GetCollection(Collection, Server, Credential);
        }

        protected IEnumerable<TfsTeamProjectCollection> GetCollections()
        {
            return GetCollections(Collection, Server, Credential);
        }

        protected TfsTeamProjectCollection GetCollection(object collection, object server, object credential)
        {
            return TeamProjectCollectionService.GetCollection(collection, server, credential);
        }

        protected IEnumerable<TfsTeamProjectCollection> GetCollections(object collection, object server, object credential)
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
