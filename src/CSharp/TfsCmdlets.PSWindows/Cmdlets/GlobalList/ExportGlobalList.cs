using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Xml;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    [Cmdlet(VerbsData.Export, "GlobalList")]
    [OutputType(typeof(string))]
    public class ExportGlobalList : GlobalListCmdletBase
    {
        protected override void ProcessRecord()
        {
            var xml = GlobalListService.ExportGlobalLists(Collection, Server, Credential);
            var listElements = xml.SelectNodes("//GLOBALLIST")?.OfType<XmlElement>();

            if (listElements == null)
                throw new InvalidOperationException("Error parsing XML contents.");

            foreach (var elem in listElements.Where(e => !e.GetAttribute("name").IsLike(Name)))
            {
                xml.DocumentElement?.RemoveChild(elem);
            }

            if (!string.IsNullOrWhiteSpace(Destination))
            {
                if (Directory.Exists(Destination))
                {
                    Destination = Path.Combine(Destination, "GlobalLists.xml");
                }

                if (!File.Exists(Destination) || ShouldProcess(Destination, "Overwrite existing file"))
                    xml.Save(Destination);
            }
            else
            {
                WriteObject(xml.OuterXml);
            }
        }

        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("GlobalList")]
        public override string Name { get; set; } = "*";

        [Parameter]
        public string Destination { get; set; }

        [Parameter(ValueFromPipeline = true)]
        public override object Collection { get; set; }
    }
}