using System;
using System.Collections.Generic;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface ITeamFoundationTeamAdapter: IAdapter
    {
        string Name { get; }
        string Description { get; }
        ITeamFoundationIdentityAdapter IdentityDescriptor { get; }
    }
}
