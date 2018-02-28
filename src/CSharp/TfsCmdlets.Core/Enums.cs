using System;

namespace TfsCmdlets.Core
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

    public enum CopyWorkItemPassthruOptions
    {
        Original,
        Copy,
        None
    }

    [Flags]
    public enum WorkItemCopyFlags
    {
        None = 0,
        CopyFiles = 1,
        CopyLinks = 2,
    }

    [Flags]
    public enum SourceControlType
    {
        Default = 0,
        TFVC = 1,
        Git = 2,
    }

    public enum Ensure
    {
        Absent,
        Present
    }

    public enum ServiceHostStatus
    {
        Started,
        Stopped
    }

    public enum ProjectState
    {
        Unchanged = -2,
        All = -1,
        New = 0,
        WellFormed = 1,
        Deleting = 2,
        CreatePending = 3,
        Deleted = 4,
    }

    public enum GroupScope
    {
        Server,
        Collection,
        Project
    }
}
