using System.Collections.Generic;
using System.Data;

namespace Labyrinth
{
    /// <summary>
    /// Stores data for an entry in the Item table
    /// </summary>
    public class ItemDataEntry
    {
        public ItemType ItemType { get; set; }
        public int Value { get; set; }
        public bool Stackable { get; set; }
        public int MaxInitialCount { get; set; }
    }

    /// <summary>
    /// Provides access to the Item table in the database
    /// </summary>
    public static class ItemDao
    {
        private static DataTable m_table;
        private static Dictionary<ItemType, ItemDataEntry> m_dataEntries;

        /// <summary>
        /// Retrieves the Item table from the database
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing the data in the table</returns>
        public static DataTable GetTable()
        {
            if (m_table == null)
            {
                m_table = Dao.GetTable(nameof(Item));
            }

            return m_table;
        }

        /// <summary>
        /// Returns data from the Item table as a collection of <see cref="ItemDataEntry"/>
        /// </summary>
        /// <returns>A collection of <see cref="ItemDataEntry"/> containing Item table data</returns>
        public static Dictionary<ItemType, ItemDataEntry> GetData()
        {
            if (m_dataEntries == null)
            {
                m_dataEntries = Dao.ExtractData<ItemType, ItemDataEntry>(GetTable());
            }

            return m_dataEntries;
        }
    }
}
