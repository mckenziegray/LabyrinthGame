using System;
using System.Data;
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

        private int MaxInitialCount;

        public ItemType ItemType { get; private set; }
        public int Value { get; protected set; } //TODO: Set Value for all item types
        public bool Stackable;
        public int Count;

        /// <summary>
        /// Creates an <see cref="Item"/> object of a particular type with a random count, if applicable
        /// </summary>
        /// <param name="type">The type of object to create</param>
        public Item(ItemType type)
        {
            DataRow entry = ItemDao.GetTable().Select($"{nameof(ItemType)} = '{type}'").FirstOrDefault();

            ItemType = type;
            Value = (int)entry[nameof(Value)];
            Stackable = (bool)entry[nameof(Stackable)];
            MaxInitialCount = (int)entry[nameof(MaxInitialCount)];

            if (Stackable)
                Count = Utils.Random.Next(1, MaxInitialCount);
            else
                Count = 1;
        }

        /// <summary>
        /// Creates an <see cref="Item"/> object
        /// </summary>
        /// <param name="type">The type of item to create</param>
        /// <param name="count">The count for the item</param>
        public Item(ItemType type, int count)
            : this(type)
        {
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
