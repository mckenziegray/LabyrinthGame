using System;
using System.Collections.Generic;
using System.Linq;
namespace Labyrinth
{
    public class Map
    {
        public const char PASSAGE = '.';
        public const char WALL = '#';

        private int m_size;

        public char[][] Grid { get; private set; }

        public Map(int size)
        {
            m_size = size;
            Generate();
        }

        /// <summary>
        /// Generates a square maze using Prim's Algorithm
        /// </summary>
        public void Generate()
        {
            Random random = Utils.Random;

            Grid = Utils.InitializeMatrix(m_size, WALL);

            Tuple<int, int> startingCoords = new Tuple<int, int>(random.Next(m_size), random.Next(m_size));
            Grid[startingCoords.Item1][startingCoords.Item2] = PASSAGE;

            List<DoubleCell> frontier = GetNeighborCells(startingCoords);

            while (frontier.Count > 0)
            {

                DoubleCell curCell = frontier[random.Next(frontier.Count)];

                if (Grid[curCell.X2][curCell.Y2] == WALL)
                {
                    Grid[curCell.X1][curCell.Y1] = PASSAGE;
                    Grid[curCell.X2][curCell.Y2] = PASSAGE;

                    frontier.AddRange(GetNeighborCells(curCell.Cell2).Where(w => GetCell(w.Cell2) == WALL));
                }

                frontier.Remove(curCell);
            }
        }

        /// <summary>
        /// Returns the character at the specified coordinates
        /// </summary>
        /// <param name="coords">The coordinates of the cell to return</param>
        /// <returns>The value in the indicated cell</returns>
        public char GetCell(Tuple<int, int> coords)
        {
            return Grid[coords.Item1][coords.Item2];
        }

        /// <summary>
        /// Prints the maze to the console;
        /// </summary>
        public void Print()
        {
            foreach (char[] row in Grid)
            {
                foreach (char cell in row)
                {
                    Console.Write($"{cell} ");
                }

                Console.WriteLine();
            }
        }

        #region private members
        private List<DoubleCell> GetNeighborsX(Tuple<int, int> coords)
        {
            List<DoubleCell> neighbors = new List<DoubleCell>();
            int x = coords.Item1;
            int y = coords.Item2;

            if (x - 2 >= 0)
                neighbors.Add(new DoubleCell(x - 1, y, x - 2, y));
            if (x + 2 < m_size)
                neighbors.Add(new DoubleCell(x + 1, y, x + 2, y));

            return neighbors;
        }

        private List<DoubleCell> GetNeighborsY(Tuple<int, int> coords)
        {
            List<DoubleCell> neighbors = new List<DoubleCell>();
            int x = coords.Item1;
            int y = coords.Item2;

            if (y - 2 >= 0)
                neighbors.Add(new DoubleCell(x, y - 1, x, y - 2));
            if (y + 2 < m_size)
                neighbors.Add(new DoubleCell(x, y + 1, x, y + 2));

            return neighbors;
        }

        private List<DoubleCell> GetNeighborCells(Tuple<int, int> coords)
        {
            return GetNeighborsX(coords).Concat(GetNeighborsY(coords)).ToList();
        }
        #endregion

        private class DoubleCell
        {
            private Tuple<int, int> m_coord1;
            private Tuple<int, int> m_coord2;

            public DoubleCell(Tuple<int, int> cell1, Tuple<int, int> cell2)
            {
                m_coord1 = cell1;
                m_coord2 = cell2;
            }

            public DoubleCell(int x1, int y1, int x2, int y2)
            {
                m_coord1 = new Tuple<int, int>(x1, y1);
                m_coord2 = new Tuple<int, int>(x2, y2);
            }

            public Tuple<int, int> Cell1
            {
                get { return m_coord1; }
            }

            public Tuple<int, int> Cell2
            {
                get { return m_coord2; }
            }

            public int X1
            {
                get { return m_coord1.Item1; }
            }

            public int X2
            {
                get { return m_coord2.Item1; }
            }

            public int Y1
            {
                get { return m_coord1.Item2; }
            }

            public int Y2
            {
                get { return m_coord2.Item2; }
            }
        }
    }
}
