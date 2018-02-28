using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.Xml;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Lab.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.VisualStudio.Services.Operations;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;
using Exception = System.Exception;
using ProjectState = Microsoft.TeamFoundation.Core.WebApi.ProjectState;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IProjectService))]
    public sealed class ProjectService : IProjectService
    {
        private const int DELAY_MS = 5000;

        public IProjectAdapter GetProject(object project, object collection, object server, object credential)
        {
            var projects = GetProjects(project, collection, server, credential).ToList();

            if (projects.Count == 0)
                throw new Exception($"Invalid project name '{project}'");

            if (projects.Count == 1)
                return projects[0];

            var names = string.Join(", ", projects.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{project}' matches {projects.Count} team projects: {names}. Please choose a more specific value for the -Project argument and try again");
        }

        public IEnumerable<IProjectAdapter> GetProjects(object project, object collection, object server, object credential)
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
                    case ProjectAdapter adapter:
                        {
                            yield return adapter;
                            break;
                        }
                    case Project p:
                        {
                            yield return new ProjectAdapter(p);
                            break;
                        }
                    case Uri u:
                        {
                            foreach (var p in GetProjectByUrl(u, collection, server, credential))
                            {
                                yield return new ProjectAdapter(p);
                            }
                            break;
                        }
                    case string s when Uri.IsWellFormedUriString(s, UriKind.Absolute):
                        {
                            foreach (var p in GetProjectByUrl(new Uri(s), collection, server, credential))
                            {
                                yield return new ProjectAdapter(p);
                            }
                            break;
                        }
                    case string s when !string.IsNullOrWhiteSpace(s):
                        {
                            foreach (var p in GetProjectByName(s, collection, server, credential))
                            {
                                yield return new ProjectAdapter(p);
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

        public IProjectAdapter CreateProject(string name, string description, object processTemplate, SourceControlType sourceControlType,
            object collection, object server, object credential)
        {
            var tpc = (TfsTeamProjectCollection)CollectionService.GetCollection(collection, server, credential).Instance;
            var template = ProcessTemplateService.GetTemplate(processTemplate, tpc, server, credential);

            var xml = new XmlDocument();
            xml.LoadXml(template.Metadata);

            var templateTypeId = xml.SelectSingleNode("//version/@type")?.Value;

            var client = tpc.GetClient<ProjectHttpClient>();

            var tpInfo = new Microsoft.TeamFoundation.Core.WebApi.TeamProject
            {
                Name = name,
                Description = description,
                Capabilities = new Dictionary<string, Dictionary<string, string>>
                {
                    {
                        "versioncontrol", new Dictionary<string, string>
                        {
                            {"sourceControlType", sourceControlType.ToString()}
                        }
                    },
                    {
                        "processTemplate", new Dictionary<string, string>
                        {
                            {"templateTypeId", templateTypeId}
                        }
                    }
                }
            };

            var token = client.QueueCreateProject(tpInfo).Result;

            if (token == null)
            {
                throw new Exception(
                    $"Error queueing team project creation: {client.LastResponseContext.Exception.Message}");
            }

            if (!WaitForOperation(token, tpc, out var message))
            {
                throw new Exception($"Error creating team project {name}: {message}");
            }

            var store = tpc.GetService<WorkItemStore>();
            store.RefreshCache();

            return GetProject(name, collection, server, credential);
        }

        public void DeleteProject(object project, object collection, object server, object credential)
        {
            var tp = (Project)GetProject(project, collection, server, credential).Instance;
            var tpc = (TfsTeamProjectCollection)CollectionService.GetCollection(collection, server, credential).Instance;
            var client = tpc.GetClient<ProjectHttpClient>();

            var token = client.QueueDeleteProject(tp.Guid).Result;

            if (token == null)
            {
                throw new Exception(
                    $"Error queueing team project deletion: {client.LastResponseContext.Exception.Message}");
            }

            if (!WaitForOperation(token, tpc, out var message))
            {
                throw new Exception($"Error deleting team project {tp.Name}: {message}");
            }

        }

        private static bool WaitForOperation(OperationReference token, TfsTeamProjectCollection tpc, out string message)
        {
            var operationsClient = tpc.GetClient<Microsoft.VisualStudio.Services.Operations.OperationsHttpClient>();
            var opsToken = operationsClient.GetOperation(token.Id).Result;

            while (
                opsToken.Status != Microsoft.VisualStudio.Services.Operations.OperationStatus.Succeeded &&
                opsToken.Status != Microsoft.VisualStudio.Services.Operations.OperationStatus.Failed &&
                opsToken.Status != Microsoft.VisualStudio.Services.Operations.OperationStatus.Cancelled)
            {
                Thread.Sleep(DELAY_MS);
                opsToken = operationsClient.GetOperation(token.Id).Result;
            }

            var status = opsToken.Status;

            message = (status != Microsoft.VisualStudio.Services.Operations.OperationStatus.Succeeded) ?
                operationsClient.LastResponseContext.Exception.Message :
                null;

            return status == OperationStatus.Succeeded;
        }

        private IEnumerable<Project> GetProjectByUrl(Uri uri, object collection, object server, object credential)
        {
            var tpc = (TfsTeamProjectCollection)CollectionService.GetCollection(collection, server, credential).Instance;
            var css = tpc.GetService<Microsoft.TeamFoundation.Server.ICommonStructureService>();
            var projInfo = css.GetProject(uri.AbsoluteUri);
            var projectName = projInfo.Name;

            return GetProjectByName(projectName, tpc, server, credential);
        }

        private IEnumerable<Project> GetProjectByName(string name, object collection, object server, object credential)
        {
            var tpc = (TfsTeamProjectCollection)CollectionService.GetCollection(collection, server, credential).Instance;
            var css = tpc.GetService<Microsoft.TeamFoundation.Server.ICommonStructureService>();
            var projectInfos = css.ListAllProjects().Where(o => o.Status == ProjectState.WellFormed && o.Name.IsLike(name));
            var store = tpc.GetService<WorkItemStore>();

            foreach (var pi in projectInfos)
            {
                yield return store.Projects[pi.Name];
            }
        }

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService CollectionService { get; set; }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }

        [Import(typeof(IProcessTemplateService))]
        private IProcessTemplateService ProcessTemplateService { get; set; }
    }
}