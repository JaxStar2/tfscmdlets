using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.WorkItem
{
    [Cmdlet(VerbsCommon.Copy, "WorkItem")]
    [OutputType(typeof(Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem))]
    public class CopyWorkItem : WorkItemCmdletBase
    {
        protected override void ProcessRecord()
        {
            var wi = GetWorkItem();
            var tp = wi.Project;
            Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemType targetType;

            while (true)
            {
                switch (Type)
                {
                    case string s:
                        {
                            targetType = tp.WorkItemTypes[s];
                            break;
                        }
                    case Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemType wit:
                        {
                            Type = wit.Name;
                            continue;
                        }
                    default:
                        {
                            Type = wi.Type.Name;
                            continue;
                        }
                }
                break;
            }

            var flags = Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemCopyFlags.None;

            if (IncludeAttachments)
            {
                flags = flags | Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemCopyFlags.CopyFiles;
            }

            if (IncludeLinks)
            {
                flags = flags | Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemCopyFlags.CopyLinks;
            }

            var newWi = wi.Copy(targetType, flags);

            if (!SkipSave)
            {
                newWi.Save();

            }

            switch (Passthru)
            {
                case CopyWorkItemPassthruOptions.Original:
                    {
                        WriteObject(wi);
                        break;
                    }
                case CopyWorkItemPassthruOptions.Copy:
                    {
                        WriteObject(newWi);
                        break;
                    }
            }
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
