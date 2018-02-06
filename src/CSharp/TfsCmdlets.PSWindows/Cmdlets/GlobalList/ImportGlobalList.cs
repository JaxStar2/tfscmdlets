using System.Linq;
using System.Management.Automation;
using System.Xml;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    [Cmdlet(VerbsData.Import, "GlobalList", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType(typeof(Models.GlobalList))]
    public class ImportGlobalList : GlobalListCmdletBase
    {
        protected override void ProcessRecord()
        {
            var tpc = GetCollection();
            var store = tpc.GetService<Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore>();
            string rawDocument;

            while (InputObject is PSObject pso)
            {
                InputObject = pso.BaseObject;
            }

            switch (InputObject)
            {
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
                case Models.GlobalList gl:
                    {
                        rawDocument = gl.ToXml().OuterXml;
                        break;
                    }
                default:
                    {
                        throw new PSArgumentException("Invalid global list definition");
                    }
            }

            var xml = new XmlDocument();
            xml.LoadXml(rawDocument);

            var listsToAdd = xml.SelectNodes("//GLOBALLIST/@name")?.OfType<XmlAttribute>().Select(o => o.Value);
            var existingLists = GetLists("*", Collection, Server, Credential).ToList();

            if (listsToAdd != null)
            {
                foreach (var name in listsToAdd)
                {
                    if (existingLists.Any(o => o.Name.Equals(name)) &&
                        (ShouldProcess(name, "Overwrite global list") || Force.IsPresent)) continue;

                    var listElement = xml.SelectSingleNode($"//GLOBALLIST[@name='{name}']");

                    if (listElement != null)
                        xml.DocumentElement?.RemoveChild(listElement);
                }
            }

            store.ImportGlobalLists(xml.DocumentElement);
        }

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [Alias("Xml")]
        public object InputObject { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        [Parameter]
        public override object Collection { get; set; }

        public override string GlobalList { get; set; }
    }
}
