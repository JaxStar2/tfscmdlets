//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Composition;
//using System.Linq;
//using Microsoft.TeamFoundation.WorkItemTracking.Client;
//using Microsoft.TeamFoundation.WorkItemTracking.Client.Fakes;
//using TfsCmdlets.Services;

//namespace TfsCmdlets.UnitTests.Stubs.Services
//{
//    [Export(typeof(IWorkItemQueryService))]
//    public class WorkItemQueryServiceStub : IWorkItemQueryService
//    {
//        public IEnumerable<T> GetItems<T>(object queryItem, WorkItemQueryScope scope, object project, object collection,
//            object server, object credential) where T : QueryItem2
//        {
//            var scopePattern = scope == WorkItemQueryScope.Both
//                ? "*"
//                : (scope == WorkItemQueryScope.Personal ? "My Queries" : "Shared Queries");
//            var rootPath = $"MyProject/{scopePattern}";

//            while (true)
//            {
//                switch (queryItem)
//                {
//                    case T item:
//                        {
//                            yield return item;
//                            break;
//                        }
//                    case QueryItem qi:
//                        {
//                            queryItem = qi.Id;
//                            continue;
//                        }
//                    case string s when Guid.TryParse(s, out var g):
//                        {
//                            queryItem = g;
//                            continue;
//                        }
//                    case Guid g:
//                        {
//                            queryItem = new Predicate<QueryItem2>(qi => qi.Id == g);
//                            continue;
//                        }
//                    case string s when !string.IsNullOrEmpty(s):
//                        {
//                            var normalizedPath = NormalizePath(s, rootPath);
//                            queryItem = new Predicate<QueryItem2>(qi =>
//                                qi.Path.IsLike(normalizedPath)); //  || qi.Name.IsLike(s)
//                            continue;
//                        }
//                    case Predicate<QueryItem2> p:
//                        {
//                            foreach (QueryItem2 q in _folders.Where(f => p(f)))
//                            {
//                                yield return (T) q;
//                            }
//                        }
//                        break;
//                }
//                break;
//            }
//        }

//        private static string NormalizePath(string queryPath, string rootPath)
//        {
//            var tokens = rootPath.Split('/');
//            var projectName = tokens[0];
//            var scopeFolderName = tokens[1];

//            if (queryPath.StartsWith("/"))
//            {
//                queryPath = $"{projectName}/{queryPath}";
//            }
//            else if (!queryPath.StartsWith(projectName, StringComparison.OrdinalIgnoreCase))
//            {
//                queryPath = $"{projectName}/{scopeFolderName}/{queryPath}";
//            }

//            return queryPath.Replace("//", "/");
//        }

//        private static QueryHierarchy2 GetStubbedHierarchy()
//        {
//            return CreateHierarchy(new[]
//            {
//                CreateFolder("My Queries", new[]
//                {
//                    CreateFolder("A personal folder", new List<QueryItem2>
//                    {

//                    })
//                }),
//                CreateFolder("Shared Queries", new[]
//                {
//                    CreateFolder("Current Iteration", new List<QueryItem2>
//                    {

//                    }),
//                    CreateFolder("Troubleshooting", new List<QueryItem2>
//                    {

//                    })
//                })
//            });
//        }

//        private static QueryHierarchy2 CreateHierarchy(IEnumerable<QueryFolder2> rootFolders)
//        {
//            var hierarchy = new ShimQueryHierarchy2().Instance;

//            typeof(QueryHierarchy2).GetProperty("IsPopulated").SetValue(hierarchy, true);
//            var add = (Action<QueryItem2, bool>)typeof(QueryItem2Collection).GetMethod("AddInternal", new Type[] { typeof(QueryItem2), typeof(bool) })
//                .CreateDelegate(typeof(Action<QueryItem2, bool>), hierarchy.GetChildren());

//            foreach (var f in rootFolders)
//            {
//                add(f, false);
//            }

//            return hierarchy;
//        }

//        private static QueryFolder2 CreateFolder(string name, IEnumerable<QueryItem2> elements, bool isPersonal = false)
//        {
//            var folder = new QueryFolder2(name);
//            typeof(QueryFolder2).GetProperty("IsPopulated").SetValue(folder, true);
//            typeof(QueryFolder2).GetProperty("IsPersonal").SetValue(folder, isPersonal);
//            typeof(QueryFolder2).GetProperty("IsNew").SetValue(folder, false);

//            foreach (var e in elements)
//            {
//                folder.GetChildren().Add(e);
//            }

//            return folder;
//        }

//        private static QueryFolder2 GetShimmedFolder(string name, string path, Guid id, bool isPersonal)
//        {
//            var folder = new ShimQueryFolder2();

//            var _ = new ShimQueryItem2(folder)
//            {
//                NameGet = () => name,
//                PathGet = () => path,
//                IdGet = () => id,
//                IsPersonalGet = () => isPersonal
//            };

//            return folder;
//        }
//    }
//}
