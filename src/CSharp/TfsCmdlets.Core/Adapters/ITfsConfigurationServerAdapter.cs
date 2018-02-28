using System;
using System.Collections.Generic;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface ITfsConfigurationServerAdapter: IAdapter
    {
        string Name { get; }
        Uri Uri { get; }
    }
}
