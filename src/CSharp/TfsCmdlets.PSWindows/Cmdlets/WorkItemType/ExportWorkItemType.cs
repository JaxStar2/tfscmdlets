using System.IO;
using System.Management.Automation;
using System.Xml;

namespace TfsCmdlets.Cmdlets.WorkItemType
{
    [Cmdlet(VerbsData.Export, "WorkItemType", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [OutputType(typeof(XmlDocument))]
    public class ExportWorkItemType : WorkItemTypeCmdletBase
    {
        protected override void ProcessRecord()
        {
            foreach (var wit in GetWits())
            {
                var doc = WorkItemTypeService.Export(IncludeGlobalLists.IsPresent);

                if (!string.IsNullOrWhiteSpace(Destination))
                {
                    if (Directory.Exists(Destination))
                    {
                        Destination = Path.Combine(Destination, $"{wit.Name}.xml");
                    }

                    if(!File.Exists(Destination) || ShouldProcess(Destination, "Overwrite existing file"))
                        doc.Save(Destination);
                }
                else
                {
                    WriteObject(doc);
                }
            }
        }

        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Name")]
        public override object Type { get; set; } = "*";

        [Parameter]
        public SwitchParameter IncludeGlobalLists { get; set; }

        [Parameter]
        public string Destination { get; set; }

        [Parameter(ValueFromPipeline = true)]
        public override object Project { get; set; }
    }
}