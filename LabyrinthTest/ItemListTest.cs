using Microsoft.VisualStudio.TestTools.UnitTesting;
using Labyrinth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LabyrinthTest
{
    [TestClass]
    public class ItemListTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            ItemList list = new ItemList();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void AddTest()
        {
            ItemList list = new ItemList();
            Item item = Item.RandomItem(list);
            int count = item.Count;

            list.Add(item);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(count, list.CountOf(item.ItemType));

            list.Add(item);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(count * 2, list.CountOf(item.ItemType));
        }

        [TestMethod]
        public void AddOrReplaceTest()
        {
            ItemList list = new ItemList();
            Item stackableItem = TestUtils.RandomStackableItem();
            Item nonStackableItem = TestUtils.RandomNonStackableItem();
            int count = stackableItem.Count;

            Item junkedItem = list.AddOrReplace(stackableItem);
            Assert.AreEqual(1, list.Count);
            Assert.IsNull(junkedItem);
            Assert.AreEqual(count, list.CountOf(stackableItem.ItemType));

            junkedItem = list.AddOrReplace(stackableItem);
            Assert.AreEqual(1, list.Count);
            Assert.IsNull(junkedItem);
            Assert.AreEqual(count * 2, list.CountOf(stackableItem.ItemType));

            junkedItem = list.AddOrReplace(nonStackableItem);
            Assert.AreEqual(2, list.Count);
            Assert.IsNull(junkedItem);
            Assert.AreEqual(1, list.CountOf(nonStackableItem.ItemType));

            junkedItem = list.AddOrReplace(nonStackableItem);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(nonStackableItem, junkedItem);
            Assert.AreEqual(1, list.CountOf(nonStackableItem.ItemType));

            // TODO: Test a non-stackable item replacing an inferior item
        }

        [TestMethod]
        public void UseTest()
        {
            ItemList list = new ItemList();
            Item item = Item.RandomItem(list);
            Assert.IsFalse(list.Contains(item.ItemType));

            list.Add(item);
            Assert.IsTrue(list.Contains(item.ItemType));
            Assert.AreEqual(item.Count, list.CountOf(item.ItemType));

            list.Use(item.ItemType, item.Count - 1);
            Assert.IsTrue(list.Contains(item.ItemType));
            Assert.AreEqual(item.Count - (item.Count - 1), list.CountOf(item.ItemType));

            list.Use(item.ItemType, 1);
            Assert.IsFalse(list.Contains(item.ItemType));
            Assert.AreEqual(0, list.CountOf(item.ItemType));
        }
    }
}
