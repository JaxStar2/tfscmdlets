using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IWorkItemQueryService))]
    public class WorkItemQueryService : IWorkItemQueryService
    {
        public IWorkItemQueryDefinitionAdapter GetDefinition(object item, WorkItemQueryScope scope, object project, object collection,
            object server, object credential)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IWorkItemQueryDefinitionAdapter> GetDefinitions(object item, WorkItemQueryScope scope, object project, object collection, object server,
            object credential)
        {
            throw new NotImplementedException();
        }

        public IWorkItemQueryFolderAdapter GetFolder(object item, WorkItemQueryScope scope, object project, object collection,
            object server, object credential)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IWorkItemQueryFolderAdapter> GetFolders(object item, WorkItemQueryScope scope, object project, object collection, object server,
            object credential)
        {
            throw new NotImplementedException();
        }

        //public T GetItem<T>(object item, WorkItemQueryScope scope, object project, object collection,
        //    object server, object credential)
        //    where T : IWorkItemQueryItemAdapter
        //{
        //    var items = GetItems<T>(item, scope, project, collection, server, credential).ToList();

        //    if (items.Count == 0)
        //        throw new Exception($"Invalid work item query item '{item}'");

        //    if (items.Count == 1)
        //        return items[0];

        //    var names = string.Join(", ", items.Select(o => o.Name).ToArray());
        //    throw new Exception($"Ambiguous work item query item '{item}' matches {items.Count} items: {names}. Please choose a more specific value and try again");
        //}

        //public IEnumerable<T> GetItems<T>(object query, WorkItemQueryScope scope, object project, object collection, object server, object credential)
        //    where T : IWorkItemQueryItemAdapter
        //{
        //    var tp = (Project) ProjectService.GetProject(project, collection, server, credential).Instance;
        //    var qh2 = tp.GetQueryHierarchy2(true);
        //    var rootFolders = qh2.GetChildrenAsync().Result;
        //    var personalScope = rootFolders.First(f => f.IsPersonal);
        //    var sharedScope = rootFolders.First(f => !f.IsPersonal);
        //    var scopePattern = scope == WorkItemQueryScope.Both ? "*" : (scope == WorkItemQueryScope.Personal ? personalScope.Name : sharedScope.Name);
        //    var rootPath = $"{tp.Name}/{scopePattern}";

        //    while (true)
        //    {
        //        switch (query)
        //        {
        //            case T item:
        //                {
        //                    yield return item;
        //                    break;
        //                }
        //            case QueryItem qi:
        //                {
        //                    query = qi.Id;
        //                    continue;
        //                }
        //            case string s when Guid.TryParse(s, out var g):
        //                {
        //                    query = g;
        //                    continue;
        //                }
        //            case Guid g:
        //                {
        //                    query = new Predicate<IWorkItemQueryItemAdapter>(qi => qi.Id == g);
        //                    continue;
        //                }
        //            case string s when !string.IsNullOrEmpty(s):
        //                {
        //                    var normalizedPath = NormalizePath(s, rootPath);
        //                    query = new Predicate<IWorkItemQueryItemAdapter>(qi => qi.Path.IsLike(normalizedPath));  //  || qi.Name.IsLike(s)
        //                    continue;
        //                }
        //            case Predicate<QueryItem2> p:
        //                {
        //                    foreach (var rf in rootFolders)
        //                    {
        //                        foreach (var q in FindItemRecursively<T>((QueryFolder2) rf, p))
        //                        {
        //                            yield return q;
        //                        }
        //                    }
        //                    break;
        //                }
        //        }
        //        break;
        //    }

        //}

        //private static IEnumerable<T> FindItemRecursively<T>(QueryFolder2 folder, Predicate<QueryItem2> criteria)
        //    where T : QueryItem2
        //{
        //    var items = folder.GetChildrenAsync().Result;

        //    foreach (var i in items)
        //    {
        //        switch (i)
        //        {
        //            case T item when criteria(i):
        //                {
        //                    yield return item;

        //                    break;
        //                }
        //            case QueryFolder2 qf2:
        //                {
        //                    foreach (var i2 in FindItemRecursively<T>(qf2, criteria))
        //                    {
        //                        yield return i2;
        //                    }
        //                    break;
        //                }
        //        }
        //    }
        //}

        //private static string NormalizePath(string queryPath, string rootPath)
        //{
        //    var tokens = rootPath.Split('/');
        //    var projectName = tokens[0];
        //    var scopeFolderName = tokens[1];

        //    if (queryPath.StartsWith("/"))
        //    {
        //        queryPath = $"{projectName}/{queryPath}";
        //    }
        //    else if (!queryPath.StartsWith(projectName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        queryPath = $"{projectName}/{scopeFolderName}/{queryPath}";
        //    }

        //    return queryPath.Replace("//", "/");
        //}

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }

    }
}
