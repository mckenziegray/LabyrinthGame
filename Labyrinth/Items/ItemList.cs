using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth
{
    /// <summary>
    /// A specialized container designed to hold <see cref="Item"/>
    /// </summary>
    class ItemList : IEnumerable<Item>
    {
        private Dictionary<ItemType, Item> items;

        public bool IsReadOnly => false;
        public int Count
        {
            get
            {
                return items.Count(kvp => this.Contains(kvp.Key));
            }
        }

        public ItemList()
        {
            items = new Dictionary<ItemType, Item>();
        }

        /// <summary>
        /// Gets the item of the given type
        /// </summary>
        /// <param name="key">The <see cref="ItemType"/> of the item to retrieve.</param>
        /// <returns>The item of the corresponding type</returns>
        public Item this[ItemType key]
        {
            get
            {
                if (!items.ContainsKey(key))
                {
                    return null;
                }

                return items[key];
            }
            private set
            {
                items[key] = value;
            }
        }

        /// <summary>
        /// Adds an item to the list if it does not already exist, or increases the count if the item does already exist
        /// </summary>
        /// <param name="item"></param>
        public void Add(Item item)
        {
            if (items.ContainsKey(item.ItemType))
            {
                items[item.ItemType].Count += item.Count;
            }
            else
            {
                items.Add(item.ItemType, item);
            }
        }

        /// <summary>
        /// Adds all items in a collection to the list
        /// </summary>
        /// <param name="list">The items to add</param>
        public void AddRange(IEnumerable<Item> list)
        {
            foreach (Item item in list)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Adds the item to the list if it is: 1) new; 2) better than the current item of that type; or 3) stackable
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="compareFunc">The function to use for comparing the items to determine which is better.</param>
        /// <returns>The item that was junked (if applicable)</returns>
        public Item AddOrReplace(Item item, Func<Item, Item, bool> compareFunc = null)
        {
            Item itemNotKept = null;

            // Either the list doesn't contain the item, or the list can hold more
            if (!items.ContainsKey(item.ItemType) || item.Stackable)
            {
                Add(item);
            }
            // The new item is better than the current one; replace it
            else if (compareFunc != null && compareFunc(item, this[item.ItemType]))
            {
                itemNotKept = this[item.ItemType];
                this[item.ItemType] = item;
            }
            // The new item is not as good as the current one; junk it
            else
            {
                itemNotKept = item;
            }

            return itemNotKept;
        }

        /// <summary>
        /// Returns true if the list contains the given item
        /// </summary>
        /// <param name="item">The type of item to check for</param>
        /// <param name="amount">The number of items to consume; defaults to 1</param>
        /// <returns>True if there are at least <paramref name="amount"/> of the given item in the list.</returns>
        public bool Contains(ItemType item, int amount = 1)
        {
            return items.ContainsKey(item) && items[item] != null && items[item].Count >= amount;
        }

        /// <summary>
        /// Decrements the count of the given item and removes the item if there are none left.
        /// </summary>
        /// <param name="item">The item being consumed</param>
        /// <param name="amount">The number of items to consume</param>
        public void Use(ItemType item, int amount = 1)
        {
            if (!Contains(item, amount))
            {
                throw new KeyNotFoundException($"{this.GetType().Name} contains no {item}.");
            }
            else
            {
                items[item].Count -= amount;
            }

            if (items[item].Count < 1)
            {
                items.Remove(item);
            }
        }

        public int CountOf(ItemType item)
        {
            return Contains(item) ? items[item].Count : 0;
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return items.Values.Where(i => i?.Count > 0).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.Values.Where(i => i?.Count > 0).GetEnumerator();
        }
    }
}
