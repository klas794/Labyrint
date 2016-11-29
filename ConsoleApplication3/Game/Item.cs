using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public bool IsKey { get; set; }

        private Dictionary<Item, Item> CombineableItems { get; set; }

        public Item( string name, string description )
        {
            Name = name;
            Description = description;
            CombineableItems = new Dictionary<Item, Item>();
        }

        public void CombineWithItem(Item item, Item product)
        {
            CombineableItems[item] = product;
        }

        public bool IsUsableWith(Item item)
        {
            return CombineableItems.ContainsKey(item);
        }

        public Item UseWith(Item match)
        {
            return CombineableItems[match];
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
