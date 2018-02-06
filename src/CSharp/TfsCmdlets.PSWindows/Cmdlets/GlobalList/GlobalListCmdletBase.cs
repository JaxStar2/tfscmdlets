using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    public abstract class GlobalListCmdletBase: CollectionLevelCmdlet
    {
        public abstract string GlobalList { get; set; }

        protected IEnumerable<Models.GlobalList> GetLists()
        {
            return GetLists(GlobalList, Collection, Server, Credential);
        }

        protected IEnumerable<Models.GlobalList> GetLists(string name, object collection, object server, object credential)
        {
            var tpc = GetCollection(collection, server, credential);
            var store = tpc.GetService<WorkItemStore>();
            var xml = store.ExportGlobalLists();
            var listElements = xml.SelectNodes("//GLOBALLIST");

            if (listElements == null)
            {
                throw new InvalidOperationException("Error retrieving global lists from TFS. XML is empty or invalid.");
            }

            foreach (var elem in listElements.OfType<XmlElement>().Where(e => e.GetAttribute("name").IsLike(name)))
            {
                yield return new Models.GlobalList
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