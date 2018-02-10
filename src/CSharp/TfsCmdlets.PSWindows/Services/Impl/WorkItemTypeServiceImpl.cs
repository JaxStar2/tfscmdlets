using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Services.Impl
{
    [Export(typeof(IWorkItemTypeService))]
    public class WorkItemTypeServiceImpl : IWorkItemTypeService
    {
        public WorkItemType GetWorkItemType(object type, object project, object collection, object server, object credential)
        {
            var servers = GetWorkItemTypes(type, project, collection, server, credential).ToList();

            if (servers.Count == 0)
                throw new Exception($"Invalid work item type name '{server}'");

            if (servers.Count == 1)
                return servers[0];

            var names = string.Join(", ", servers.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{server}' matches {servers.Count} work item types: {names}. " +
                                "Please choose a more specific value for the Type argument and try again");

        }

        public IEnumerable<WorkItemType> GetWorkItemTypes(object type, object project, object collection, object server, object credential)
        {
            while (true)
            {
                switch (type)
                {
                    case PSObject pso:
                        {
                            type = pso.BaseObject;
                            continue;
                        }
                    case WorkItemType wit:
                        {
                            yield return wit;
                            break;
                        }
                    case string s when !string.IsNullOrEmpty(s):
                        {
                            var tp = ProjectService.GetProject(project, collection, server, credential);

                            yield return tp.WorkItemTypes[s];
                            break;
                        }
                    default:
                        {
                            throw new Exception("Invalid work item type '{type}'.");
                        }
                }

                break;
            }
        }

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }
    }
}
