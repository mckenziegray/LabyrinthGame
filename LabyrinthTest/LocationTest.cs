using Labyrinth;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabyrinthTest
{
    [TestClass]
    class LocationTest
    {
        [TestMethod]
        public void GetNeighborTest()
        {
            Location location = new Location();
            Location neighbor = new Location();
            int dirInt = Utils.Random.Next(Enum.GetValues(typeof(Direction)).Length);
            Direction dir = (Direction)dirInt;
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                Assert.AreEqual(null, location.GetNeighbor(d));
            }

            location.Neighbors[dirInt] = neighbor;
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                if (d == dir)
                    Assert.AreEqual(null, location.GetNeighbor((d)));
                else
                    Assert.AreEqual(neighbor, location.GetNeighbor((d)));
            }
        }
    }
}
