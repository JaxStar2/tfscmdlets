using System;
using System.Management.Automation;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Cmdlets.WorkItem
{
    [Cmdlet(VerbsCommon.Copy, "WorkItem")]
    [OutputType("Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem,Microsoft.TeamFoundation.WorkItemTracking.Client")]
    public class CopyWorkItem : WorkItemCmdletBase
    {
        protected override void ProcessRecord()
        {
            throw new NotImplementedException();

            //var wi = GetWorkItem();
            //var tp = wi.Project;
            //IWorkItemTypeAdapter targetType;

            //while (true)
            //{
            //    switch (Type)
            //    {
            //        case string s:
            //            {
            //                targetType = tp.WorkItemTypes[s];
            //                break;
            //            }
            //        case Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemType wit:
            //            {
            //                Type = wit.Name;
            //                continue;
            //            }
            //        default:
            //            {
            //                Type = wi.Type.Name;
            //                continue;
            //            }
            //    }
            //    break;
            //}

            //var flags = WorkItemCopyFlags.None;

            //if (IncludeAttachments)
            //{
            //    flags = flags | WorkItemCopyFlags.CopyFiles;
            //}

            //if (IncludeLinks)
            //{
            //    flags = flags | WorkItemCopyFlags.CopyLinks;
            //}

            //var newWi = wi.Copy(targetType, flags);

            //if (!SkipSave)
            //{
            //    newWi.Save();

            //}

            //switch (Passthru)
            //{
            //    case CopyWorkItemPassthruOptions.Original:
            //        {
            //            WriteObject(wi);
            //            break;
            //        }
            //    case CopyWorkItemPassthruOptions.Copy:
            //        {
            //            WriteObject(newWi);
            //            break;
            //        }
            //}
        }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [Alias("id")]
        [ValidateNotNull]
        public override object WorkItem { get; set; }

        [Parameter]
        public object Type { get; set; }

        [Parameter()]
        public SwitchParameter IncludeAttachments { get; set; }

        [Parameter]
        public SwitchParameter IncludeLinks { get; set; }

        [Parameter]
        public SwitchParameter SkipSave { get; set; }

        [Parameter]
        public CopyWorkItemPassthruOptions Passthru { get; set; } = CopyWorkItemPassthruOptions.Copy;

        [Parameter]
        public override object Project { get; set; }
    }
}
