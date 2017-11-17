using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Cmdlets.Connection;
using TfsCmdlets.Cmdlets.TeamProjectCollection;

namespace TfsCmdlets.Cmdlets
{
    public abstract class ProjectLevelCmdlet : CollectionLevelCmdlet
    {
        [Parameter(ValueFromPipeline = true)]
        public virtual object Project { get; set; }

        [Parameter()]
        public override object Collection { get; set; }

        protected Project GetProject()
        {
            var projects = GetProjects(Project, Collection, Credential).ToList();

            if (projects.Count == 0)
            {
                throw new PSArgumentException($"Invalid project name '{Project}'", nameof(Project));
            }
            else if (projects.Count > 1)
            {
                var names = string.Join(", ", projects.Select(o => o.Name).ToArray());
                throw new PSArgumentException($"Ambiguous name '{Project}' matches {projects.Count} team projects: {names}. Please choose a more specific value for the {nameof(Project)} argument and try again", nameof(Project));
            }

            return projects[0];
        }

        protected IEnumerable<Project> GetProjects(object project, object collection = null, object credential = null)
        {
            switch (project)
            {
                case PSObject pso:
                    {
                        foreach (var p in GetProjects(pso.BaseObject))
                        {
                            yield return p;
                        }
                        break;
                    }
                case Project p:
                    {
                        yield return p;
                        break;
                    }
                case Uri u:
                    {
                        foreach (var p in GetByUrl(u, collection, credential))
                        {
                            yield return p;
                        }
                        break;
                    }
                case string s when (Uri.IsWellFormedUriString(s, UriKind.Absolute)):
                    {
                        foreach (var p in GetByUrl(new Uri(s), collection, credential))
                        {
                            yield return p;
                        }
                        break;
                    }
                case string s when (!string.IsNullOrWhiteSpace(s)):
                    {
                        foreach (var p in GetByName(s, collection, credential))
                        {
                            yield return p;
                        }
                        break;
                    }
                case null when (CurrentConnections.TeamProject != null):
                    {
                        yield return CurrentConnections.TeamProject;
                        break;
                    }
                default:
                    {
                        throw new PSArgumentException(
                            "No TFS team project information available. Either supply a valid -Project argument or use Connect-TfsTeamProject prior to invoking this cmdlet.",
                            nameof(Project));
                    }
            }
        }

        private IEnumerable<Project> GetByUrl(Uri uri, object collection, object credential = null)
        {
            var tpc = GetCollection();
            var css = tpc.GetService<ICommonStructureService>();
            var projInfo = css.GetProject(uri.AbsoluteUri);
            var projectName = projInfo.Name;

            return GetByName(projectName, tpc);
        }

        private IEnumerable<Project> GetByName(string name, object collection, object credential = null)
        {
            var tpc = GetCollection();
            var css = tpc.GetService<ICommonStructureService>();
            var pattern = new WildcardPattern(name);
            var projectInfos = css.ListAllProjects().Where(o => o.Status == ProjectState.WellFormed && pattern.IsMatch(o.Name));
            var store = tpc.GetService<WorkItemStore>();

            foreach (var pi in projectInfos)
            {
                yield return store.Projects[pi.Name];
            }
        }

    }
}
