using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.Server;
using TfsCmdlets.Services;

namespace TfsCmdlets.UnitTests.Stubs.Services
{
    [Export(typeof(IProcessTemplateService))]
    internal class ProcessTemplateServiceImpl: IProcessTemplateService
    {
        public TemplateHeader GetTemplate(object name, object collection, object server, object credential)
        {
            var templates = GetTemplates(name, collection, server, credential).ToList();

            if (templates.Count == 0)
                throw new Exception($"Invalid process template name '{name}'");

            if (templates.Count == 1)
                return templates[0];

            var names = string.Join(", ", templates.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{name}' matches {templates.Count} team project collections: {names}. Please choose a more specific value for the Name argument and try again");
        }

        public IEnumerable<TemplateHeader> GetTemplates(object template, object collection, object server, object credential)
        {
            return new List<TemplateHeader>
            {
                new TemplateHeader
                {
                    Name = "Agile",
                    Description = "This template is flexible and will work great for most teams using Agile planning methods, including those practicing Scrum.",
                    Metadata = @"<metadata><name>Agile</name><description>This template is flexible and will work great for most teams using Agile planning methods, including those practicing Scrum.</description><version type=""adcc42ab-9882-485e-a3ed-7678f01f66bc"" major=""16"" minor=""1"" /><plugins><plugin name=""Microsoft.ProjectCreationWizard.Classification"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Reporting"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Groups"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.WorkItemTracking"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.VersionControl"" wizardPage=""true"" /><plugin name=""Microsoft.ProjectCreationWizard.TestManagement"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Build"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Lab"" wizardPage=""false"" /></plugins></metadata>",
                    Rank = 0,
                    State = "visible",
                    TemplateId = 536870913
                },
                new TemplateHeader
                {
                    Name = "Scrum",
                    Description = "This template is for teams who follow the Scrum framework.",
                    Metadata = @"<metadata><name>Scrum</name><description>This template is for teams who follow the Scrum framework.</description><version type=""6b724908-ef14-45cf-84f8-768b5384da45"" major=""16"" minor=""1"" /><plugins><plugin name=""Microsoft.ProjectCreationWizard.Classification"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Reporting"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Groups"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.WorkItemTracking"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.VersionControl"" wizardPage=""true"" /><plugin name=""Microsoft.ProjectCreationWizard.TestManagement"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Build"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Lab"" wizardPage=""false"" /></plugins></metadata>",
                    Rank = 1,
                    State = "visible",
                    TemplateId = 536870915
                },
                new TemplateHeader
                {
                    Name = "CMMI",
                    Description = "This template is for more formal projects requiring a framework for process improvement and an auditable record of decisions.",
                    Metadata = @"<metadata><name>CMMI</name><description>This template is for more formal projects requiring a framework for process improvement and an auditable record of decisions.</description><version type=""27450541-8e31-4150-9947-dc59f998fc01"" major=""16"" minor=""1"" /><plugins><plugin name=""Microsoft.ProjectCreationWizard.Classification"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Reporting"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Groups"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.WorkItemTracking"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.VersionControl"" wizardPage=""true"" /><plugin name=""Microsoft.ProjectCreationWizard.TestManagement"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Build"" wizardPage=""false"" /><plugin name=""Microsoft.ProjectCreationWizard.Lab"" wizardPage=""false"" /></plugins></metadata>",
                    Rank = 2,
                    State = "visible",
                    TemplateId = 536870914
                }
            };
        }
    }
}
