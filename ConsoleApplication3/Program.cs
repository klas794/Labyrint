using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {

            var game = new GameEngine();

            game.CreateWorld();

            game.RunGameLoop();
        }
    }
}
