using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsCmdlets.Core.Models;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.PSWindows.TfsApi.Services
{
    [Export(typeof(IGlobalListService))]
    public class GlobalListService : IGlobalListService
    {
        public GlobalList GetGlobalList(string globalList, object collection, object server, object credential)
        {
            var lists = GetGlobalLists(globalList, collection, server, credential).ToList();

            if (lists.Count == 0)
                throw new Exception($"Invalid global list '{globalList}'");

            if (lists.Count == 1)
                return lists[0];

            var names = string.Join(", ", lists.Select(o => o.Name).ToArray());
            throw new Exception($"Ambiguous name '{globalList}' matches {lists.Count} global lists: {names}. " +
                                "Please choose a more specific value for the -GlobalList argument and try again");
        }

        public IEnumerable<GlobalList> GetGlobalLists(string globalList, object collection, object server, object credential)
        {
            var store = GetWorkItemStore(collection, server, credential);
            var xml = store.ExportGlobalLists();
            var listElements = xml.SelectNodes("//GLOBALLIST");

            if (listElements == null)
            {
                throw new InvalidOperationException("Error retrieving global lists from TFS. XML is empty or invalid.");
            }

            foreach (var elem in listElements.OfType<XmlElement>().Where(e => e.GetAttribute("name").IsLike(globalList)))
            {
                yield return new GlobalList
                {
                    Name = elem.GetAttribute("name"),
                    Items = new List<string>(elem.ChildNodes.OfType<XmlElement>().Select(e => e.GetAttribute("value")))
                };
            }
        }

        public GlobalList CreateGlobalList(string globalList, IEnumerable<string> items, bool force, object collection,
            object server, object credential)
        {
            var store = GetWorkItemStore(collection, server, credential);
            var xml = store.ExportGlobalLists();
            var listItems = items.ToList();

            // Checks whether the global list already exists
            var list = (XmlElement)xml.SelectSingleNode($"//GLOBALLIST[@name='{globalList}']");

            if (list != null)
            {
                if (force)
                {
                    list.ParentNode?.RemoveChild(list);
                }
                else
                {
                    throw new InvalidOperationException($"Global list {globalList} already exists.");
                }
            }

            list = xml.CreateElement("GLOBALLIST");
            list.SetAttribute("name", globalList);

            // Adds the item elements to the list
            foreach (var item in listItems)
            {
                var itemElement = xml.CreateElement("LISTITEM");
                itemElement.SetAttribute("value", item);
                list.AppendChild(itemElement);
            }

            // Appends the new list to the XML obj
            xml.DocumentElement?.RemoveAll();
            xml.DocumentElement?.AppendChild(list);
            store.ImportGlobalLists(xml.DocumentElement);

            return new GlobalList
            {
                Name = globalList,
                Items = new List<string>(listItems)
            };
        }

        public void DeleteGlobalList(string globalList, object collection, object server, object credential)
        {
            var list = GetGlobalList(globalList, collection, server, credential);
            var store = GetWorkItemStore(collection, server, credential);
            var xml = new XmlDocument();

            xml.LoadXml("<Package />");

            var elem = xml.CreateElement("DestroyGlobalList");
            elem.SetAttribute("ListName", "*" + list.Name);
            elem.SetAttribute("ForceDelete", "true");
            xml.DocumentElement?.AppendChild(elem);

            store.SendUpdatePackage(xml.DocumentElement, out var _, false);
        }

        public void ImportGlobalLists(XmlElement xmlElement, object collection, object server, object credential)
        {
            var store = GetWorkItemStore(collection, server, credential);
            store.ImportGlobalLists(xmlElement);
        }

        public XmlDocument ExportGlobalLists(object collection, object server, object credential)
        {
            var store = GetWorkItemStore(collection, server, credential);
            return store.ExportGlobalLists();
        }

        private WorkItemStore GetWorkItemStore(object collection, object server, object credential)
        {
            var tpc = (TfsTeamProjectCollection)CollectionService.GetCollection(collection, server, credential).Instance;
            var store = tpc.GetService<WorkItemStore>();
            return store;
        }

        [Import(typeof(ITeamProjectCollectionService))]
        private ITeamProjectCollectionService CollectionService { get; set; }
    }
}
