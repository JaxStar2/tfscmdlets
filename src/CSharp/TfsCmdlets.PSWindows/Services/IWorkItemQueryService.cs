using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Services
{
    public interface IWorkItemQueryService
    {
        IEnumerable<T> GetItems<T>(object item, WorkItemQueryScope scope, object project,
            object collection, object server, object credential)
            where T: QueryItem2;
    }
}
