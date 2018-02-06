using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Server;
using TfsCmdlets.Providers;

namespace TfsCmdlets.Cmdlets.ProcessTemplate
{

    [Cmdlet(VerbsCommon.Get, "ProcessTemplate")]
    [OutputType(typeof(TemplateHeader))]
    public class GetProcessTemplate : CollectionLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(ProcessTemplateProvider.GetTemplates(Name, Collection, Server, Credential), true);
        }

        [Parameter(Position = 0)]
        [SupportsWildcards]
        public string Name { get; set; } = "*";

        [Parameter(ValueFromPipeline = true)]
        public override object Collection { get; set; }

        [Import(typeof(IProcessTemplateProvider))]
        private IProcessTemplateProvider ProcessTemplateProvider { get; set; }

    }
}