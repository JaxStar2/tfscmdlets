using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.ProcessConfiguration.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IProcessConfigurationService))]
    public class ProcessConfigurationService: IProcessConfigurationService
    {
        public IProjectProcessConfigurationAdapter GetProcessConfiguration(object project, object collection, object server, object credential)
        {
            var tp = (Project) ProjectService.GetProject(project, collection, server, credential).Instance;
            var url = tp.Uri;
            var svc = tp.Store.TeamProjectCollection.GetService<ProjectProcessConfigurationService>();

            return new ProjectProcessConfigurationAdapter(svc.GetProcessConfiguration(url.AbsoluteUri));
        }

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }
    }
}
