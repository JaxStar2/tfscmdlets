using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Cmdlets.WorkItemQuery
{
    [Cmdlet(VerbsCommon.Get, "WorkItemQuery")]
    [OutputType(typeof(QueryDefinition2))]
    public class GetWorkItemQuery : ProjectLevelCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(GetQueries(Query, Scope, Project, Collection, Server, Credential), true);
        }

        protected IEnumerable<QueryDefinition2> GetQueries(object query, WorkItemQueryScope scope, object project,
            object collection, object server, object credential)
        {
            switch (query)
            {
                case QueryDefinition2 qd2:
                {
                    yield return qd2;

                    break;
                }
                case string s when !string.IsNullOrEmpty(s):
                {
                    //var tp = GetProject();
                    //var qh2 = tp.GetQueryHierarchy2(true);
                    //var rootFolders = qh2.GetChildrenAsync().Result;

                    break;
                }
            }

            throw new NotImplementedException();

            //if (Query - is  [Microsoft.TeamFoundation.WorkItemTracking.Client.QueryDefinition2])
            //{
            //    return Query
            //}

            //tp = Get - TfsTeamProject - Project Project - Collection Collection
            //    qh = tp.GetQueryHierarchy2(true)
            //qh.GetChildrenAsync().Wait()


            //rootFolders = (qh.GetChildren() | Where - Object {
            //    Scope - eq "Both" - or _.IsPersonal - eq(Scope - eq "Personal")
            //})

            //rootFolders | _GetQueriesRecursively | Where - Object {
            //    (_.Path - like Query) -or(_.Name - like Query) - or(_.RelativePath - like Query)
            //} | Sort - Object ParentPath, Name
        }

        [Parameter(Position = 0)]
        [ValidateNotNull]
        [SupportsWildcards]
        [Alias("Path")]
        public object Query { get; set; } = "**/*";

        [Parameter]
        [ValidateSet("Personal", "Shared", "Both")]
        public WorkItemQueryScope Scope { get; set; } = WorkItemQueryScope.Both;

        [Parameter(ValueFromPipeline = true)]
        public override object Project { get; set; }
    }
}
