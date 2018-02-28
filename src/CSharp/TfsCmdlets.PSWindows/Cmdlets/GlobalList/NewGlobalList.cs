using System;
using System.Linq;
using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    [Cmdlet(VerbsCommon.New, "GlobalList", ConfirmImpact = ConfirmImpact.Medium,
        SupportsShouldProcess = true)]
    [OutputType(typeof(Core.Models.GlobalList))]
    public class NewGlobalList : GlobalListCmdletBase
    {
        protected override void ProcessRecord()
        {
            var lists = GetLists();
            var forceCreation = false;

            if (lists.Any(gl => gl.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)))
            {
                if (!Force.IsPresent && !ShouldProcess($"{Name}", "Overwrite existing global list"))
                {
                    throw new InvalidOperationException(
                        $"Global List {Name} already exists. To overwrite an existing list, use the -Force switch.");
                }

                forceCreation = true;
            }

            if (!forceCreation && !ShouldProcess(Name, "Create global list")) return;

            var list = GlobalListService.CreateGlobalList(Name, Items, forceCreation, Collection, Server, Credential);

            if (Passthru)
            {
                WriteObject(list);
            }
        }
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        [Alias("GlobalList")]
        public override string Name { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 1)]
        public string[] Items { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        [Parameter]
        public SwitchParameter Passthru { get; set; }

        [Parameter]
        public override object Collection { get; set; }
    }
}