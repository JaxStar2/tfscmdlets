using System;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Models
{
    public class WorkItemHistoryRevision
    {
        public WorkItemHistoryRevision(IWorkItemRevisionAdapter revision)
        {
            WorkItemId = (int)revision.Fields["System.Id"].Value;
            WorkItemTitle = (string)revision.Fields["System.Title"].Value;
            WorkItemType = (string)revision.Fields["System.WorkItemType"].Value;
            Revision = (int)revision.Fields["System.Rev"].Value;
            ChangedDate = (DateTime)revision.Fields["System.ChangedDate"].Value;
            ChangedDate = (DateTime)revision.Fields["System.AuthorizedDate"].Value;
            RevisedDate = (DateTime)revision.Fields["System.RevisedDate"].Value;
            ChangedBy = (string)revision.Fields["System.ChangedBy"].Value;
            Changes = new WorkItemHistoryRevisionEntryCollection();

            foreach (var f in revision.Fields)
            {
                if ((!f.IsChangedInRevision) ||
                    (f.ReferenceName.Equals("System.Rev")) ||
                    (f.ReferenceName.Equals("System.Watermark")) ||
                    (f.ReferenceName.Equals("System.ChangedDate")) ||
                    (f.ReferenceName.Equals("System.AuthorizedDate")) ||
                    (f.ReferenceName.Equals("System.RevisedDate")) ||
                    (f.ReferenceName.Equals("System.ChangedBy")) ) continue;

                Changes.Add(new WorkItemHistoryRevisionEntry(f.Name, f.ReferenceName, f.Value, f.OldValue));
            }
        }

        public int WorkItemId { get; }

        public string WorkItemTitle { get; }

        public string WorkItemType { get; }

        public int Revision { get; set; }

        public DateTime ChangedDate { get; }

        public string ChangedBy { get; }

        public DateTime AuthorizedDate { get; }

        public DateTime RevisedDate { get; }

        public WorkItemHistoryRevisionEntryCollection Changes { get; }

        public int ChangeCount => Changes.Count;

        public string FriendlyDescription => $"{WorkItemType} #{WorkItemId}";

    }
}
