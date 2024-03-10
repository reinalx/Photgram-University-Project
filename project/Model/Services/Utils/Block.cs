using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.Utils
{
    public class Block<E>
    {
        public List<E> Items { get; private set; }
        public bool ExistMoreItems { get; private set; }

        public Block(List<E> items, bool existMoreItmes)
        {
            this.Items = items;
            this.ExistMoreItems = existMoreItmes;
        }

    }
}
