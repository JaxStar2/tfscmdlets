using System;
using Microsoft.TeamFoundation.Client;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class TfsConfigurationServerAdapter: ITfsConfigurationServerAdapter
    {
        public static implicit operator TfsConfigurationServerAdapter(TfsConfigurationServer instance)
        {
            return new TfsConfigurationServerAdapter(instance);
        }

        public static implicit operator TfsConfigurationServer(TfsConfigurationServerAdapter instance)
        {
            return instance.Instance as TfsConfigurationServer;
        }

        private readonly TfsConfigurationServer _instance;

        public TfsConfigurationServerAdapter(TfsConfigurationServer instance)
        {
            _instance = instance;
        }

        public object Instance => _instance;
        public string Name => _instance.Name;
        public Uri Uri => _instance.Uri;
    }
}
