using System;
using System.Threading.Tasks;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Core.Adapters
{
    public interface ITfsTeamProjectCollectionAdapter: IAdapter
    {
        string Name { get; }
        Uri Uri { get; }
    }
}
