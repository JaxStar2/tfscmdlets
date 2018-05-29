using System;
using System.Collections.Generic;
using System.Text;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IReleaseDefinitionService
    {
        IReleaseDefinitionAdapter GetReleaseDefinition(object build, object project, object collection, object server,
            object credential);

        IEnumerable<IReleaseDefinitionAdapter> GetReleaseDefinitions(object build, object project, object collection, object server,
            object credential);
    }
}
