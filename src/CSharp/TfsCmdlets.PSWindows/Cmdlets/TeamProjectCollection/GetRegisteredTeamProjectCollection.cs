using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using TfsCmdlets.Core.Services;

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
    [OutputType("Microsoft.TeamFoundation.Client.RegisteredProjectCollection")]
    public class GetRegisteredTeamProjectCollection : Cmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        [SupportsWildcards]
        public string Name { get; set; } = "*";

        protected override void ProcessRecord()
        {
            WriteObject(RegisteredConnectionService.GetRegisteredProjectCollections(Name), true);
        }

        [Import(typeof(IRegisteredConnectionService))]
        private IRegisteredConnectionService RegisteredConnectionService { get; set; }
    }
}