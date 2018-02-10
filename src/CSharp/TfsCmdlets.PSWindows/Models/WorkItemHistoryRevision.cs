using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Models
{
    public class WorkItemHistoryRevision
    {
        internal WorkItemHistoryRevision(Revision revision)
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

            foreach (Field f in revision.Fields)
            {
                if ((!f.IsChangedInRevision) ||
                    (f.ReferenceName.Equals("System.Rev")) ||
                    (f.ReferenceName.Equals("System.Watermark")) ||
                    (f.ReferenceName.Equals("System.ChangedDate")) ||
                    (f.ReferenceName.Equals("System.AuthorizedDate")) ||
                    (f.ReferenceName.Equals("System.RevisedDate")) ||
                    (f.ReferenceName.Equals("System.ChangedBy")) ) continue;

                Changes.Add(new WorkItemHistoryRevisionEntry(f));
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

    public class WorkItemHistoryRevisionEntryCollection : List<WorkItemHistoryRevisionEntry>
    {
        public WorkItemHistoryRevisionEntry this[string referenceName] => Find(f => f.ReferenceName.Equals(referenceName));
    }

    public class WorkItemHistoryRevisionEntry
    {
        internal WorkItemHistoryRevisionEntry(Field field)
        {
            FieldName = field.Name;
            ReferenceName = field.ReferenceName;
            NewValue = field.Value;
            OriginalValue = field.OriginalValue;
        }

        public string FieldName { get; }
        public string ReferenceName { get; set; }
        public object NewValue { get; }
        public object OriginalValue { get; }

        public override string ToString()
        {
            return $"{FieldName}: [{OriginalValue}] => [{NewValue}]";
        }
    }
}
