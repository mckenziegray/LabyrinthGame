﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth
{
    public class Player : Unit
    {
        private const int STARTING_HP = 10;
        private const int STARTING_POWER = 1;
        private const int STARTING_DEFENSE = 1;
        public readonly int[] XP_TO_LEVEL_UP =
        {
            50,     // XP for level 2
            150,
            300,
            500,
            750,
            1050,
            1400,
            1800,
            2250    // XP for level 10
        };

        public event EventHandler<ItemGainedEventArgs> ItemGained;
        public event EventHandler<StatsIncreasedEventArgs> StatsIncreased;

        public int Level { get; private set; }
        public int JunkValue { get; private set; }

        /// <summary>
        /// Constructs a <see cref="Player"/> object with default starting stats
        /// </summary>
        public Player() : base(STARTING_HP, STARTING_POWER, STARTING_DEFENSE)
        {
            Level = 1;
            JunkValue = 0;
            UpdateStats();
        }

        /// <summary>
        /// Updates the player's stats according to the current <see cref="Level"/> and <see cref="Unit.Items"/>
        /// </summary>
        /// <returns>A list of tuples representing the stats that increased</returns>
        private List<StatIncrease> UpdateStats()
        {
            List<StatIncrease> increasedStats = new();

            int newPower = Level + (Items.Contains(ItemType.Weapon) ? (Items[ItemType.Weapon] as Weapon).Damage : 0);
            if (newPower != Power)
            {
                increasedStats.Add(new StatIncrease(Stats.POWER, newPower - Power, newPower));
                Power = newPower;
            }

            int newDefense = Level - 1 + (Items.Contains(ItemType.Armor) ? (Items[ItemType.Armor] as Armor).Defense : 0) 
                + (Items.Contains(ItemType.Shield) ? (Items[ItemType.Shield] as Shield).Defense : 0);
            if (newDefense != Defense)
            {
                increasedStats.Add(new StatIncrease(Stats.DEFENSE, newDefense - Defense, newDefense));
                Defense = newDefense;
            }

            return increasedStats;
        }

        /// <summary>
        /// Increases the player's XP.
        /// Increases the player's level and updates stats if XP is high enough.
        /// Invokes the <see cref="StatsIncreased"/> event.
        /// </summary>
        /// <param name="amount">The amount of XP to give to the player</param>
        /// <returns>True if the player leveled up; false otherwise</returns>
        public bool GiveXP(int amount)
        {
            List<StatIncrease> increasedStats = new();
            int curLevel = Level;
            bool leveledUp = false;

            XP += amount;
            increasedStats.Add(new(Stats.XP, amount, XP));

            for (int i = 0; i < XP_TO_LEVEL_UP.Length; i++)
            {
                int newLevel = i + 2;

                // Player has enough xp to reach level [i+2]
                if (XP > XP_TO_LEVEL_UP[i])
                {
                    if (newLevel > Level)
                    {
                        Level = newLevel;
                        leveledUp = true;
                    }
                }
                else
                {
                    break;
                }
            }

            if (leveledUp)
            {
                increasedStats.Add(new StatIncrease(Stats.LEVEL, Level - curLevel, Level));
                increasedStats.AddRange(UpdateStats());

                increasedStats = OrderStatsList(increasedStats);
            }

            StatsIncreased?.Invoke(this, new StatsIncreasedEventArgs() { StatsIncreased = increasedStats });

            return leveledUp;
        }

        /// <summary>
        /// Sorts a list of stats according to the order in which their increases should be displayed
        /// </summary>
        /// <param name="stats">The list of stats to sort</param>
        /// <returns>A new list that has been sorted</returns>
        private List<StatIncrease> OrderStatsList(List<StatIncrease> stats)
        {
            List<StatIncrease> orderedStats = new();

            if (stats.Any(s => s.Stat == Stats.XP))
                orderedStats.Add(stats.Single(s => s.Stat == Stats.XP));

            if (stats.Any(s => s.Stat == Stats.LEVEL))
                orderedStats.Add(stats.Single(s => s.Stat == Stats.LEVEL));

            orderedStats.AddRange(stats.Where(s => !orderedStats.Contains(s)));

            return orderedStats;
        }

        /// <summary>
        /// Gives an item to the player
        /// </summary>
        /// <param name="item">The item to give to the player</param>
        /// <param name="firstItem">Whether this is the first item in a group that the player is receiving</param>
        public void GiveItem(Item item)
        {
            switch (item.ItemType)
            {
                case ItemType.Weapon:
                    AddOrReplaceItem(item);
                    break;
                case ItemType.Armor:
                    AddOrReplaceItem(item);
                    break;
                case ItemType.Shield:
                    AddOrReplaceItem(item);
                    break;
                case ItemType.Bow:
                    AddOrReplaceItem(item);
                    break;
                case ItemType.Arrows:
                case ItemType.Potions:
                case ItemType.Gold:
                    AddOrReplaceItem(item);
                    break;
                default:
                    throw new ArgumentException($"Unknown item of type {item.ItemType}");
            }
        }

        /// <summary>
        /// Gives multiple items to the player
        /// </summary>
        /// <param name="items">The items to give to the player</param>
        public void GiveLoot(IEnumerable<Item> items)
        {
            for (int i = 0; i < items.Count(); i++)
            {
                GiveItem(items.ElementAt(i));
            }
        }

        /// <summary>
        /// Causes the player to consume a potion, if they have one.
        /// Throws <see cref="KeyNotFoundException"/> if the player does not have any potions.
        /// </summary>
        /// <returns>The amount of <see cref="Unit.CurrentHP"/> restored from consuming the potion</returns>
        public int UsePotion()
        {
            Items.Use(ItemType.Potions);
            return Heal(Item.POTION_HEAL_AMOUNT);
        }

        /// <summary>
        /// Causes the player to use an arrow.
        /// Throws an <see cref="KeyNotFoundException"/> if the player does not have any arrows
        /// </summary>
        /// <returns>A tuple containing the result of the attack and the amount of damage it does</returns>
        public (AttackResult Result, int Damage) UseArrow()
        {
            Items.Use(ItemType.Arrows);

            (AttackResult Result, int Damage) result = RollDamage();
            return new(result.Result, (int)(result.Damage * Item.BOW_DAMAGE_MULTIPLIER));
        }

        /// <summary>
        /// Causes the player to use the given amount of gold
        /// </summary>
        /// <param name="amount">The amount of gold to spend</param>
        public void SpendGold(int amount)
        {
            Items.Use(ItemType.Gold, amount);
        }

        /// <summary>
        /// Causes the player to attack the given <see cref="Unit"/> using a bow
        /// Throws an <see cref="KeyNotFoundException"/> if the player does not have a bow or arrows
        /// </summary>
        /// <param name="other">The <see cref="Unit"/> to attack</param>
        /// <returns>A tuple containing the result of the attack and the amount of damage it does</returns>
        public (AttackResult Result, int Damage) AttackWithBow(Unit other)
        {
            if (!Items.Contains(ItemType.Bow))
                throw new KeyNotFoundException($"The player does not have a {ItemType.Bow}.");

            (AttackResult Result, int Damage) attackResult = UseArrow();
            other.Damage(attackResult.Damage - other.Defense);

            return attackResult;
        }

        /// <summary>
        /// Adds the item to the player's list of items if it is: 1) new; 2) better than the current item of that type; or 3) stackable.
        /// Adds the value of the item that was not kept to <see cref="JunkValue"/>.
        /// Invokes the <see cref="ItemGained"/> event.
        /// </summary>
        /// <param name="newItem">The item to give the player</param>
        /// <param name="compareFunc">The function for comparing the new item to the existing one</param>
        /// <param name="firstItem">Whether this is the first item in a group that the player is receiving</param>
        private void AddOrReplaceItem(Item newItem, bool firstItem = true)
        {
            Item junkedItem = Items.AddOrReplace(newItem);

            if (junkedItem != null)
                JunkValue += junkedItem.Value;

            ItemGainedEventArgs eventArgs = new ItemGainedEventArgs() {
                Item = newItem,
                ItemKept = junkedItem != newItem,
                FirstItem = firstItem
            };

            UpdateStats();

            ItemGained?.Invoke(this, eventArgs);
        }

        public void SellJunk()
        {
            if (JunkValue > 0)
                Items.Add(new Item(ItemType.Gold, JunkValue));
        }
    }
}
