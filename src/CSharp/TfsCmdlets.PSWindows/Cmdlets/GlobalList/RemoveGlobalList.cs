using System.Linq;
using System.Management.Automation;
using System.Xml;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    [Cmdlet(VerbsCommon.Remove, "GlobalList", ConfirmImpact = ConfirmImpact.High,
        SupportsShouldProcess = true)]
    [OutputType(typeof(Core.Models.GlobalList))]
    public class RemoveGlobalList : GlobalListCmdletBase
    {
        protected override void ProcessRecord()
        {
            var lists = GetLists();

            foreach (var list in lists)
            {
                if (!ShouldProcess(list.Name, "Remove global list")) continue;

                GlobalListService.DeleteGlobalList(list.Name, Collection, Server, Credential);
            }
        }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        [Alias("GlobalList")]
        public override string Name { get; set; }

        [Parameter]
        public override object Collection { get; set; }
    }
}