using System.Collections.Generic;
using System.Data;

namespace Labyrinth
{
    /// <summary>
    /// Stores data for an entry in the Armor table
    /// </summary>
    public class ArmorDataEntry
    {
        public ArmorType ArmorType { get; set; }
        public int Defense { get; set; }
        public int Value { get; set; }
    }

    /// <summary>
    /// Provides access to the Armor table in the database
    /// </summary>
    static class ArmorDao
    {
        private static DataTable m_table;
        private static Dictionary<ArmorType, ArmorDataEntry> m_dataEntries;

        /// <summary>
        /// Retrieves the Armor table from the database
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing the data in the table</returns>
        public static DataTable GetTable()
        {
            if (m_table == null)
            {
                m_table = Dao.GetTable(nameof(Armor));
            }

            return m_table;
        }

        /// <summary>
        /// Returns data from the Armor table as a collection of <see cref="ArmorDataEntry"/>
        /// </summary>
        /// <returns>A collection of <see cref="ArmorDataEntry"/> containing Armor table data</returns>
        public static Dictionary<ArmorType, ArmorDataEntry> GetData()
        {
            if (m_dataEntries == null)
            {
                m_dataEntries = Dao.ExtractData<ArmorType, ArmorDataEntry>(GetTable());
            }

            return m_dataEntries;
        }
    }
}
