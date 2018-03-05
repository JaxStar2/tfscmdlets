using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.Build
{
    public abstract class BuildCmdletBase: ProjectLevelCmdlet
    {
        protected IBuildDefinitionAdapter GetBuildDefinition()
        {
            return BuildDefinitionService.GetBuildDefinition(Definition, Project, Collection, Server, Credential);
        }

        protected IEnumerable<IBuildDefinitionAdapter> GetBuildDefinitions()
        {
            return BuildDefinitionService.GetBuildDefinitions(Definition, Project, Collection, Server, Credential);
        }

        public abstract object Definition { get; set; }

        [Import(typeof(IBuildDefinitionService))]
        protected IBuildDefinitionService BuildDefinitionService { get; set; }
    }
}
