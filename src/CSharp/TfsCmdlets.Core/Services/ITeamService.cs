using System.Collections.Generic;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface ITeamService
    {
        ITeamFoundationTeamAdapter GetTeam(object team, object project, object collection, object server,
            object credential);

        IEnumerable<ITeamFoundationTeamAdapter> GetTeams(object team, object project, object collection, object server,
            object credential);

        ITeamFoundationTeamAdapter GetDefaultTeam(object project, object collection, object server,
            object credential);

        ITeamFoundationTeamAdapter CreateTeam(string name, string description, object project, object collection, object server,
            object credential);

        void DeleteTeam(object team, object project, object collection, object server, object credential);

        ITeamFoundationTeamAdapter RenameTeam(object team, string newName, object project, object collection, object server,
            object credential);

        ITeamFoundationTeamAdapter SetTeam(object team, string newName, string description, object project, object collection, object server,
            object credential);

        void SetDefaultTeam(object team, object project, object collection, object server, object credential);
    }
}