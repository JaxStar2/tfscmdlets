using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Lab.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Cmdlets.Connection;
using TfsCmdlets.Cmdlets.TeamProjectCollection;

namespace TfsCmdlets.Cmdlets.TeamProject
{
    [Cmdlet(verbName:VerbsCommon.Get, nounName:"TeamProject", DefaultParameterSetName = "Get by project")]
    [OutputType(typeof(Microsoft.TeamFoundation.WorkItemTracking.Client.Project))]
    public class GetTeamProject : ProjectLevelCmdlet
    {
        [Parameter(Position = 0, ParameterSetName = "Get by project")]
        public override object Project { get; set; } = "*";

        [Parameter(ValueFromPipeline = true, Position = 1, ParameterSetName = "Get by project")]
        public override object Collection { get; set; }

        [Parameter(Position = 0, ParameterSetName = "Get current")]
        public SwitchParameter Current { get; set; }

        protected override void ProcessRecord()
        {
            if (Current.IsPresent)
            {
                WriteObject(CurrentConnections.TeamProject);
                return;
            }

            foreach (var p in GetProjects(Project, Collection))
            {
               WriteObject(p);
            }
        }
    }
}