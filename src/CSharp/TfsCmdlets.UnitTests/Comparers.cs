using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;

namespace TfsCmdlets.UnitTests
{
    public class RegisteredConfigurationServerComparer: IComparer, IComparer<RegisteredConfigurationServer>
    {
        public bool Equals(RegisteredConfigurationServer x, RegisteredConfigurationServer y)
        {
            return x.Name.Equals(y.Name) &&
                   x.Uri.Equals(y.Uri) &&
                   x.InstanceId.Equals((y.InstanceId));
        }

        public int GetHashCode(RegisteredConfigurationServer obj)
        {
            return obj.GetHashCode();
        }

        public int Compare(object x, object y)
        {
            return Compare((RegisteredConfigurationServer) x, (RegisteredConfigurationServer) y);
        }

        public int Compare(RegisteredConfigurationServer x, RegisteredConfigurationServer y)
        {
            return (x.Name.Equals(y.Name) &&
                   x.Uri.Equals(y.Uri) &&
                   x.InstanceId.Equals((y.InstanceId))? 0: -1);
        }
    }
}
