using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Cmdlets.WorkItem
{
    [Cmdlet(VerbsCommon.Remove, "WorkItem", ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    [OutputType(typeof(Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem))]
    public class RemoveWorkItem : WorkItemCmdletBase
    {
        protected override void ProcessRecord()
        {
            var wis = GetWorkItems();
            var ids = (from wi in wis where ShouldProcess($"{wi.Type.Name} #{wi.Id} ('{wi.Title}')", "Remove work item") select wi.Id).ToList();
            var tpc = GetCollection();
            var store = tpc.GetService<WorkItemStore>();

            store.DestroyWorkItems(ids);
        }

        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
        public override object WorkItem { get; set; }

        // Hidden 
        public override object Project { get; set; }
    }
}
