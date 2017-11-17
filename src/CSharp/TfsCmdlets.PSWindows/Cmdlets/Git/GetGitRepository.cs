using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Cmdlets.TeamProject;

namespace TfsCmdlets.Cmdlets.Git
{
    [Cmdlet(verbName: VerbsCommon.Get, nounName: "GitRepository")]
    [OutputType(typeof(Microsoft.TeamFoundation.SourceControl.WebApi.GitRepository))]
    public class GetGitRepository : GitCmdletBase
    {
        [Parameter()]
        [SupportsWildcards()]
        [Alias("Name")]
        public string Repository { get; set; } = "*";

        protected override void ProcessRecord()
        {
            WriteObject(GetRepositories(Repository), true);
        }
    }
}