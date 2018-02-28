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
    [Cmdlet(VerbsCommon.Add, "GroupMember")]
    public class AddGroupMember: ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var group = IdentityManagementService.GetGroup(Group.ToString(), Scope, Project, Collection, Server, Credential);
        }

        [Parameter(Mandatory = true, Position = 0)]
        [SupportsWildcards]
        public object Group { get; set; } = "*";

        [Parameter]
        public GroupScope Scope { get; set; }

        [Parameter(Position = 1)]
        [SupportsWildcards]
        public string Member { get; set; } = "*";

        [Parameter]
        public override object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        [Import(typeof(IIdentityManagementService))]
        private IIdentityManagementService IdentityManagementService { get; set; }
    }
}
