using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.TeamFoundation.Server;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    public abstract class NodeCmdletBase : ProjectLevelCmdlet
    {
        protected NodeCmdletBase()
        {
            var name = GetType().GetCustomAttributes(true).OfType<CmdletAttribute>().Select(attr => attr.NounName).First();
            Scope = (NodeScope) Enum.Parse(typeof(NodeScope), name);
        }

        protected IEnumerable<NodeInfo> GetNodes(object node)
        {
            if (node is NodeInfo n)
            {
                yield return n; yield break;
            }

            var tp = GetProject();
            var tpc = tp.Store.TeamProjectCollection;
            var projectName = tp.Name;
            var cssService = tpc.GetService<ICommonStructureService>();

            if (node is Uri u)
            {
                yield return cssService.GetNode(u.AbsoluteUri); yield break;
            }

            var rootPath = NormalizePath($@"{projectName}\{Scope}", projectName, Scope.ToString());
            var fullPath = NormalizePath($@"{rootPath}\{node}", projectName, Scope.ToString());
            var rootNodeUri = cssService.GetNodeFromPath(rootPath).Uri;
            var rootElement = cssService.GetNodesXml(new[] { rootNodeUri }, true);
            var nodePaths = (rootElement.SelectNodes("//@Path") ?? throw new InvalidOperationException())
                .Cast<XmlNode>().Select(e => e.Value).Where(s => s.IsLike(fullPath));

            foreach (var p in nodePaths)
            {
                yield return cssService.GetNodeFromPath(p);
            }
        }

        protected static string NormalizePath(string path, string projectName, string scope)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            if (!Regex.IsMatch(path, $@"^\\?{projectName}\\{scope}"))
            {
                path = $@"\{projectName}\{scope}\{path}";
            }

            var newPath = Regex.Replace(path, @"\\{2,}", @"\");

            if (!newPath.StartsWith(@"\"))
            {
                newPath = $@"\{newPath}";
            }

            if (newPath.EndsWith(@"\"))
            {
                newPath = newPath.Substring(0, newPath.Length - 1);
            }

            return newPath;
        }

        protected ICommonStructureService GetCssService()
        {
            var tp = GetProject();
            var tpc = tp.Store.TeamProjectCollection;

            return tpc.GetService<ICommonStructureService>();
        }

        public abstract object Path { get; set; }

        protected NodeScope Scope { get; }
    }
}