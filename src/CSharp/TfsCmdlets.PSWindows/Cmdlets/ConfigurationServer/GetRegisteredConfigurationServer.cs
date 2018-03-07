using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.ConfigurationServer
{
    [Cmdlet(VerbsCommon.Get, "RegisteredConfigurationServer")]
    [OutputType("Microsoft.TeamFoundation.Client.RegisteredConfigurationServer,Microsoft.TeamFoundation.Client")]
    public class GetRegisteredConfigurationServer : BaseCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(RegisteredConnectionService.GetRegisteredConfigurationServers(Name), true);
        }

        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; } = "*";

        [Import(typeof(IRegisteredConnectionService))]
        private IRegisteredConnectionService RegisteredConnectionService { get; set; }
    }
}
