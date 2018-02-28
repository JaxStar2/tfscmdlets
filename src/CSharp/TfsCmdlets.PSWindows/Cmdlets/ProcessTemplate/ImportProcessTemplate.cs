using System;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Compression;
using System.Management.Automation;
using System.Xml;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.ProcessTemplate
{
    [Cmdlet(VerbsData.Import, "ProcessTemplate", ConfirmImpact = ConfirmImpact.Medium)]
    public class ImportProcessTemplate : ProcessTemplateCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [Alias("Path")]
        public string SourcePath { get; set; }

        [Parameter()]
        [ValidateSet("Visible")]
        public string State { get; set; } = "Visible";

        protected override void ProcessRecord()
        {
            var sourceXmlFile = Path.Combine(SourcePath, "ProcessTemplate.xml");

            if (!File.Exists(sourceXmlFile))
            {
                throw new Exception(
                    $"Invalid path. Source path '{SourcePath}' must be a directory and must contain a file named ProcessTemplate.xml.");
            }

            var tpc = GetCollection();
            var tempFile = System.IO.Path.GetTempFileName();
            var zipFile = $"{tempFile}.zip";

            ZipFile.CreateFromDirectory(SourcePath, zipFile, CompressionLevel.Optimal, false);

            var doc = new XmlDocument();
            doc.Load(sourceXmlFile);

            var name = doc.SelectSingleNode("//ProcessTemplate/metadata/name").InnerText;
            var description = doc.SelectSingleNode("//ProcessTemplate/metadata/description").InnerText;
            var metadata = doc.SelectSingleNode("//ProcessTemplate/metadata").OuterXml;

            try
            {
                ProcessTemplateService.AddUpdateTemplate(name, description, metadata, State, zipFile);

            }
            catch (System.Net.WebException ex)
            {
                var errMessage = ex.Message;

                if (ex.Response is System.Net.HttpWebResponse r)
                {
                    errMessage += $" {r.StatusDescription}";
                }

                throw new Exception($"Error uploading process template to server: '{errMessage}'", ex);
            }
            finally
            {
                File.Delete(zipFile);
            }
        }

        [Parameter]
        public override object Collection { get; set; }

        // Hidden
        public override object ProcessTemplate { get; set; }
    }
}
