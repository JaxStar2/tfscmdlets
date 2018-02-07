using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Client.Fakes;

namespace TfsCmdlets.UnitTests
{
    public static class MockHelper
    {
        public static void MockRegisteredTfsConnections ()
        {
            ShimRegisteredTfsConnections.GetConfigurationServers = SampleData.GetRegisteredServers;
        }
    }
}
