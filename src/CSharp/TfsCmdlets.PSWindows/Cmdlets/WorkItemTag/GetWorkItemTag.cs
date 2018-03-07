using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.WorkItemTag
{
    [Cmdlet(VerbsCommon.Get, "WorkItemTag")]
    [OutputType("Microsoft.TeamFoundation.Core.WebApi.WebApiTagDefinition,Microsoft.TeamFoundation.Core.WebApi")]
    public class GetWorkItemTag: ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(TagService.GetTags(Tag, Project, Collection, Server, Credential), true);
        }

        [Parameter(Position = 0)]
        [SupportsWildcards]
        public object Tag { get; set; } = "*";

        [Parameter(ValueFromPipeline = true)]
        public override object Project { get; set; }

        [Import(typeof(IWorkItemTagService))]
        private IWorkItemTagService TagService { get; set; }
    }
}
