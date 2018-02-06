using System;

namespace TfsCmdlets
{
    public enum NodeScope
    {
        Area,
        Iteration
    }

    [Flags]
    public enum WorkItemQueryScope
    {
        Personal = 1,
        Shared = 2,
        Both = 4
    }
}
