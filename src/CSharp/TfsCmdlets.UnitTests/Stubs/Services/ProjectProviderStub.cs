using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client.Fakes;
using TfsCmdlets.Services;

namespace TfsCmdlets.UnitTests.Stubs.Services
{
    [Export(typeof(IProjectService))]
    internal sealed class ProjectServiceStub : IProjectService
    {
        private readonly List<Project> _projects = GetShimmedProjects();

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
            return _projects.ToList();
        }

        private static List<Project> GetShimmedProjects()
        {
            return new List<Project>
            {
                new ShimProject
                {
                    NameGet = () => "Project Foo",
                    GuidGet = () => new Guid("3932f2e2-7190-48a2-adb2-0c9b6569b430"),
                    UriGet = () => new Uri("vstfs:///Classification/TeamProject/3932f2e2-7190-48a2-adb2-0c9b6569b430"),
                    IdGet = () => 1
                }.Instance,
                new ShimProject
                {
                    NameGet = () => "Project Bar",
                    GuidGet = () => new Guid("5d1e6336-25e8-49fe-8be1-c0a3a55a03e3"),
                    UriGet = () => new Uri("vstfs:///Classification/TeamProject/5d1e6336-25e8-49fe-8be1-c0a3a55a03e3"),
                    IdGet = () => 2
                }.Instance,
                new ShimProject
                {
                    NameGet = () => "Project Baz",
                    GuidGet = () => new Guid("15305dcc-0784-4ea0-97b4-8d8c846ef92e"),
                    UriGet = () => new Uri("vstfs:///Classification/TeamProject/15305dcc-0784-4ea0-97b4-8d8c846ef92e"),
                    IdGet = () => 3
                }.Instance
            };
        }

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService TeamProjectCollectionService { get; set; }

        [Import(typeof(ICurrentConnectionService))]
        private ICurrentConnectionService CurrentConnectionService { get; set; }
    }
}