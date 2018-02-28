using System;
using System.Collections.Generic;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface IProjectAdapter: IAdapter
    {
        string Name { get; }
        IDictionary<string, IWorkItemTypeAdapter> WorkItemTypes { get; }
        Guid Guid { get; }
        Uri Uri { get; }
    }
}
