using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.Security
{
    [Cmdlet(VerbsCommon.Get, "Group")]
    [OutputType("Microsoft.TeamFoundation.Framework.Client.TeamFoundationIdentity,Microsoft.TeamFoundation.Client")]
    public class GetGroup: ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(IdentityManagementService.GetGroups(Group.ToString(), Scope, Project, Collection, Server, Credential), true);
        }

        [Parameter(Position = 0)]
        [SupportsWildcards]
        public object Group { get; set; } = "*";

        [Parameter]
        public GroupScope Scope { get; set; }

        [Parameter]
        public override object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        [Import(typeof(IIdentityManagementService))]
        private IIdentityManagementService IdentityManagementService { get; set; }
    }
}
