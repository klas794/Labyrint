using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    enum Direction { East, West, North, South, None }

    class Entities
    {
        public const string DefaultPlayerName = "John Doe";

        public static bool IsDirection(string text)
        {
            var directionValues = Enum.GetValues(typeof(Direction));
            
            foreach (var item in directionValues)
            {
                if (item.ToString().ToLower() == text)
                {
                    return true;
                }
            }

            return false;
        }

        public static Direction GetDirection(string directionText)
        {
            var directionValues = Enum.GetValues(typeof(Direction));

            foreach (Direction item in directionValues)
            {
                if (item.ToString().ToLower() == directionText)
                {
                    return item;
                }
            }

            return Direction.None;
        }

        public static Direction OppositeDirection(Direction direction)
        {
            if (direction == Direction.East) return Direction.West;
            else if (direction == Direction.West) return Direction.East;
            else if (direction == Direction.North) return Direction.South;
            else return Direction.North;
        }

        
        public static void ConnectRooms( ref Room roomOne, ref Room roomTwo, 
            Direction roomOneDirection, string doorDescription, bool locked = false)
        {
            
            var roomTwoDirection = OppositeDirection(roomOneDirection);

            roomOne.AddDoor(roomOneDirection, new Door(doorDescription, locked));
            roomTwo.AddDoor(roomTwoDirection, new Door(doorDescription, locked));
            
            roomOne.PointDoor(roomOneDirection, roomTwo);
            roomTwo.PointDoor(roomTwoDirection, roomOne);

           
        }
        
        public static void ConnectItems(ref Item itemOne, ref Item itemTwo, Item product)
        {
            itemOne.CombineWithItem(itemTwo, product);
            itemTwo.CombineWithItem(itemOne, product);
        }
    }
}
