using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Management.Automation;
using System.Threading;
using System.Xml;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using TfsCmdlets.Services;

namespace TfsCmdlets.Cmdlets.TeamProject
{
    [Cmdlet(VerbsCommon.New, "TeamProject", ConfirmImpact = ConfirmImpact.Medium,
        SupportsShouldProcess = true)]
    [OutputType(typeof(Microsoft.TeamFoundation.WorkItemTracking.Client.Project))]
    public class NewTeamProject : ProjectLevelCmdlet
    {
        private const int DELAY_MS = 5000;

        [Parameter(Position = 0, Mandatory = true)]
        [Alias("Name")]
        public override object Project { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public TeamFoundationSourceControlType SourceControl { get; set; }

        [Parameter]
        public object ProcessTemplate { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Import(typeof(IProcessTemplateService))]
        private IProcessTemplateService ProcessTemplateService { get; set; }

        protected override void ProcessRecord()
        {
            if (!(Project is string project))
            {
                throw new PSArgumentException($"Invalid team project '{Project}'");
            }

            if (!ShouldProcess(project, "Create team project")) return;

            if (SourceControl == TeamFoundationSourceControlType.Default)
            {
                SourceControl = TeamFoundationSourceControlType.Git;
            }

            var tpc = GetCollection();
            var template = ProcessTemplateService.GetTemplate(ProcessTemplate, tpc, Server, Credential);
            var xml = new XmlDocument();
            xml.LoadXml(template.Metadata);
            var templateTypeId = xml.SelectSingleNode("//version/@type")?.Value;
            var client = tpc.GetClient<ProjectHttpClient>();

            var tpInfo = new Microsoft.TeamFoundation.Core.WebApi.TeamProject
            {
                Name = project,
                Description = Description,
                Capabilities = new Dictionary<string, Dictionary<string, string>>
                {
                    {
                        "versioncontrol", new Dictionary<string, string>
                        {
                            {"sourceControlType", SourceControl.ToString()}
                        }
                    },
                    {
                        "processTemplate", new Dictionary<string, string>
                        {
                            {"templateTypeId", templateTypeId}
                        }
                    }
                }
            };

            var token = client.QueueCreateProject(tpInfo).Result;

            if (token == null)
            {
                throw new Exception(
                    $"Error queueing team project creation: {client.LastResponseContext.Exception.Message}");
            }

            var operationsClient = tpc.GetClient<Microsoft.VisualStudio.Services.Operations.OperationsHttpClient>();
            var opsToken = operationsClient.GetOperation(token.Id).Result;

            while (
                opsToken.Status != Microsoft.VisualStudio.Services.Operations.OperationStatus.Succeeded &&
                opsToken.Status != Microsoft.VisualStudio.Services.Operations.OperationStatus.Failed &&
                opsToken.Status != Microsoft.VisualStudio.Services.Operations.OperationStatus.Cancelled)
            {
                Thread.Sleep(DELAY_MS);
                opsToken = operationsClient.GetOperation(token.Id).Result;
            }

            if (opsToken.Status != Microsoft.VisualStudio.Services.Operations.OperationStatus.Succeeded)
            {
                throw new Exception(
                    $"Error creating team project Project: {operationsClient.LastResponseContext.Exception.Message}");
            }

            var wiStore = tpc.GetService<Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore>();
            wiStore.RefreshCache();

            WriteObject(GetProject());
        }
    }
}