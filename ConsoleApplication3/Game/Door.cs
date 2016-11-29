using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Door
    {
        public string Description { get; set; }
        public Room Target { get; set; }
        public bool Locked { get; set; } = false;

        public string Name { get; set; }


        public Door(string description = "Dull wooden door")
        {
            Description = description;
        }

        public Door(string description = "Dull wooden door", bool locked = false)
        {
            Description = description;
            Locked = locked;
        }

    }
}
