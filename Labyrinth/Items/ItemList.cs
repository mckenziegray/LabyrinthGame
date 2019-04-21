using System;
using System.Collections.Generic;
using System.Text;

namespace Labyrinth
{
    /// <summary>
    /// A specialized container designed to hold <see cref="Item"/>s
    /// </summary>
    class ItemList : Dictionary<ItemType, Item>
    {
        /// <summary>
        /// Adds an item to the dictionary if it does not already exist, or increases the count if the item does already exist
        /// </summary>
        /// <param name="item"></param>
        public void Add(Item item)
        {
            if (this.ContainsKey(item.ItemType))
            {
                this[item.ItemType].Count += item.Count;
            }
            else
            {
                this.Add(item.ItemType, item);
            }
        }

        /// <summary>
        /// Adds all items in the given dictionary
        /// </summary>
        /// <param name="list">The items to add</param>
        public void Add(Dictionary<ItemType, Item> list)
        {
            foreach (Item item in list.Values)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Adds all items in the given list
        /// </summary>
        /// <param name="list">The items to add</param>
        public void Add(IEnumerable<Item> list)
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
        public Item AddOrReplace(Item item, Func<Item, Item, bool> compareFunc)
        {
            Item itemNotKept = null;

            // Either the list doesn't contain the item, or the list can hold more
            if (!this.ContainsKey(item.ItemType) || item.Stackable)
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
        /// Determines if the item exists in the list
        /// </summary>
        /// <param name="item">The type of item to check for</param>
        /// <returns>True if the item exists in the list with a <see cref="Count"/> of at least 1</returns>
        public bool Contains(ItemType item)
        {
            return this.ContainsKey(item) && this[item] != null && this[item].Count > 0;
        }

        /// <summary>
        /// Decrements the count of the given item
        /// </summary>
        /// <param name="item">The item being consumed</param>
        public void UseItem(ItemType item)
        {
            if (this.ContainsKey(item) && this[item] != null)
                --this[item].Count;
        }
    }
}
