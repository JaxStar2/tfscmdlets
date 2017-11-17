using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Management.Automation;
using System.Xml;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    [Cmdlet(verbName: VerbsData.Import, nounName: "GlobalList", ConfirmImpact = ConfirmImpact.Medium, SupportsShouldProcess = true)]
    [OutputType(typeof(Models.GlobalList))]
    public class ImportGlobalList : GlobalListCmdletBase
    {

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [Alias("Xml")]
        public object InputObject { get; set; }

        [Parameter()]
        public SwitchParameter Force { get; set; }

        [Parameter()]
        public override object Collection { get; set; }

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

            var listsToAdd = xml.SelectNodes("//GLOBALLIST/@name").OfType<XmlAttribute>().Select(o => o.Value);
            var existingLists = GetLists("*", Collection).ToList();

            foreach (var name in listsToAdd)
            {
                if (existingLists.Any(o => o.Name.Equals(name)) &&
                    (ShouldProcess(name, "Overwrite global list") || Force.IsPresent)) continue;

                xml.DocumentElement.RemoveChild(xml.SelectSingleNode($"//GLOBALLIST[@name='{name}']"));
            }

            store.ImportGlobalLists(xml.DocumentElement);
        }
    }
}
