using System;

namespace TfsCmdlets.Core.Adapters
{
    public interface IWorkItemQueryItemAdapter: IAdapter
    {
        string Name { get; }
        Guid Id { get; }
        string Path { get; }
    }
}
