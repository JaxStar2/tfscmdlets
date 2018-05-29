using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.ReleaseManagement
{
    public abstract class ReleaseCmdletBase : ProjectLevelCmdlet
    {
        protected IReleaseDefinitionAdapter GetReleaseDefinition()
        {
            return ReleaseDefinitionService.GetReleaseDefinition(Definition, Project, Collection, Server, Credential);
        }

        protected IEnumerable<IReleaseDefinitionAdapter> GetReleaseDefinitions()
        {
            return ReleaseDefinitionService.GetReleaseDefinitions(Definition, Project, Collection, Server, Credential);
        }

        public abstract object Definition { get; set; }

        [Import(typeof(IReleaseDefinitionService))]
        protected IReleaseDefinitionService ReleaseDefinitionService { get; set; }
    }
}
