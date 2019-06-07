using Microsoft.VisualStudio.TestTools.UnitTesting;
using Labyrinth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LabyrinthTest
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void GiveItemTest()
        {
            Player player = new Player();
            Item item = Item.RandomItem(player.Items);
            int count = item.Count;
            Assert.IsFalse(player.Items.Contains(item.ItemType));
            
            player.GiveItem(item);
            Assert.AreEqual(count, player.Items.CountOf(item.ItemType));

            player.GiveItem(item);
            if (item.Stackable)
                Assert.AreEqual(count * 2, player.Items.CountOf(item.ItemType));
            else
                Assert.AreEqual(count, player.Items.CountOf(item.ItemType));
        }

        [TestMethod]
        public void GiveLootTest()
        {
            int numItems = Utils.Random.Next(2, 10);
            ItemList items = ItemList.RandomLoot(numItems);
            Player player = new Player();

            Assert.IsFalse(player.Items.Any());

            player.GiveLoot(items);
            Assert.IsTrue(items.SequenceEqual(player.Items));
        }

        [TestMethod]
        public void GiveXpTest()
        {
            Player player = new Player();
            Assert.AreEqual(0, player.XP);
            Assert.AreEqual(1, player.Level);

            int xp = Utils.Random.Next(TestUtils.MAX_VALUE);
            int level = Array.IndexOf(player.XP_TO_LEVEL_UP, Array.Find(player.XP_TO_LEVEL_UP.Reverse().ToArray(), x => x <= xp)) + 2;
            player.GiveXP(xp);
            Assert.AreEqual(xp, player.XP);
            Assert.AreEqual(level, player.Level);
        }

        [TestMethod]
        public void SpendGoldTest()
        {
            Player player = new Player();
            Item gold = new Item(ItemType.Gold, Utils.Random.Next(2, TestUtils.MAX_VALUE));
            player.GiveItem(gold);
            Assert.AreEqual(gold.Count, player.Items.CountOf(ItemType.Gold));

            player.SpendGold(gold.Count - 1);
            Assert.AreEqual(1, player.Items.CountOf(ItemType.Gold));

            player.SpendGold(gold.Count);
            Assert.AreEqual(0, player.Items.CountOf(ItemType.Gold));
        }

        [TestMethod]
        public void SellJunkTest()
        {
            Player player = new Player();
            Item item = TestUtils.RandomNonStackableItem();
            player.GiveItem(item);
            Assert.AreEqual(0, player.JunkValue);

            player.GiveItem(item);
            Assert.AreEqual(item.Value, player.JunkValue);
            Assert.AreEqual(0, player.Items.CountOf(ItemType.Gold));

            player.SellJunk();
            Assert.AreEqual(item.Value, player.Items.CountOf(ItemType.Gold));
        }

        [TestMethod]
        public void UsePotionTest()
        {
            Player player = new Player();
            Item potion = new Item(ItemType.Potions, 2);
            int damage = Math.Clamp(player.CurrentHP - Item.POTION_HEAL_AMOUNT, 1, player.MaxHP - 1);
            player.Damage(damage);
            Assert.ThrowsException<KeyNotFoundException>(() => player.UsePotion());
            Assert.AreEqual(player.MaxHP - damage, player.CurrentHP);

            player.GiveItem(potion);
            int amountHealed = player.UsePotion();
            Assert.AreEqual(damage, amountHealed);
            Assert.AreEqual(player.MaxHP, player.CurrentHP);
            Assert.AreEqual(1, player.Items.CountOf(ItemType.Potions));
        }

        [TestMethod]
        public void UseArrowTest()
        {
            Player player = new Player();
            Item arrow = new Item(ItemType.Arrows, 2);
            Assert.ThrowsException<KeyNotFoundException>(() => player.UseArrow());

            player.GiveItem(arrow);
            var result = player.UseArrow();
            AttackResult attackResult = result.Item1;
            int damage = result.Item2;
            switch(attackResult)
            {
                case AttackResult.Miss:
                    Assert.AreEqual(0, damage);
                    break;
                case AttackResult.Hit:
                    Assert.AreEqual((int)(player.Power * Item.BOW_DAMAGE_MULTIPLIER), damage);
                    break;
                case AttackResult.Crit:
                    Assert.AreEqual((int)(player.Power * Item.BOW_DAMAGE_MULTIPLIER * Unit.CRIT_MULTIPLIER), damage);
                    break;
            }
            Assert.AreEqual(1, player.Items.CountOf(ItemType.Arrows));
        }

        [TestMethod]
        public void AttackWithBowTest()
        {
            Player player = new Player();
            int hp = Utils.Random.Next(1, TestUtils.MAX_VALUE);
            Unit target = new Unit(hp, 0, 0);
            Item bow = new Item(ItemType.Bow);
            Item arrow = new Item(ItemType.Arrows, 1);
            Assert.ThrowsException<KeyNotFoundException>(() => player.AttackWithBow(target));

            player.GiveItem(arrow);
            Assert.ThrowsException<KeyNotFoundException>(() => player.AttackWithBow(target));

            player.GiveItem(bow);
            var result = player.AttackWithBow(target);
            AttackResult attackResult = result.Item1;
            int damage = result.Item2;
            switch (attackResult)
            {
                case AttackResult.Miss:
                    Assert.AreEqual(0, damage);
                    break;
                case AttackResult.Hit:
                    Assert.AreEqual((int)(player.Power * Item.BOW_DAMAGE_MULTIPLIER), damage);
                    break;
                case AttackResult.Crit:
                    Assert.AreEqual((int)(player.Power * Item.BOW_DAMAGE_MULTIPLIER * Unit.CRIT_MULTIPLIER), damage);
                    break;
            }
            Assert.AreEqual(hp - damage, target.CurrentHP);
            Assert.AreEqual(0, player.Items.CountOf(ItemType.Arrows));
            Assert.ThrowsException<KeyNotFoundException>(() => player.AttackWithBow(target));
        }
    }
}
