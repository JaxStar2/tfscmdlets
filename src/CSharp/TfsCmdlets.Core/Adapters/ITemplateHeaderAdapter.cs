using System;
using System.Collections.Generic;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface ITemplateHeaderAdapter: IAdapter
    {
        string Name { get; }
        string Description { get; }
        int TemplateId { get; }
        string Metadata { get; }
    }
}
