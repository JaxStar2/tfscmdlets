using System;
using System.Collections.Generic;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface IWorkItemRevisionAdapter: IAdapter
    {
        IWorkItemFieldAdapterCollection Fields { get; }
    }
}
