using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IProcessConfigurationService
    {
        IProjectProcessConfigurationAdapter GetProcessConfiguration(object project, object collection, object server,
            object credential);
    }
}
