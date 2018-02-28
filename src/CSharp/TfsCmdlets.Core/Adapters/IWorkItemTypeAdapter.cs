using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TfsCmdlets.Core.Adapters
{
    public interface IWorkItemTypeAdapter: IAdapter
    {
        IProjectAdapter Project { get; }
        string Name { get; }
    }
}
