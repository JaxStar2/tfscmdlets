using System;
using System.Collections.Generic;
using System.Xml;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface ICommonStructureService
    {
        string CreateNode(string nodeName, string parentNodeUri, object collection, object server, object credential);
        string CreateNode(string nodeName, string parentNodeUri, DateTime? startDate, DateTime? finishDate, object collection, object server, object credential);
        INodeInfoAdapter GetNode(string nodeUri, object collection, object server, object credential);
        INodeInfoAdapter GetNodeFromPath(string path, object collection, object server, object credential);
        XmlElement GetNodesXml(string[] uris, bool childNodes, object collection, object server, object credential);
        void MoveBranch(string nodeUri, string newParentNodeUri, object collection, object server, object credential);
        void DeleteBranches(string[] nodeUris, string reclassifyUri, object collection, object server, object credential);
        void RenameNode(string nodeUri, string newName, object collection, object server, object credential);
        void ReorderNode(string nodeUri, int moveBy, object collection, object server, object credential);
    }

    public interface IProjectInfoAdapter
    {
        string Name { get; }
        ProjectState Status { get; }
    }
}