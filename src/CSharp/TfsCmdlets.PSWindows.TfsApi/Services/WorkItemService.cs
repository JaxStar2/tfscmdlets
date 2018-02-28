using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.VisualStudio.Services.Common;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;
using WorkItemCopyFlags = TfsCmdlets.Core.WorkItemCopyFlags;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IWorkItemService))]
    public class WorkItemService : IWorkItemService
    {
        public IWorkItemAdapter GetWorkItem(object workItem, object revision, object asof, string query, string filter, string text, Dictionary<string, object> macros, object project, object collection, object server, object credential)
        {
            var wis = GetWorkItems(workItem, revision, asof, query, filter, text, macros, project, collection, server, credential).ToList();

            if (wis.Count == 0)
                throw new Exception($"Invalid work item ID '{workItem}'");

            if (wis.Count == 1)
                return wis[0];

            var ids = string.Join(", ", wis.Select(o => o.Id).ToArray());
            throw new Exception($"Ambiguous ID '{workItem}' matches {wis.Count} work items: {ids}. Please choose a more specific value for the -WorkItem argument and try again");
        }

        public IEnumerable<IWorkItemAdapter> GetWorkItems(object workItem, object revision, object asof, string query, string filter, string text, Dictionary<string, object> macros, object project, object collection, object server, object credential)
        {
            while (true)
            {
                switch (workItem)
                {
                    case PSObject pso:
                        {
                            workItem = pso.BaseObject;
                            continue;
                        }
                    case WorkItem wi when revision == null && asof == null:
                        {
                            yield return new WorkItemAdapter(wi);
                            break;
                        }
                    case WorkItem wi:
                        {
                            workItem = new[] { wi.Id };
                            continue;
                        }
                    case int id:
                        {
                            workItem = new[] { id };
                            continue;
                        }
                    case IEnumerable<int> ids:
                        {
                            IEnumerable<int> revs = null;
                            IEnumerable<DateTime> asofDates = null;

                            switch (revision)
                            {
                                case PSObject pso:
                                {
                                    revision = pso.BaseObject;
                                    continue;
                                }
                                case int i:
                                    {
                                        revs = new[] { i };
                                        break;
                                    }
                                case IEnumerable<int> list:
                                    {
                                        revs = list;
                                        break;
                                    }
                            }

                            switch (asof)
                            {
                                case PSObject pso:
                                {
                                    asof = pso.BaseObject;
                                    continue;
                                }
                                case DateTime d:
                                    {
                                        asofDates = new[] { d };
                                        break;
                                    }
                                case IEnumerable<DateTime> list:
                                    {
                                        asofDates = list;
                                        break;
                                    }
                            }

                            foreach (var i in GetByIds(ids, revs, asofDates, project, collection, server, credential))
                            {
                                yield return new WorkItemAdapter(i);
                            }

                            break;
                        }
                    case null when !string.IsNullOrWhiteSpace(query):
                        {
                            foreach (var i in GetByWiql(query, macros, project, collection, server, credential))
                            {
                                yield return new WorkItemAdapter(i);
                            }
                            break;
                        }
                    case null when !string.IsNullOrWhiteSpace(filter):
                        {
                            foreach (var i in GetByFilter(filter, macros, project, collection, server, credential))
                            {
                                yield return new WorkItemAdapter(i);
                            }
                            break;
                        }
                    case null when !string.IsNullOrWhiteSpace(text):
                        {
                            foreach (var wi in GetByText(text, macros, project, collection, server, credential))
                            {
                                yield return new WorkItemAdapter(wi);
                            }
                            break;
                        }
                    default:
                        {
                            throw new Exception("Invalid WorkItem argument. Check the supplied value and try again.");
                        }
                }
                break;
            }
        }

        public IWorkItemAdapter GetWorkItem(int id, bool bypassRules)
        {
            throw new NotImplementedException();
        }

        public IWorkItemAdapter NewWorkItem(IWorkItemTypeAdapter wit)
        {
            throw new NotImplementedException();
        }

        public IWorkItemAdapter NewWorkItem(IWorkItemTypeAdapter wit, bool bypassRules)
        {
            throw new NotImplementedException();
        }

        public void DestroyWorkItems(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public void Save(IWorkItemAdapter wi)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IWorkItemFieldAdapter> Validate(IWorkItemAdapter wi)
        {
            throw new NotImplementedException();
        }

        public IWorkItemAdapter Copy(IWorkItemAdapter wi, IWorkItemTypeAdapter targetType, WorkItemCopyFlags flags)
        {
            throw new NotImplementedException();
        }

        protected virtual IEnumerable<WorkItem> GetByIds(IEnumerable<int> ids, IEnumerable<int> rev, IEnumerable<DateTime> asof, object project, object collection, object server, object credential)
        {
            var tp = ((Project) ProjectService.GetProject(project, collection, server, credential).Instance);
            var store = tp.Store;
            var idList = ids.ToList();

            if (rev != null)
            {
                var revList = rev.ToList();

                if (revList.Count > 1 && revList.Count != idList.Count)
                {
                    throw new Exception(
                        "When supplying a list of IDs and Revision, both must have the same number of elements");
                }

                for (var i = 0; i < idList.Count; i++)
                {
                    yield return store.GetWorkItem(idList[i], revList.Count == 1 ? revList[0] : revList[i]);
                }
            }
            else if (asof != null)
            {
                var asofList = asof.ToList();

                if (asofList.Count > 1 && asofList.Count != idList.Count)
                {
                    throw new Exception(
                        "When supplying a list of IDs and Changed Dates (AsOf), both must have the same number of elements");
                }

                for (var i = 0; i < idList.Count; i++)
                {
                    yield return store.GetWorkItem(idList[i], asofList.Count == 1 ? asofList[0] : asofList[i]);
                }
            }
            else
            {
                foreach (var i in idList) yield return store.GetWorkItem(i);
            }
        }

        protected virtual IEnumerable<WorkItem> GetByText(string text, Dictionary<string, object> macros, object project, object collection, object server, object credential)
        {
            var localMacros = new Dictionary<string, object>
            {
                {"TfsQueryText", text}
            };

            if (macros != null && macros.Count > 0)
            {
                localMacros.AddRange(macros);
            }

            return GetByWiql("SELECT * FROM WorkItems WHERE [System.Title] CONTAINS @TfsQueryText OR [System.Description] CONTAINS @TfsQueryText", localMacros, project, collection, server, credential);
        }

        protected virtual IEnumerable<WorkItem> GetByFilter(string filter, Dictionary<string, object> macros, object project, object collection, object server, object credential)
        {
            return GetByWiql($"SELECT * FROM WorkItems WHERE {filter}", macros, project, collection, server, credential);
        }

        protected virtual IEnumerable<WorkItem> GetByWiql(string queryText, Dictionary<string, object> macros, object project, object collection, object server, object credential)
        {
            var tp = ((Project) ProjectService.GetProject(project, collection, server, credential).Instance);
            var store = tp.Store;

            if (!queryText.IsLike("SELECT *"))
            {
                var savedQuery = WorkItemQueryService.GetDefinition(queryText, WorkItemQueryScope.Both,
                    project, collection, server, credential);

                queryText = savedQuery.QueryText;
            }

            if (macros == null)
            {
                macros = new Dictionary<string, object>();
            }

            if (queryText.IndexOf("@project", StringComparison.OrdinalIgnoreCase) > 0 && !macros.ContainsKey("Project"))
            {
                macros["Project"] = tp.Name;
            }

            if (queryText.IndexOf("@me", StringComparison.OrdinalIgnoreCase) > 0 && !macros.ContainsKey("Me"))
            {
                macros["Me"] = store.TeamProjectCollection.AuthorizedIdentity.DisplayName;
            }

            var wis = store.Query(queryText, macros);

            foreach (WorkItem wi in wis)
            {
                yield return wi;
            }
        }

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }

        [Import(typeof(IWorkItemQueryService))]
        private IWorkItemQueryService WorkItemQueryService { get; set; }
    }
}
