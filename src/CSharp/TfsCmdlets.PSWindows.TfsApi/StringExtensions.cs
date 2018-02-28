using System.Collections.Generic;
using System.Management.Automation;

namespace TfsCmdlets.PSWindows.TfsApi
{
    public static class StringExtensions
    {
        private static readonly Dictionary<string, WildcardPattern> CachedPatterns = new Dictionary<string, WildcardPattern>();

        public static bool IsLike(this string s, string pattern)
        {
            if (!CachedPatterns.ContainsKey(pattern))
            {
                CachedPatterns.Add(pattern, new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
            }

            return CachedPatterns[pattern].IsMatch(s);
        }
    }
}
