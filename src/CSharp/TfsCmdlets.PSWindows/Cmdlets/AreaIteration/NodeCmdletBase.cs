using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Xml;
using TfsCmdlets.Core;
using TfsCmdlets.Core.Adapters;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.AreaIteration
{
    public abstract class NodeCmdletBase : ProjectLevelCmdlet
    {
        protected NodeCmdletBase()
        {
            var name = GetType().GetCustomAttributes(true).OfType<CmdletAttribute>().Select(attr => attr.NounName).First();
            Scope = (NodeScope) Enum.Parse(typeof(NodeScope), name);
        }

        protected IEnumerable<INodeInfoAdapter> GetNodes(object node)
        {
            if (node is INodeInfoAdapter n)
            {
                yield return n; yield break;
            }

            var tp = GetProject();
            var projectName = tp.Name;

            if (node is Uri u)
            {
                yield return CommonStructureService.GetNode(u.AbsoluteUri, Collection, Server, Credential); yield break;
            }

            var rootPath = NormalizePath($@"{projectName}\{Scope}", projectName, Scope.ToString());
            var fullPath = NormalizePath($@"{rootPath}\{node}", projectName, Scope.ToString());
            var rootNodeUri = CommonStructureService.GetNodeFromPath(rootPath, Collection, Server, Credential).Uri;
            var rootElement = CommonStructureService.GetNodesXml(new[] { rootNodeUri }, true, Collection, Server, Credential);
            var nodePaths = (rootElement.SelectNodes("//@Path") ?? throw new InvalidOperationException())
                .Cast<XmlNode>().Select(e => e.Value).Where(s => s.IsLike(fullPath));

            foreach (var p in nodePaths)
            {
                yield return CommonStructureService.GetNodeFromPath(p, Collection, Server, Credential);
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

        public abstract object Path { get; set; }

        protected NodeScope Scope { get; }

        [Import(typeof(ICommonStructureService))]
        protected ICommonStructureService CommonStructureService { get; set; }
    }
}