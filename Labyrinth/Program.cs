using System;

namespace Labyrinth
{
    internal class Program
    {
        private const int STARTING_DIFFICULTY = 1;

        private static void Main(string[] args)
        {
            Dao.InitializeDatabase();

            Game game = new Game(STARTING_DIFFICULTY);
            game.Start();

            Environment.Exit(0);
        }
    }
}
