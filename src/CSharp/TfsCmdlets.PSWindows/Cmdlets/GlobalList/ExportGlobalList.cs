using System;
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
            var xml = GetListsAsXml();
            var listElements = xml.SelectNodes("//GLOBALLIST")?.OfType<XmlElement>();

            if (listElements == null)
                throw new InvalidOperationException("Error parsing XML contents.");

            foreach (var elem in listElements.Where(e => !e.GetAttribute("name").IsLike(GlobalList)))
            {
                xml.DocumentElement?.RemoveChild(elem);
            }

            WriteObject(xml.OuterXml);
        }

        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Name")]
        public override string GlobalList { get; set; } = "*";

        [Parameter(ValueFromPipeline = true)]
        public override object Collection { get; set; }
    }
}