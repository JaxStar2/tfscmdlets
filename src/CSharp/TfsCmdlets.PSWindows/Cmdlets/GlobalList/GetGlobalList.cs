using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Management.Automation;
using System.Xml;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    [Cmdlet(verbName: VerbsCommon.Get, nounName: "GlobalList")]
    [OutputType(typeof(Models.GlobalList))]
    public class GetGlobalList : GlobalListCmdletBase
    {
        protected override void ProcessRecord()
        {
            var lists = GetLists();

            WriteObject(lists, true);
        }
    }
}