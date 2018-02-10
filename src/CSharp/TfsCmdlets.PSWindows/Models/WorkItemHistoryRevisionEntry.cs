using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Models
{
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