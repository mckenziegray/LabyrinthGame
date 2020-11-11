using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth
{
    class Program
    {
        private const int STARTING_DIFFICULTY = 1;

        static void Main(string[] args)
        {
            Dao.InitializeDatabase();

            Game game = new Game(STARTING_DIFFICULTY);
            game.Start();
        }
    }
}
