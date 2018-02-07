using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Providers;

namespace TfsCmdlets.Cmdlets.ConfigurationServer
{
    [Cmdlet(VerbsCommon.Get, "RegisteredConfigurationServer")]
    [OutputType(typeof(RegisteredConfigurationServer))]
    public class GetRegisteredConfigurationServer : BaseCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(RegisteredConnectionsProvider.GetRegisteredConfigurationServers(Name), true);
        }

        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; } = "*";

        [Import(typeof(IRegisteredConnectionsProvider))]
        private IRegisteredConnectionsProvider RegisteredConnectionsProvider { get; set; }
    }
}
