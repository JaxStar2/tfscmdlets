using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class GitRepositoryAdapter: AdapterBase<GitRepository>, IGitRepositoryAdapter
    {
        public GitRepositoryAdapter(GitRepository innerInstance) : base(innerInstance)
        {
        }

        public string Name => InnerInstance.Name;
        public object Id => InnerInstance.Id;
    }
}
