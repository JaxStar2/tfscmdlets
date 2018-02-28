using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class WorkItemAdapter: AdapterBase<WorkItem>, IWorkItemAdapter
    {
        public WorkItemAdapter(WorkItem innerInstance) : base(innerInstance)
        {
        }

        public int Id => InnerInstance.Id;
        public string Title
        {
            get => InnerInstance.Title;
            set => InnerInstance.Title = value;
        }

        public string State
        {
            get => InnerInstance.State;
            set => InnerInstance.State = value;
        }

        public string AreaPath
        {
            get => InnerInstance.AreaPath;
            set => InnerInstance.AreaPath = value;
        }

        public string IterationPath
        {
            get => InnerInstance.IterationPath;
            set => InnerInstance.IterationPath = value;
        }

        public string Description
        {
            get => InnerInstance.Description;
            set => InnerInstance.Description = value;
        }

        public IWorkItemTypeAdapter Type { get; set; }
        public IWorkItemFieldAdapterCollection Fields { get; set; }
        public IWorkItemRevisionAdapterCollection Revisions { get; }
        public IWorkItemLinkAdapterCollection Links { get; }
    }
}
