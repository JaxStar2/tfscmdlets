using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class ProjectAdapter: AdapterBase<Project>, IProjectAdapter
    {
        public ProjectAdapter(Project innerInstance) : base(innerInstance)
        {
        }

        private IDictionary<string, IWorkItemTypeAdapter> GetWorkItemTypes()
        {
            var types = new Dictionary<string, IWorkItemTypeAdapter>();

            foreach (WorkItemType wit in InnerInstance.WorkItemTypes)
            {
                types.Add(wit.Name, new WorkItemTypeAdapter(wit));
            }

            return types;
        }

        public string Name => InnerInstance.Name;
        public IDictionary<string, IWorkItemTypeAdapter> WorkItemTypes => GetWorkItemTypes();
        public Guid Guid => InnerInstance.Guid;
        public Uri Uri => InnerInstance.Uri;
    }
}