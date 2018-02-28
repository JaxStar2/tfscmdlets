using System;
using System.Collections.Generic;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface IWorkItemFieldAdapterCollection: IEnumerable<IWorkItemFieldAdapter>
    {
        IWorkItemFieldAdapter this[string referenceName] { get; }
    }
}
