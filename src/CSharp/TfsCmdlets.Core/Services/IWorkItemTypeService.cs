using System.Collections.Generic;
using System.Xml;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IWorkItemTypeService
    {
        IWorkItemTypeAdapter GetWorkItemType(object type, object project, object collection, object server, object credential);
        IEnumerable<IWorkItemTypeAdapter> GetWorkItemTypes(object type, object project, object collection, object server, object credential);
        XmlDocument Export(bool includeGlobalLists);
        void Import(IProjectAdapter tp, XmlElement documentElement);
        bool Exists(string typeName, IProjectAdapter tp);
    }
}
