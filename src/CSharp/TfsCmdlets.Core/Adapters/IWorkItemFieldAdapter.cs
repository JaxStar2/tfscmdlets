using System;
using System.Collections.Generic;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface IWorkItemFieldAdapter: IAdapter
    {
        object Value { get; set; }
        string ReferenceName { get; }
        bool IsChangedInRevision { get; }
        string Name { get; }
        object OldValue { get; }
        IWorkItemRevisionAdapterCollection Revisions { get; }
    }
}
