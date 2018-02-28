using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TfsCmdlets.Core.Models;

namespace TfsCmdlets.Core.Services
{
    public interface IGlobalListService
    {
        GlobalList GetGlobalList(string globalList, object collection, object server, object credential);

        IEnumerable<GlobalList> GetGlobalLists(string globalList, object collection, object server, object credential);

        GlobalList CreateGlobalList(string globalList, IEnumerable<string> items, bool force, object collection,
            object server, object credential);

        void DeleteGlobalList(string globalList, object collection, object server, object credential);

        void ImportGlobalLists(XmlElement xmlElement, object collection, object server, object credential);

        XmlDocument ExportGlobalLists(object collection, object server, object credential);
    }
}
