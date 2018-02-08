using System;
using System.Management.Automation;
using System.Xml;

namespace TfsCmdlets.Cmdlets.WorkItemType
{
    [Cmdlet(VerbsData.Import, "WorkItemType", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [OutputType(typeof(XmlDocument))]
    public class ImportWorkItemType : WorkItemTypeCmdletBase
    {
        protected override void ProcessRecord()
        {
            XmlDocument doc;

            switch (Type)
            {
                case XmlDocument d:
                    {
                        doc = d;
                        break;
                    }
                case string s:
                    {
                        doc = new XmlDocument();
                        doc.LoadXml(s);
                        break;
                    }
                case null when !string.IsNullOrWhiteSpace(Path):
                    {
                        doc = new XmlDocument();
                        doc.Load(Path);
                        break;
                    }
                default:
                    {
                        throw new Exception("Invalid arguments specified. Supply either a valid XML document or a file path");
                    }
            }

            var tp = GetProject();
            var typeName = doc.SelectSingleNode("//WORKITEMTYPE/@name")?.Value;

            if (string.IsNullOrWhiteSpace(typeName))
                throw new Exception("Invalid work item type definition XML");

            if (!tp.WorkItemTypes.Contains(typeName) || ShouldProcess(typeName, "Overwrite existing work item type"))
            {
                tp.WorkItemTypes.Import(doc.DocumentElement);
            }

            WriteObject(GetWit(typeName, Project, Collection, Server, Credential));
        }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Import from XML")]
        [Alias("Xml")]
        public override object Type { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Import from file")]
        public string Path { get; set; }

        [Parameter]
        public override object Project { get; set; }
    }
}