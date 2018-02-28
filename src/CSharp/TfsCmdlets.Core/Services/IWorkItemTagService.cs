using System;
using System.Collections.Generic;
using System.Text;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IWorkItemTagService
    {
        IWorkItemTagAdapter GetTag(object tag, object project, object collection, object server, object credential);

        IEnumerable<IWorkItemTagAdapter> GetTags(object tag, object project, object collection, object server, object credential);
    }
}
