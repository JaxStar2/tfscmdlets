using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.Connection
{
    /*
    <#
    .SYNOPSIS
    Connects to a team project.

    .DESCRIPTION
    The Connect-TfsTeamProject cmdlet "connects" (initializes a Microsoft.TeamFoundation.WorkItemTracking.Client.Project object) to a TFS Team Project. That connection is subsequently kept in a global variable to be later reused until it's closed by a call to Disconnect-TfsTeamProject.
    Cmdlets in the TfsCmdlets module that require a team project object to be provided via their -Project argument in order to access a TFS project will use the connection opened by this cmdlet as their "default project". In other words, TFS cmdlets (e.g. New-TfsArea) that have a -Project argument will use the connection provided by Connect-TfsTeamProject by default.

    .PARAMETER Project
    ${HelpParam_Project}

    .PARAMETER Collection
    ${HelpParam_Collection}

    .PARAMETER Credential
    ${HelpParam_TfsCredential}

    .PARAMETER Interactive
    ${HelpParam_Interactive}

    .PARAMETER Passthru
    ${HelpParam_Passthru}

    .INPUTS
    Microsoft.TeamFoundation.WorkItemTracking.Client.Project
    System.String

    .EXAMPLE
    Connect-TfsTeamProject -Project FabrikamFiber
    Connects to a project called FabrikamFiber in the current team project collection (as specified in a previous call to Connect-TfsTeamProjectCollection)

    .EXAMPLE
    Connect-TfsTeamProject -Project FabrikamFiber -Collection http://vsalm:8080/tfs/FabrikamFiberCollection
    Connects to a project called FabrikamFiber in the team project collection specified in the given URL
    #>
    */
    [Cmdlet(VerbsCommunications.Connect, "TeamProject", DefaultParameterSetName = "Explicit credentials")]
    [OutputType("Microsoft.TeamFoundation.WorkItemTracking.Client.Project,Microsoft.TeamFoundation.WorkItemTracking.Client")]
    public class ConnectTeamProject : ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            if (Interactive.IsPresent)
            {
                Credential = CredentialService.GetCredential(true);
            }

            var project = GetProject();
            CurrentConnectionService.TeamProject = project;

            if (Passthru)
            {
                WriteObject(project);
            }
        }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public override object Project { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        [Parameter]
        public override object Server { get; set; }

        [Parameter(ParameterSetName = "Explicit credentials")]
        public override object Credential { get; set; }

        [Parameter(ParameterSetName = "Prompt for credentials", Mandatory = true)]
        public SwitchParameter Interactive { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }
    }
}
