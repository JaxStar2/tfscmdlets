using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;
using TfsCmdlets.PSWindows.TfsApi.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IWorkspaceService))]
    public class WorkspaceService : IWorkspaceService
    {
        public IWorkspaceAdapter GetWorkspace(object workspace, string ownerName, string computerName, object collection, object server, object credential)
        {
            var ws = GetWorkspaces(workspace, ownerName, computerName, collection, server, credential).ToList();

            if (ws.Count == 0)
                throw new Exception($"Invalid workspace '{workspace}'");

            if (ws.Count == 1)
                return ws[0];

            var ids = string.Join(", ", ws.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous workspace '{workspace}' matches {ws.Count} workspaces: {ids}. Please choose a more specific value and try again");
        }

        public IEnumerable<IWorkspaceAdapter> GetWorkspaces(object workspace, string ownerName, string computerName, object collection, object server,
            object credential)
        {
            string ws;

            while (true)
            {
                switch (workspace)
                {
                    case PSObject pso:
                    {
                        workspace = pso.BaseObject;
                        continue;
                    }
                    case Workspace w:
                    {
                        yield return new WorkspaceAdapter(w);
                        yield break;
                    }
                    case string s:
                    {
                        ws = s;
                        break;
                    }
                    default:
                    {
                        throw new InvalidOperationException("Invalid workspace");
                    }
                }
                break;
            }

            var tpc = (TfsTeamProjectCollection)CollectionService.GetCollection(collection, server, credential).Instance;
            var owner = ownerName ?? tpc.AuthorizedIdentity.UniqueName;
            var computer = computerName ?? Environment.MachineName;
            var vcs = tpc.GetService<VersionControlServer>();
            var hasWorkspaceWildcards = ws.Contains("*") || ws.Contains("?");
            var hasOwnerWildcards = owner.Contains("*") || owner.Contains("?");
            var hasComputerWildcards = computer.Contains("*") || computer.Contains("?");
            var hasWildcards = hasWorkspaceWildcards || hasOwnerWildcards || hasComputerWildcards;

            IEnumerable<Workspace> result = vcs.QueryWorkspaces(
                hasWorkspaceWildcards ? null : ws,
                hasOwnerWildcards ? null : owner,
                hasComputerWildcards ? null : computer
            );

            if (hasWildcards)
            {
                result = result.Where(w =>
                    (!hasWorkspaceWildcards || w.Name.IsLike(ws)) &&
                    (!hasOwnerWildcards || w.OwnerAliases.Any(o => o.IsLike(owner))) &&
                    (!hasComputerWildcards|| w.Computer.IsLike(computer))
                );
            }

            foreach (var i in result)
            {
                yield return new WorkspaceAdapter(i);
            }
        }

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService CollectionService { get; set; }
    }
}
