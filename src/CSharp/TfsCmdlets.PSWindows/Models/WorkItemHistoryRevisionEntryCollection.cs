using System.Collections.Generic;

namespace TfsCmdlets.Models
{
    public class WorkItemHistoryRevisionEntryCollection : List<WorkItemHistoryRevisionEntry>
    {
        public WorkItemHistoryRevisionEntry this[string referenceName] => Find(f => f.ReferenceName.Equals(referenceName));
    }
}