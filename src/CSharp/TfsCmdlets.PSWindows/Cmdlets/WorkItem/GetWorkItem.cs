using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Management.Automation;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.WorkItem
{
    [Cmdlet(VerbsCommon.Get, "WorkItem", DefaultParameterSetName = "Query by text")]
    [OutputType("Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem,Microsoft.TeamFoundation.WorkItemTracking.Client")]
    public class GetWorkItem : WorkItemCmdletBase
    {
        protected override void ProcessRecord()
        {
            WriteObject(GetWorkItems(Revision, AsOf, Query, Filter, Text, Macros), true);
        }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Query by revision")]
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Query by date")]
        [Alias("id")]
        [ValidateNotNull()]
        public override object WorkItem { get; set; }

        [Parameter(ParameterSetName = "Query by revision")]
        [Alias("Rev")]
        public object Revision { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Query by date")]
        public object AsOf { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Query by WIQL")]
        [Alias("WIQL", "QueryText", "SavedQuery", "QueryPath")]
        public string Query { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Query by filter")]
        public string Filter { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Query by text")]
        public string Text { get; set; }

        [Parameter()]
        public Dictionary<string,object> Macros { get; set; }

        [Parameter(ValueFromPipeline = true)]
        public override object Project { get; set; }
    }
}
