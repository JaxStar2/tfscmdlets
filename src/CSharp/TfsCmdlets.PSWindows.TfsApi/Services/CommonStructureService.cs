using System;
using System.ComponentModel.Composition;
using System.Xml;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(ICommonStructureService))]
    public class CommonStructureService: ICommonStructureService
    {
        public string CreateNode(string nodeName, string parentNodeUri, object collection, object server, object credential)
        {
            var css = GetCssService(collection, server, credential);

            return css.CreateNode(nodeName, parentNodeUri);
        }

        public string CreateNode(string nodeName, string parentNodeUri, DateTime? startDate, DateTime? finishDate, object collection, object server, object credential)
        {
            var css = GetCssService(collection, server, credential);

            return css.CreateNode(nodeName, parentNodeUri, startDate, finishDate);
        }

        public INodeInfoAdapter GetNodeFromPath(string nodePath, object collection, object server, object credential)
        {
            var css = GetCssService(collection, server, credential);

            return new NodeInfoAdapter(css.GetNodeFromPath(nodePath));
        }

        public INodeInfoAdapter GetNode(string nodeUri, object collection, object server, object credential)
        {
            var css = GetCssService(collection, server, credential);

            return new NodeInfoAdapter(css.GetNode(nodeUri));
        }

        public XmlElement GetNodesXml(string[] uris, bool childNodes, object collection, object server,
            object credential)
        {
            var css = GetCssService(collection, server, credential);

            return css.GetNodesXml(uris, childNodes);
        }

        public void MoveBranch(string nodeUri, string newParentNodeUri, object collection, object server,
            object credential)
        {
            var css = GetCssService(collection, server, credential);

            css.MoveBranch(nodeUri, newParentNodeUri);
        }

        public void DeleteBranches(string[] nodeUris, string reclassifyUri, object collection, object server, object credential)
        {
            var css = GetCssService(collection, server, credential);

            css.DeleteBranches(nodeUris, reclassifyUri);
        }

        public void RenameNode(string nodeUri, string newNodeName, object collection, object server, object credential)
        {
            var css = GetCssService(collection, server, credential);

            css.RenameNode(nodeUri, newNodeName);
        }

        public void ReorderNode(string nodeUri, int moveBy, object collection, object server, object credential)
        {
            var css = GetCssService(collection, server, credential);

            css.ReorderNode(nodeUri, moveBy);
        }

        private Microsoft.TeamFoundation.Server.ICommonStructureService4 GetCssService(object collection, object server, object credential)
        {
            var tpc = (TfsTeamProjectCollection) CollectionService.GetCollection(collection, server, credential).Instance;

            return tpc.GetService<Microsoft.TeamFoundation.Server.ICommonStructureService4>();
        }

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService CollectionService { get; set; }
    }
}
