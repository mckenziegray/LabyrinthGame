using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth
{
    class Item
    {
        private static readonly int NUM_ITEMS = Enum.GetNames(typeof(ItemType)).Length;
        private const float CHANCE_FOR_GOLD = 0.4f;
        private const int MAX_INITIAL_GOLD = 5;
        private const int MAX_INITIAL_ARROWS = 3;
        public const float BOW_DAMAGE_MULTIPLIER = 1.5f;
        public const int POTION_HEAL_AMOUNT = 10;

        private Random m_random = new Random();

        public ItemType ItemType { get; private set; }
        public int Count { get; set; }
        public int Value { get; private set; } //TODO: Set Value for all item types

        /// <summary>
        /// True if more than one of the item can be held; false otherwise
        /// </summary>
        public bool Stackable
        {
            get
            {
                return ItemType == ItemType.Arrows || ItemType == ItemType.Gold || ItemType == ItemType.Potions;
            }
        }

        /// <summary>
        /// Creates an <see cref="Item"/> object of a particular type with a random count, if applicable
        /// </summary>
        /// <param name="type">The type of object to create</param>
        public Item(ItemType type)
        {
            ItemType = type;

            if (type == ItemType.Gold)
                Count = m_random.Next(1, MAX_INITIAL_GOLD);
            else if (type == ItemType.Arrows)
                Count = m_random.Next(1, MAX_INITIAL_ARROWS);
            else
                Count = 1;
        }

        /// <summary>
        /// Creates an <see cref="Item"/> object
        /// </summary>
        /// <param name="type">The type of item to create</param>
        /// <param name="count">The count for the item</param>
        public Item(ItemType type, int count)
        {
            ItemType = type;
            Count = count;
        }

        /// <summary>
        /// Returns a random item according to special logic
        /// </summary>
        /// <param name="currentLoot">The list of items to which the random item will be added - this will affect the item that is returned</param>
        /// <returns>A random item that is not contained in the given item list, or gold</returns>
        public static Item RandomItem(ItemList currentLoot)
        {
            float roll = Utils.GetRandomPercent();

            float defaultChance = (1.0f - CHANCE_FOR_GOLD) / (NUM_ITEMS - 1); //Chance for remaining items
            float chance = 0.0f;

            foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
            {
                if (itemType == ItemType.Gold || (currentLoot?.Values.Any(i => i.ItemType == itemType) ?? false))
                    continue;

                chance += defaultChance;
                if (roll < chance)
                {
                    switch(itemType)
                    {
                        case ItemType.Weapon:
                            return new Weapon();
                        case ItemType.Armor:
                            return new Armor();
                        case ItemType.Shield:
                            return new Shield();
                        default:
                            return new Item(itemType);
                    }
                }
            }

            return new Item(ItemType.Gold);
        }
    }
}
