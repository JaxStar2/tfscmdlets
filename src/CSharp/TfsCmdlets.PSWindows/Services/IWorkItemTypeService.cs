using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Services
{
    public interface IWorkItemTypeService
    {
        WorkItemType GetWorkItemType(object type, object project, object collection, object server, object credential);
        IEnumerable<WorkItemType> GetWorkItemTypes(object type, object project, object collection, object server, object credential);
    }
}
