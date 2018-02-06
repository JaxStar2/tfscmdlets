using System.Management.Automation;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    [Cmdlet(VerbsCommon.Get, "GlobalList")]
    [OutputType(typeof(Models.GlobalList))]
    public class GetGlobalList : GlobalListCmdletBase
    {
        protected override void ProcessRecord()
        {
            var lists = GetLists();

            WriteObject(lists, true);
        }

        [Parameter(Position = 0)]
        [SupportsWildcards]
        [Alias("Name")]
        public override string GlobalList { get; set; } = "*";

        [Parameter(ValueFromPipeline = true)]
        public override object Collection { get; set; }
    }
}