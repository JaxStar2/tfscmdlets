using System;
using System.Collections.Generic;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface IRegisteredConfigurationServerAdapter: IAdapter
    {
        Uri Uri { get; }
    }
}
