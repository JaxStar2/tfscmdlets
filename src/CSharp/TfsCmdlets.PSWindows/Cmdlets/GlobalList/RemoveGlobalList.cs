using System.Linq;
using System.Management.Automation;
using System.Xml;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    [Cmdlet(VerbsCommon.Remove, "GlobalList", ConfirmImpact = ConfirmImpact.High,
        SupportsShouldProcess = true)]
    [OutputType(typeof(Models.GlobalList))]
    public class RemoveGlobalList : GlobalListCmdletBase
    {
        protected override void ProcessRecord()
        {
            var lists = GetLists();
            var listsToRemove = lists.Where(o => ShouldProcess(o.Name, "Remove global list")).ToList();

            if (listsToRemove.Count == 0) return;

            var xml = new XmlDocument();
            xml.LoadXml("<Package />");

            foreach (var list in listsToRemove)
            {
                var elem = xml.CreateElement("DestroyGlobalList");
                elem.SetAttribute("ListName", "*" + list.Name);
                elem.SetAttribute("ForceDelete", "true");
                xml.DocumentElement?.AppendChild(elem);
            }

            var tpc = GetCollection();
            var store = tpc.GetService<WorkItemStore>();

            store.SendUpdatePackage(xml.DocumentElement, out var _, false);
        }

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public override string GlobalList { get; set; }

        [Parameter]
        public override object Collection { get; set; }
    }
}