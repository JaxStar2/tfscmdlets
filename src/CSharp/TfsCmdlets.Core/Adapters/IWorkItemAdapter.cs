using System;
using System.Text;

namespace TfsCmdlets.Core.Adapters
{
    public interface IWorkItemAdapter: IAdapter
    {
        int Id { get; }
        string Title { get; set; }
        string State { get; set; }
        string AreaPath { get; set; }
        string IterationPath { get; set; }
        string Description { get; set; }
        IWorkItemTypeAdapter Type { get; set; }
        IWorkItemFieldAdapterCollection Fields { get; set; }
        IWorkItemRevisionAdapterCollection Revisions { get; }
        IWorkItemLinkAdapterCollection Links { get; }
    }
}
