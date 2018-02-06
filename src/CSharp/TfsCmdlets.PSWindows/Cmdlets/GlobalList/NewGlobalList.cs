using System.Collections;
using System.Management.Automation;
using System.Xml;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    [Cmdlet(VerbsCommon.New, "GlobalList", ConfirmImpact = ConfirmImpact.Medium,
        SupportsShouldProcess = true)]
    [OutputType(typeof(Models.GlobalList))]
    public class NewGlobalList : GlobalListCmdletBase
    {
        protected override void BeginProcessing()
        {
            var tpc = GetCollection();
            var store = tpc.GetService<WorkItemStore>();
            var xml = store.ExportGlobalLists();

            // Checks whether the global list already exists
            var list = (XmlElement) xml.SelectSingleNode($"//GLOBALLIST[@name='{GlobalList}']");

            if (list != null)
            {
                if (Force.IsPresent && ShouldProcess($"{GlobalList}", "Overwrite existing global list"))
                {
                    list.ParentNode?.RemoveChild(list);
                }
                else
                {
                    throw new PSInvalidOperationException(
                        "Global List Name already exists. To overwrite an existing list, use the -Force switch.");
                }
            }

            if (!ShouldProcess(GlobalList, "Create global list")) return;

            list = xml.CreateElement("GLOBALLIST");
            list.SetAttribute("name", GlobalList);

            // Adds the item elements to the list
            foreach (var item in Items)
            {
                var itemElement = xml.CreateElement("LISTITEM");
                itemElement.SetAttribute("value", item.ToString());
                list.AppendChild(itemElement);
            }

            // Appends the new list to the XML obj
            xml.DocumentElement?.RemoveAll();
            xml.DocumentElement?.AppendChild(list);
            store.ImportGlobalLists(xml.DocumentElement);

            if (Passthru)
            {
                WriteObject(GetLists(GlobalList, Collection, Server, Credential), true);
            }
        }
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Alias("Name")]
        public override string GlobalList { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public IEnumerable Items { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Parameter]
        public override object Collection { get; set; }
    }
}