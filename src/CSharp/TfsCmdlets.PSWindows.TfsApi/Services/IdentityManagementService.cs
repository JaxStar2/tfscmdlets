using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;
using IIdentityManagementService = TfsCmdlets.Core.Services.IIdentityManagementService;
using ITeamProjectCollectionService = TfsCmdlets.Core.Services.ITeamProjectCollectionService;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IIdentityManagementService))]
    public class IdentityManagementService : IIdentityManagementService
    {
        public ITeamFoundationIdentityAdapter GetGroup(string name, GroupScope scope, object project, object collection, object server,
            object credential)
        {
            var svc = GetIdentityManagementService(collection, server, credential);

            var identity = svc.ReadIdentity(IdentitySearchFactor.DisplayName, name, MembershipQuery.Direct, ReadIdentityOptions.None);

            return new TeamFoundationIdentityAdapter(identity);
        }

        public IEnumerable<ITeamFoundationIdentityAdapter> GetGroups(string name, GroupScope scope, object project, object collection, object server,
            object credential)
        {
            string scopeId = null;
            TfsTeamProjectCollection tpc;
            var svc = GetIdentityManagementService(collection, server, credential);

            switch (scope)
            {
                case GroupScope.Server:
                    {
                        throw new NotImplementedException();
                    }
                case GroupScope.Collection:
                    {
                        tpc = (TfsTeamProjectCollection)CollectionService.GetCollection(collection, server, credential).Instance;
                        break;
                    }
                case GroupScope.Project:
                    {
                        var tp = (Project)ProjectService.GetProject(project, collection, server, credential).Instance;
                        scopeId = tp.Uri.AbsoluteUri;
                        tpc = tp.Store.TeamProjectCollection;
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }

            foreach (var i in svc.ListApplicationGroups(scopeId, ReadIdentityOptions.None)
                .Where(g => g.DisplayName.IsLike(name)))
            {
                yield return new TeamFoundationIdentityAdapter(i);
            }
        }

        public void CreateGroup(string name, string description, GroupScope scope, object project, object collection,
            object server, object credential)
        {
            string scopeId = null;
            TfsTeamProjectCollection tpc;

            switch (scope)
            {
                case GroupScope.Server:
                    {
                        throw new NotImplementedException();
                    }
                case GroupScope.Collection:
                    {
                        tpc = (TfsTeamProjectCollection)CollectionService.GetCollection(collection, server, credential).Instance;
                        break;
                    }
                case GroupScope.Project:
                    {
                        var tp = (Project)ProjectService.GetProject(project, collection, server, credential).Instance;
                        scopeId = tp.Uri.AbsoluteUri;
                        tpc = tp.Store.TeamProjectCollection;
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }

            var svc = tpc.GetService<IIdentityManagementService2>();

            var group = svc.CreateApplicationGroup(scopeId, name, description);
        }

        public void DeleteGroup(object identityDescriptor, object collection, object server, object credential)
        {
            var svc = GetIdentityManagementService(collection, server, credential);

            svc.DeleteApplicationGroup((IdentityDescriptor)identityDescriptor);
        }

        public IEnumerable<ITeamFoundationIdentityAdapter> GetGroupMembers(string name, object collection, object server, object credential)
        {
            var svc = GetIdentityManagementService(collection, server, credential);

            throw new NotImplementedException();
        }

        private IIdentityManagementService2 GetIdentityManagementService(object collection, object server, object credential)
        {
            var tpc = (TfsTeamProjectCollection)CollectionService.GetCollection(collection, server, credential)
                .Instance;
            var svc = tpc.GetService<IIdentityManagementService2>();

            return svc;
        }

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService CollectionService { get; set; }

        [Import(typeof(IProjectService))]
        private IProjectService ProjectService { get; set; }
    }
}
