using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Services.Common;

namespace TfsCmdlets
{
    internal static class StringExtensions
    {
        private static readonly Dictionary<string, WildcardPattern> CachedPatterns = new Dictionary<string, WildcardPattern>();

        internal static bool IsLike(this string s, string pattern)
        {
            var wildcardPattern = CachedPatterns.GetOrAddValue(pattern, () => new WildcardPattern(pattern, WildcardOptions.IgnoreCase));

            return wildcardPattern.IsMatch(s);
        }
    }
}
