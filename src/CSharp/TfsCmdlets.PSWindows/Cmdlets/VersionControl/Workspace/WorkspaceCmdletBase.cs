using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.VersionControl.Workspace
{
    public abstract class WorkspaceCmdletBase: CollectionLevelCmdlet
    {
        public abstract object Workspace { get; set; }

        [Import(typeof(IWorkspaceService))]
        protected IWorkspaceService WorkspaceService { get; set; }
    }
}
