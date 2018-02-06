using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;

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
            return Provider.GetCollection(collection, server, credential);
        }

        protected IEnumerable<TfsTeamProjectCollection> GetCollections(object collection, object server, object credential)
        {
            return Provider.GetCollections(collection, server, credential);
        }

        public abstract object Collection { get; set; }

        [Parameter]
        public override object Server { get; set; }

        [Parameter]
        public override object Credential { get; set; }
    }
}
