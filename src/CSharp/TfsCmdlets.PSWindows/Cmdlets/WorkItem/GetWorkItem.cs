using System;
using System.Management.Automation;

namespace TfsCmdlets.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "WorkItem")]
    public class GetWorkItem : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("Hello world!");
        }
    }
}
