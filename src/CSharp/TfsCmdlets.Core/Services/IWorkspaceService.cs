using System;
using System.Collections.Generic;
using System.Text;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IWorkspaceService
    {
        IWorkspaceAdapter GetWorkspace(object workspace, string ownerName, string computerName, object collection, object server, object credential);
        IEnumerable<IWorkspaceAdapter> GetWorkspaces(object workspace, string ownerName, string computerName, object collection, object server, object credential);
    }
}
