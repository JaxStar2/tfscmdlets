using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.Cmdlets.TeamProjectCollection
{
    /*
    <#
    .SYNOPSIS
        Gets one or more Team Project Collection addresses registered in the current computer.

    .PARAMETER Name
        Specifies the name of a registered collection. When omitted, all registered collections are returned. Wildcards are permitted.

    .INPUTS
        System.String
    #>
    */
    [Cmdlet(VerbsCommon.Get, "RegisteredTeamProjectCollection")]
    [OutputType(typeof(RegisteredProjectCollection))]
    public class GetRegisteredTeamProjectCollection : Cmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        [SupportsWildcards]
        public string Name { get; set; } = "*";

        protected override void ProcessRecord()
        {
            var pattern = new WildcardPattern(Name);

            foreach (var tpc in RegisteredTfsConnections.GetProjectCollections()
                .Where(o => pattern.IsMatch(o.DisplayName)))
            {
                WriteObject(tpc);
            }
        }

        internal static IEnumerable<RegisteredProjectCollection> Get(string name)
        {
            var pattern = new WildcardPattern(name);

            foreach (var tpc in RegisteredTfsConnections.GetProjectCollections()
                .Where(o => pattern.IsMatch(o.DisplayName)))
            {
                yield return tpc;
            }
        }
    }
}