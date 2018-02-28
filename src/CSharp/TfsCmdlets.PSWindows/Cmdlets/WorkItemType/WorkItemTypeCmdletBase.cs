using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.WorkItemType
{
    public abstract class WorkItemTypeCmdletBase : ProjectLevelCmdlet
    {
        public abstract object Type { get; set; }

        protected IWorkItemTypeAdapter GetWit()
        {
            return GetWit(Type, Project, Collection, Server, Credential);
        }

        protected IEnumerable<IWorkItemTypeAdapter> GetWits()
        {
            return GetWits(Type, Project, Collection, Server, Credential);
        }

        protected IWorkItemTypeAdapter GetWit(object wit, object project, object collection, object server, object credential)
        {
            var collections = GetWits(wit, project, collection, server, credential).ToList();

            if (collections.Count == 0)
                throw new PSArgumentException($"Invalid work item type name '{collection}'", nameof(Collection));
            
            if (collections.Count == 1)
                return collections[0];

            var names = string.Join(", ", collections.Select(o => o.Name).ToArray());
            throw new PSArgumentException($"Ambiguous name '{collection}' matches {collections.Count} team project collections: {names}. Please choose a more specific value for the {nameof(Collection)} argument and try again", nameof(Collection));
        }

        protected IEnumerable<IWorkItemTypeAdapter> GetWits(object wit, object project, object collection, object server, object credential)
        {
            switch (wit)
            {
                case IWorkItemTypeAdapter w:
                    {
                        yield return w;

                        break;
                    }
                case string s when !string.IsNullOrWhiteSpace(s):
                    {
                        var tp = GetProject(project, collection, server, credential);

                        foreach (var w in tp.WorkItemTypes.Where(o => o.Key.IsLike(s)))
                        {
                            yield return w.Value;
                        }

                        break;
                    }
                default:
                    {
                        throw new PSArgumentException($"Invalid work item type '{wit}'");
                    }
            }

        }

        [Import(typeof(IWorkItemTypeService))]
        protected IWorkItemTypeService WorkItemTypeService { get; set; }

    }
}