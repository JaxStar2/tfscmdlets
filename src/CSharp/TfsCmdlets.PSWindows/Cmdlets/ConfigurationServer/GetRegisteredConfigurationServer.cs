using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Cmdlets.ConfigurationServer
{
    [Cmdlet(verbName:VerbsCommon.Get, nounName:"RegisteredConfigurationServer")]
    [OutputType(typeof(Microsoft.TeamFoundation.Client.RegisteredConfigurationServer))]
    public class GetRegisteredConfigurationServer: Cmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        public string Name { get; set; } = "*";

        protected override void ProcessRecord()
        {
            foreach (var registeredServer in Get(Name))
            {
                WriteObject(registeredServer);
            }
        }

        public static IEnumerable<RegisteredConfigurationServer> Get(string serverName)
        {
            var pattern = serverName.Equals("localhost", StringComparison.OrdinalIgnoreCase) || serverName.Equals(".")
                ? Environment.MachineName
                : serverName;

            foreach (var s in RegisteredTfsConnections.GetConfigurationServers().Where(o => o.Name.IsLike(pattern)))
            {
                yield return s;
            }
        }
    }
}
