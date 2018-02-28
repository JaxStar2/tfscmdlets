using System;
using System.Collections.Generic;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface IBuildDefinitionAdapter: IAdapter
    {
        string Name { get; }
    }
}
