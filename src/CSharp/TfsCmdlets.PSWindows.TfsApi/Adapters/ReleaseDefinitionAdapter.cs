﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public class ReleaseDefinitionAdapter : AdapterBase<ReleaseDefinition>, IReleaseDefinitionAdapter
    {
        public ReleaseDefinitionAdapter(ReleaseDefinition innerInstance) : base(innerInstance)
        {
        }

        public string Name => InnerInstance.Name;
    }
}