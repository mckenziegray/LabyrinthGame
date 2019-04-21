using System;

namespace Labyrinth
{
    class Maze
    {
        private const float CHANCE_FOR_ROOM = 1.0f / 3.0f;

        public Map Map { get; private set; }
        public Location[][] Network { get; private set; }

        public Maze(int size)
        {
            this.Map = new Map(size);

            Network = Utils.InitializeMatrix<Location>(size, null);

            // Place locations in the network wherever the map does not have a wall
            for (int i = 0; i < this.Map.Grid.Length; i++)
            {
                for (int j = 0; j < this.Map.Grid[i].Length; j++)
                {
                    if (Map.Grid[i][j] == Map.PASSAGE)
                    {
                        Network[i][j] = new Location(Utils.Roll(CHANCE_FOR_ROOM));
                    }
                }
            }

            // Wire together neighbors for each location
            for (int i = 0; i < Network.Length; i++)
            {
                for (int j = 0; j < Network[i].Length; j++)
                {
                    if (Network[i][j] != null)
                    {
                        ConnectNeighbors(i, j);
                    }
                }
            }
        }

        public void Print()
        {
            foreach (Location[] row in Network)
            {
                foreach (Location cell in row)
                {
                    if (cell == null || cell.Neighbors[(int)Direction.North] == null)
                    {
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine();

                foreach (Location cell in row)
                {
                    if (cell == null)
                    {
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.Write(cell.Neighbors[(int)Direction.West] == null ? ' ' : '-');
                        Console.Write(cell.IsRoom ? 'R' : 'P');
                        Console.Write(cell.Neighbors[(int)Direction.East] == null ? ' ' : '-');
                    }
                }
                Console.WriteLine();

                foreach (Location cell in row)
                {
                    if (cell == null || cell.Neighbors[(int)Direction.South] == null)
                    {
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine();
            }
        }

        private void ConnectNeighbors(int row, int col)
        {
            if (row - 1 >= 0)
            {
                Network[row][col].Neighbors[(int)Direction.North] = Network[row - 1][col];
            }

            if (row + 1 < Network.Length)
            {
                Network[row][col].Neighbors[(int)Direction.South] = Network[row + 1][col];
            }

            if (col - 1 >= 0)
            {
                Network[row][col].Neighbors[(int)Direction.West] = Network[row][col - 1];
            }
            
            if (col + 1 < Network.Length)
            {
                Network[row][col].Neighbors[(int)Direction.East] = Network[row][col + 1];
            }
        }
    }
}
