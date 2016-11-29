using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Player
    {
        public List<Item> Items { get; set; }
        public string Name { get; set; }

        public Player(string name = Entities.DefaultPlayerName)
        {
            Items = new List<Item>();
            Name = name;
        }

        public string GetItems()
        {
            return string.Join(", ", Items);
        }

        public bool HasAKey()
        {
            return Items.FindAll(x => x.Name == "key").Count == 0;
        }
    }
}
