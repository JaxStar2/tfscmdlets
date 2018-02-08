using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Server;
using TfsCmdlets.Services;

namespace TfsCmdlets.Cmdlets.ProcessTemplate
{

    [Cmdlet(VerbsCommon.Get, "ProcessTemplate")]
    [OutputType(typeof(TemplateHeader))]
    public class GetProcessTemplate : CollectionLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(ProcessTemplateService.GetTemplates(Name, Collection, Server, Credential), true);
        }

        [Parameter(Position = 0)]
        [SupportsWildcards]
        public string Name { get; set; } = "*";

        [Parameter(ValueFromPipeline = true)]
        public override object Collection { get; set; }

        [Import(typeof(IProcessTemplateService))]
        private IProcessTemplateService ProcessTemplateService { get; set; }

    }
}