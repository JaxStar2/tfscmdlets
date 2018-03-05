using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IWorkItemTypeService))]
    public class WorkItemTypeService : ServiceBase<WorkItemType, WorkItemTypeAdapter>, IWorkItemTypeService
    {
        public XmlDocument Export(bool includeGlobalLists)
        {
            throw new NotImplementedException();
        }

        public void Import(IProjectAdapter tp, XmlElement documentElement)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string typeName, IProjectAdapter tp)
        {
            throw new NotImplementedException();
        }

        #region Get Items

        protected override string ItemName => "work item type";
        protected override Func<WorkItemType, string> ItemDescriptor => (wit => wit.Name);

        public IWorkItemTypeAdapter GetWorkItemType(object type, object project, object collection, object server, object credential)
            => new WorkItemTypeAdapter(GetItem(type, project, collection, server, credential));

        public IEnumerable<IWorkItemTypeAdapter> GetWorkItemTypes(object type, object project, object collection, object server, object credential)
            => GetItems(type, project, collection, server, credential).Select(wit => new WorkItemTypeAdapter(wit));

        protected override IEnumerable<WorkItemType> GetAllItems(object item, ScopeObjects so)
        {
            var tp = (Project)ProjectService.GetProject(so.Project, so.Collection, so.Server, so.Credential).Instance;

            return tp.WorkItemTypes.Cast<WorkItemType>();
        }

        #endregion

        #region Imports

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }

        #endregion
    }
}
