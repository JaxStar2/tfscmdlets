using System;
using System.Collections.Generic;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface IWorkspaceAdapter: IAdapter
    {
        string Name { get; }
    }
}
