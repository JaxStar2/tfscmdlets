using System.Collections.Generic;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IIdentityManagementService
    {
        ITeamFoundationIdentityAdapter GetGroup(string name, GroupScope scope, object project, object collection, object server, object credential);

        IEnumerable<ITeamFoundationIdentityAdapter> GetGroups(string name, GroupScope scope, object project, object collection, object server, object credential);

        ITeamFoundationIdentityAdapter CreateGroup(string name, string description, GroupScope scope, object project, object collection, object server, object credential);

        void DeleteGroup(object identityDescriptor, object collection, object server, object credential);

        IEnumerable<ITeamFoundationIdentityAdapter> GetGroupMembers(string name, object collection, object server, object credential);
    }
}