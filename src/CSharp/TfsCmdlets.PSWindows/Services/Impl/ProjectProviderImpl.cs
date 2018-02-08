using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Services.Impl
{
    [Export(typeof(IProjectService))]
    internal sealed class ProjectServiceImpl : IProjectService
    {
        public Project GetProject(object project, object collection, object server, object credential)
        {
            var projects = GetProjects(project, collection, server, credential).ToList();

            if (projects.Count == 0)
                throw new Exception($"Invalid project name '{project}'");

            if (projects.Count == 1)
                return projects[0];

            var names = string.Join(", ", projects.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{project}' matches {projects.Count} team projects: {names}. Please choose a more specific value for the -Project argument and try again");
        }

        public IEnumerable<Project> GetProjects(object project, object collection, object server, object credential)
        {
            while (true)
            {
                switch (project)
                {
                    case PSObject pso:
                        {
                            project = pso.BaseObject;
                            continue;
                        }
                    case Project p:
                        {
                            yield return p;
                            break;
                        }
                    case Uri u:
                        {
                            foreach (var p in GetProjectByUrl(u, collection, server, credential))
                            {
                                yield return p;
                            }
                            break;
                        }
                    case string s when Uri.IsWellFormedUriString(s, UriKind.Absolute):
                        {
                            foreach (var p in GetProjectByUrl(new Uri(s), collection, server, credential))
                            {
                                yield return p;
                            }
                            break;
                        }
                    case string s when !string.IsNullOrWhiteSpace(s):
                        {
                            foreach (var p in GetProjectByName(s, collection, server, credential))
                            {
                                yield return p;
                            }
                            break;
                        }
                    case null when CurrentConnectionService.TeamProject != null:
                        {
                            yield return CurrentConnectionService.TeamProject;
                            break;
                        }
                    default:
                        {
                            throw new PSArgumentException("No TFS team project information available. Either supply a valid -Project argument or use Connect-TfsTeamProject prior to invoking this cmdlet.", nameof(Project));
                        }
                }
                break;
            }
        }

        private IEnumerable<Project> GetProjectByUrl(Uri uri, object collection, object server, object credential)
        {
            var tpc = TeamProjectCollectionService.GetCollection(collection, server, credential);
            var css = tpc.GetService<ICommonStructureService>();
            var projInfo = css.GetProject(uri.AbsoluteUri);
            var projectName = projInfo.Name;

            return GetProjectByName(projectName, tpc, server, credential);
        }

        private IEnumerable<Project> GetProjectByName(string name, object collection, object server, object credential)
        {
            var tpc = TeamProjectCollectionService.GetCollection(collection, server, credential);
            var css = tpc.GetService<ICommonStructureService>();
            var projectInfos = css.ListAllProjects().Where(o => o.Status == ProjectState.WellFormed && o.Name.IsLike(name));
            var store = tpc.GetService<WorkItemStore>();

            foreach (var pi in projectInfos)
            {
                yield return store.Projects[pi.Name];
            }
        }

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService TeamProjectCollectionService { get; set; }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }
    }
}