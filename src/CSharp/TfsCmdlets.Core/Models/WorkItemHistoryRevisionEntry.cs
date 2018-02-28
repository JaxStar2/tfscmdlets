namespace TfsCmdlets.Core.Models
{
    public class WorkItemHistoryRevisionEntry
    {
        internal WorkItemHistoryRevisionEntry(string name, string referenceName, object value, object originalValue)
        {
            FieldName = name;
            ReferenceName = referenceName;
            NewValue = value;
            OriginalValue = originalValue;
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