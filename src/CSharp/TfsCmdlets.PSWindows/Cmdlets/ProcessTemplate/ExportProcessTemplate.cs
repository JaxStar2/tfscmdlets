using System;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Compression;
using System.Management.Automation;
using System.Xml;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.ProcessTemplate
{
    [Cmdlet(VerbsData.Export, "ProcessTemplate", ConfirmImpact = ConfirmImpact.Medium)]
    public class ExportProcessTemplate : CollectionLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            var template = ProcessTemplateService.GetTemplate(ProcessTemplate, Collection, Server, Credential);
            var templateName = !string.IsNullOrEmpty(NewName) ? NewName : template.Name;
            var description = !string.IsNullOrEmpty(NewDescription) ? NewDescription : template.Description;

            var tempFile = ProcessTemplateService.GetTemplateData(template, Collection, Server, Credential);
            var zipFile = $"{tempFile}.zip";

            File.Move(tempFile, zipFile);

            var outDir = Path.Combine(DestinationPath, templateName);

            if (Directory.Exists(outDir))
            {
                if (!Force)
                    throw new Exception(
                        $"Output directory '{outDir}' already exists. To overwrite an existing directory, use the Force switch");

                Directory.Delete(outDir, true);
            }

            Directory.CreateDirectory(outDir);

            ZipFile.ExtractToDirectory(zipFile, outDir);

            if (!string.IsNullOrEmpty(NewName) || !string.IsNullOrEmpty(NewDescription))
            {
                var xmlFileName = Path.Combine(outDir, "ProcessTemplate.xml");
                var doc = new XmlDocument(); doc.Load(xmlFileName);
                var metadataElem = ((XmlElement)doc.DocumentElement.SelectSingleNode("//ProcessTemplate/metadata"));

                metadataElem["name"].InnerText = templateName;
                metadataElem["description"].InnerText = description;
                metadataElem["version"].SetAttribute("type", Guid.NewGuid().ToString());
                metadataElem["version"].SetAttribute("major", "1");
                metadataElem["version"].SetAttribute("minor", "0");

                doc.Save(xmlFileName);
            }

            File.Delete(zipFile);
        }

        [Parameter(Position = 0)]
        [Alias("Name", "Process")]
        [SupportsWildcards]
        public object ProcessTemplate { get; set; } = "*";

        [Parameter(Position = 1, Mandatory = true)]
        public string DestinationPath { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string NewName { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string NewDescription { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        [Parameter(ValueFromPipeline = true)]
        public override object Collection { get; set; }

        [Import(typeof(IProcessTemplateService))]
        private IProcessTemplateService ProcessTemplateService { get; set; }

    }
}
