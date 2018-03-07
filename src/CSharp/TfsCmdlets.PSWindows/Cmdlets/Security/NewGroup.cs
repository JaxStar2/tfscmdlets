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
    [Cmdlet(VerbsCommon.New, "Group", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType("Microsoft.TeamFoundation.Framework.Client.TeamFoundationIdentity,Microsoft.TeamFoundation.Client")]
    public class NewGroup: ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var group = IdentityManagementService.CreateGroup(Group, Description, Scope, Project, Collection, Server, Credential);

            if (Passthru)
                WriteObject(group);
        }

        [Parameter(Position = 0, Mandatory = true)]
        public string Group { get; set; }

        [Parameter()]
        public string Description { get; set; }

        [Parameter(Mandatory = true)]
        public GroupScope Scope { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Parameter]
        public override object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        [Import(typeof(IIdentityManagementService))]
        private IIdentityManagementService IdentityManagementService { get; set; }
    }
}
