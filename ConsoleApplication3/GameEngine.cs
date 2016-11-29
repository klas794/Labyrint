using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ConsoleApplication3
{
    class GameEngine
    {
        private Room _location;
        private Player _player;

        public void CreateWorld()
        {
            _player = new Player();

            string[] doorTypes =
            {
                "Wooden door with light green paint on the surface.",
                "White painted door with square insets of glass.",
                "Black painted door with no windows."
            };

            Room livingRoom = new Room(
                "The living room is large and dusty. There are framed pictures of ancestors on the\n" +
                "walls and it has an old red carpet on the floor.");

            var candleHolder = livingRoom.AddItem("candle holder", "The holder fits four candles, one hole is plugged.");
            var key = livingRoom.AddItem("key", "There is a small engravement that says: \"Backyard\"");
            Item moose = livingRoom.AddItem("stuffed moose head");
            
            Room hallWay = new Room(
                "The hallway alley is narrow and the wallpaper is loosening in\n" +
                "its corners. A rat runs across it, leaving a trace from tiny feet");
            
            hallWay.AddItem("boots", "The boots have stars decorated on each side");
            hallWay.AddItem("hanger", "The hanger is made of steel and a bit twisted");

            Room walkingCloset = new Room(
                "The space inside the closet is dry and has a damped light.\n" +
                "The hangers are all gone.");

            walkingCloset.AddItem("shirt", "The shirt is made of silk");
            Item can = walkingCloset.AddItem("can", "The can once contained tomatoes");

            Room paintStudio = new Room(
                "This room belongs to a talented painter. Huge half-finished paintings are \n" +
                "leaned against the wall. There is no light");

            var matchBox = paintStudio.AddItem("matchbox", "The box has one match inside.");
            var candle = paintStudio.AddItem("candle", "An unsused candle");

            Room backYard = new Room(
                "The back yard has decorating bushes shaped like horses,\n" +
                "and a short cut green lawn.\n" +
                "This is the ENDPOINT");
            
            Entities.ConnectRooms(ref hallWay, ref livingRoom, Direction.North, doorTypes[0]);
            Entities.ConnectRooms(ref livingRoom, ref walkingCloset, Direction.East, doorTypes[1]);
            Entities.ConnectRooms(ref livingRoom, ref paintStudio, Direction.North, doorTypes[1]);
            Entities.ConnectRooms(ref livingRoom, ref backYard, Direction.West, doorTypes[2], true);
            
            Item mooseWithHat = new Item("moose head with can hat", "Constructed item");
            Item litCandle = new Item("lit candle", "A candle burning.");
            Item mountedCandleHolder = new Item("mounted candle holder", "Constructed item");
            Item litCandleHolder = new Item("lit mounted candle holder", 
                "A candle holder with one lit candle in it");

            Entities.ConnectItems(ref moose, ref can, mooseWithHat);
            Entities.ConnectItems(ref matchBox, ref candle, litCandle);
            Entities.ConnectItems(ref litCandle, ref candleHolder, litCandleHolder);
            Entities.ConnectItems(ref candleHolder, ref candle, mountedCandleHolder);
            Entities.ConnectItems(ref mountedCandleHolder, ref matchBox, litCandleHolder);

            _location = hallWay;
        }

        public void RunGameLoop()
        {
            var gameOn = true;
            var awaitingValidAction = true;
            string action;

            if (_location == null)
            {
                throw new Exception("Game misconfigured: no starting point");
            }
            else
            {
                ShowRoom();
            }
            

            while (gameOn)
            {
                Console.WriteLine("\nWhat do you want to do?");
                awaitingValidAction = true;

                while (awaitingValidAction)
                {
                    action = Console.ReadLine();
                    action = action.ToLower();
                    awaitingValidAction = false;

                    if (action == "exit")
                    {
                        gameOn = false;
                    }
                    else if(action == "look")
                    {
                        ShowRoom();
                    }
                    else if (action == "character" || action == "player" 
                        || action == "inventory" || action == "show inventory")
                    {
                        ShowCharacter();
                    }
                    else if(Regex.IsMatch(action, @"^go\s.+"))
                    {
                        string directionText = Regex.Replace(action, @"^go\s(.+)", "$1");

                        Direction direction = Entities.GetDirection(directionText);
                        
                        TryWalking(direction);
                        
                    }
                    else if(Regex.IsMatch(action, @"^get\s.+"))
                    {
                        var itemName = Regex.Replace(action, @"^get\s(.+)", "$1");
                        GetItem(itemName);
                    }
                    else if (Regex.IsMatch(action, @"^drop\s.+"))
                    {
                        var itemName = Regex.Replace(action, @"^drop\s(.+)", "$1");
                        DropItem(itemName);
                    }
                    else if (Regex.IsMatch(action, @"^inspect\s\w+"))
                    {
                        var itemText = Regex.Replace(action, @"^inspect\s([\w\s]+)", "$1");
                        var item = _player.Items.Find(x => x.Name == itemText);

                        if(item == null )
                        {
                            item = _location.Items.Find(x => x.Name == itemText);
                        }

                        if (item != null)
                        {
                            Console.WriteLine(item.Description);
                        }
                        else
                        {
                            if(!InspectAllDoors(itemText))
                            {
                                Console.WriteLine("Unknown item");
                            }
                        }
                    }
                    else if (Regex.IsMatch(action, @"^use[\w\s]+\son\s[\w\s]+"))
                    {
                        var objects = Regex.Match(action, @"^use\s([\w\s]+)\son\s([\w\s]+)");

                        var key1 = objects.Groups[1].Value;
                        var key2 = objects.Groups[2].Value;

                        if(key1 == "key" && key2 == "door")
                        {
                            if(_player.HasAKey())
                            {
                                Console.WriteLine("You do not own a key");
                            }
                            else
                            {
                                UnlockAllDoors(); 
                            }
                        }
                        else
                        {
                            TryMatchItems(key1, key2);
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("I dont understand, try again.");
                        awaitingValidAction = true;
                    }
                }
            }

            
        }

        private bool InspectAllDoors(string doorName)
        {
            bool doorFound = false;

            if(doorName == "door")
            {
                if (_location.Doors.Count == 1)
                {
                    Console.WriteLine(_location.Doors.First().Value.Description);
                }
                else if(_location.Doors.Count > 1)
                {
                    Console.WriteLine("Specifiy which door to look at (use direction)");
                }
                doorFound = true;
            }

            foreach (var door in _location.Doors)
            {
                var name = door.Key.ToString().ToLower() + " door";
                if (name == doorName)
                {
                    Console.WriteLine(door.Value.Description);
                    doorFound = true;
                    break;
                }
            }

            return doorFound;
        }

        private void UnlockAllDoors()
        {
            List<Direction> doorsUnlocked = new List<Direction>();

            foreach (KeyValuePair<Direction, Door> item in _location.Doors)
            {
                if(item.Value.Locked)
                {
                    _location.UnlockDoor(item.Key);
                    doorsUnlocked.Add(item.Key);
                }
            }
            
            var message = doorsUnlocked.Count == 0 ? "No locked door found" :
                        "Doors unlocked: " + string.Join(", ", doorsUnlocked);

            Console.WriteLine(message);
        }

        private void TryWalking(Direction direction)
        {
            if (direction == Direction.None ||
                !_location.Doors.ContainsKey(direction))
            {
                Console.WriteLine("Invalid direction");
            }
            else
            {
                if (_location.Doors[direction].Locked)
                {
                    Console.WriteLine("The door is locked");
                }
                else
                {
                    _location = _location.Doors[direction].Target;
                    ShowRoom();
                }
            }
        }

        private void TryMatchItems(string item1Name, string item2Name)
        {
            var obj1 = _player.Items.Find(x => x.Name == item1Name);
            var obj2 = _player.Items.Find(x => x.Name == item2Name);

            if (obj1 == null || obj2 == null)
            {
                Console.WriteLine("You dont carry those objects.");
            }
            else if (!obj1.IsUsableWith(obj2))
            {
                Console.WriteLine("Sorry. Those objects cannot be used on each other.");
            }
            else
            {
                var obj3 = obj1.UseWith(obj2);

                _player.Items.Remove(obj1);
                _player.Items.Remove(obj2);
                _player.Items.Add(obj3);

                Console.WriteLine("You obtained {0}!", obj3);
            }
        }

        private void DropItem(string itemName)
        {
            var item = _player.Items.Find(x => x.Name == itemName);

            if (item == null)
            {
                Console.WriteLine("There is no {0} item to drop", itemName);
            }
            else
            {
                _location.Items.Add(item);
                _player.Items.Remove(item);
                Console.Clear();
                Console.WriteLine("Item " + item + " dropped on the floor!");
            }
        }

        private void GetItem(string itemName)
        {
            var item = _location.Items.Find(x => x.Name == itemName);

            if (item == null)
            {
                Console.WriteLine("There is no {0} item on floor", itemName);
            }
            else
            {
                _player.Items.Add(item);
                _location.Items.Remove(item);

                Console.Clear();
                Console.WriteLine("Picked up " + item + "!");
                Console.WriteLine("You have {0} items.", _player.Items.Count);
            }
        }

        private void ShowRoom()
        {
            Console.Clear();
            Console.WriteLine("Room description:\n{0}\n\nItems on the floor:\n{1}\n\nDoors: {2}", 
                _location.Description,
                _location.GetItems(),
                _location.GetDoors()
                );
        }

        private void ShowCharacter()
        {
            Console.Clear();
            Console.WriteLine("Player name: {0}\n\nItems owned: {1}",
                _player.Name,
                _player.GetItems()
                );
        }
    }
}
