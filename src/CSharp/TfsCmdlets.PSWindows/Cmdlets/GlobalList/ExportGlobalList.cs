using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Management.Automation;
using System.Xml;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    [Cmdlet(verbName: VerbsData.Export, nounName: "GlobalList")]
    [OutputType(typeof(string))]
    public class ExportGlobalList : GlobalListCmdletBase
    {
        protected override void ProcessRecord()
        {
            var xml = GetListsAsXml();

            foreach (var elem in xml.SelectNodes("//GLOBALLIST").OfType<XmlElement>()
                .Where(e => !e.GetAttribute("name").IsLike(GlobalList)))
            {
                xml.DocumentElement.RemoveChild(elem);
            }

            WriteObject(xml.OuterXml);
        }
    }
}