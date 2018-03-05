using System;
using System.Collections.Generic;
using System.Text;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.Core.Services
{
    public interface IBuildDefinitionService
    {
        IBuildDefinitionAdapter GetBuildDefinition(object build, object project, object collection, object server,
            object credential);

        IEnumerable<IBuildDefinitionAdapter> GetBuildDefinitions(object build, object project, object collection, object server,
            object credential);
    }
}
