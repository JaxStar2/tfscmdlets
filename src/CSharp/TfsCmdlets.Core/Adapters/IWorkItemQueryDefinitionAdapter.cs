using System;
using System.Collections.Generic;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface IWorkItemQueryDefinitionAdapter: IWorkItemQueryItemAdapter
    {
        string QueryText { get; }
    }
}
