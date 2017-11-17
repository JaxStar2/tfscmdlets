using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Xml;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    public class GlobalListCmdletBase: CollectionLevelCmdlet
    {
        [Parameter(Position = 0)]
        [SupportsWildcards()]
        [Alias("Name")]
        public virtual string GlobalList { get; set; } = "*";

        protected IEnumerable<Models.GlobalList> GetLists()
        {
            return GetLists(GlobalList, Collection);
        }

        protected static IEnumerable<Models.GlobalList> GetLists(string name, object collection)
        {
            var tpc = GetCollection(collection);
            var store = tpc.GetService<WorkItemStore>();
            var xml = store.ExportGlobalLists();

            foreach (var elem in xml.SelectNodes("//GLOBALLIST").OfType<XmlElement>().Where(e => e.GetAttribute("name").IsLike(name)))
            {
                yield return new Models.GlobalList()
                {
                    Name = elem.GetAttribute("name"),
                    Items = new List<string>(elem.ChildNodes.OfType<XmlElement>().Select(e => e.GetAttribute("value")))
                };
            }
        }

        protected XmlDocument GetListsAsXml()
        {
            var tpc = GetCollection();
            var store = tpc.GetService<WorkItemStore>();
            var xml = store.ExportGlobalLists();
            return xml;
        }
    }
}