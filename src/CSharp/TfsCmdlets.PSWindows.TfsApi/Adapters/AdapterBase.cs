using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsCmdlets.Core.Adapters;

namespace TfsCmdlets.PSWindows.TfsApi.Adapters
{
    public abstract class AdapterBase<T>: IAdapter
    {
        protected AdapterBase(T innerInstance)
        {
            InnerInstance = innerInstance;
        }

        protected T InnerInstance { get; private set; }

        public object Instance => InnerInstance;
    }
}
