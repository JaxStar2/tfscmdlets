using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Server;

namespace TfsCmdlets.Cmdlets.ProcessTemplate
{
    [Cmdlet(VerbsCommon.Get, "ProcessTemplate")]
    [OutputType(typeof(TemplateHeader))]
    public class GetProcessTemplate : ProcessTemplateCmdletBase
    {
        protected override void ProcessRecord()
        {
            WriteObject(GetProcessTemplates(), true);
        }

        [Parameter(Position = 0)]
        [Alias("Name")]
        [SupportsWildcards]
        public override object ProcessTemplate { get; set; } = "*";

        [Parameter(ValueFromPipeline = true)]
        public override object Collection { get; set; }
    }
}