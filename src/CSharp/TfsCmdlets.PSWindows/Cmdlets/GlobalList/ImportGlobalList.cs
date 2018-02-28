using System;
using System.Linq;
using System.Management.Automation;
using System.Xml;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    [Cmdlet(VerbsData.Import, "GlobalList", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType(typeof(Core.Models.GlobalList))]
    public class ImportGlobalList : GlobalListCmdletBase
    {
        protected override void ProcessRecord()
        {
            var tpc = GetCollection();
            string rawDocument;

            while (true)
            {
                switch (InputObject)
                {
                    case PSObject pso:
                    {
                        InputObject = pso.BaseObject;
                        continue;
                    }
                    case XmlDocument x:
                    {
                        rawDocument = x.OuterXml;
                        break;
                    }
                    case string s:
                    {
                        rawDocument = s;
                        break;
                    }
                    case Core.Models.GlobalList gl:
                    {
                        rawDocument = gl.ToXml().OuterXml;
                        break;
                    }
                    default:
                    {
                        throw new ArgumentException("Invalid global list definition");
                    }
                }
                break;
            }

            var xml = new XmlDocument();
            xml.LoadXml(rawDocument);

            var listsToAdd = xml.SelectNodes("//GLOBALLIST/@name")?.OfType<XmlAttribute>().Select(o => o.Value);
            var existingLists = GlobalListService.GetGlobalLists("*", Collection, Server, Credential).ToList();

            if (listsToAdd != null)
            {
                foreach (var name in listsToAdd)
                {
                    if (existingLists.Any(o => o.Name.Equals(name)) &&
                        (Force.IsPresent || ShouldProcess(name, "Overwrite global list"))) continue;

                    var listElement = xml.SelectSingleNode($"//GLOBALLIST[@name='{name}']");

                    if (listElement != null)
                        xml.DocumentElement?.RemoveChild(listElement);
                }
            }

            GlobalListService.ImportGlobalLists(xml.DocumentElement, Collection, Server, Credential);
        }

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [Alias("Xml")]
        public object InputObject { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        // Hidden 
        public override string Name { get; set; }
    }
}
