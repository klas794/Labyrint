using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Room
    {
        public string Description { get; set; }

        public Dictionary<Direction, Door> Doors { get; set; }

        public List<Item> Items { get; set; }

        public Room(string description)
        {
            Doors = new Dictionary<Direction, Door>();
            Items = new List<Item>();
            Description = description;
        }
        
        public void AddDoor(Direction direction, Door door)
        {
            Doors[direction] = door;
        }

        public void AddDoor(Direction direction, string doorDescription)
        {   
            Doors[direction] = new Door(doorDescription);
        }

        public void AddDoor(Direction direction, string doorDescription, bool locked)
        {
            Doors[direction] = new Door(doorDescription, locked);
        }

        public void PointDoor(Direction direction, Room target)
        {
            
            if (Doors.ContainsKey(direction))
            {
                Doors[direction].Target = target;
            }
            else
            {
                throw new Exception("Trying to set target of non-existing door.");
            }
        }

        public string GetItems()
        {
            return Items.Count == 0 ? "None" : string.Join(", ", Items);
        }

        public string GetDoors()
        {
            return Doors.Keys.Count == 0 ? "None": string.Join(", ", Doors.Keys);
        }

        private void AddItem(Item thing)
        {
            Items.Add(thing);
        }

        public Item AddItem(string itemName, string description = "No description")
        {
            var item = new Item(itemName, description);
            AddItem(item);
            return item;
        }

        public Room OpenDoor(string direction)
        {
            Direction enumDirection = Entities.GetDirection(direction);

            return OpenDoor(enumDirection);
        }

        public Room OpenDoor(Direction direction)
        {
            if(Doors.ContainsKey(direction))
            {
                return Doors[direction].Target;
            }
            else
            {
                return null;
            }
        }

        internal void UnlockDoor(Direction direction)
        {
            var oppositeDirection = Entities.OppositeDirection(direction);

            Doors[direction].Locked = false;
            Doors[direction].Target.Doors[oppositeDirection].Locked = false;
        }
    }
}
