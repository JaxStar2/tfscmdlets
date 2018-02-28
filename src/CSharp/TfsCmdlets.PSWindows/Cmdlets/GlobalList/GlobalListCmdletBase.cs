using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml;
using TfsCmdlets.Core.Services;

namespace TfsCmdlets.Cmdlets.GlobalList
{
    public abstract class GlobalListCmdletBase: CollectionLevelCmdlet
    {
        public abstract string Name { get; set; }

        protected Core.Models.GlobalList GetList()
        {
            return GlobalListService.GetGlobalList(Name, Collection, Server, Credential);
        }

        protected IEnumerable<Core.Models.GlobalList> GetLists()
        {
            return GlobalListService.GetGlobalLists(Name, Collection, Server, Credential);
        }

        [Import(typeof(IGlobalListService))]
        protected IGlobalListService GlobalListService { get; set; }
    }
}