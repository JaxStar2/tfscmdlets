using System;

namespace TfsCmdlets.Core.Adapters
{
    public interface INodeInfoAdapter: IAdapter
    {
        string Uri { get; }
        string Path { get; }
    }
}