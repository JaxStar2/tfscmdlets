using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TfsCmdlets.CorrectnessTests
{
    internal static class Extensions
    {
        public static bool IsMatch(this string obj, string pattern)
        {
            return Regex.IsMatch(obj, pattern);
        }

        public static bool HasArgument(this Type type, string argumentName)
        {
            var pi = type.GetProperty(argumentName);

            return pi?.GetCustomAttribute<ParameterAttribute>() != null;
        }
    }
}
