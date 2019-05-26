using System.Collections.Generic;
using System.Data;

namespace Labyrinth
{
    /// <summary>
    /// Stores data for an entry in the Shield table
    /// </summary>
    public class ShieldDataEntry
    {
        public ShieldType ShieldType { get; set; }
        public int Defense { get; set; }
        public int Value { get; set; }
    }

    /// <summary>
    /// Provides access to the Shield table in the database
    /// </summary>
    public static class ShieldDao
    {
        private static DataTable m_table;
        private static Dictionary<ShieldType, ShieldDataEntry> m_dataEntries;

        /// <summary>
        /// Retrieves the Shield table from the database
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing the data in the table</returns>
        public static DataTable GetTable()
        {
            if (m_table == null)
            {
                m_table = Dao.GetTable(nameof(Shield));
            }

            return m_table;
        }

        /// <summary>
        /// Returns data from the Shield table as a collection of <see cref="ShieldDataEntry"/>
        /// </summary>
        /// <returns>A collection of <see cref="ShieldDataEntry"/> containing Shield table data</returns>
        public static Dictionary<ShieldType, ShieldDataEntry> GetData()
        {
            if (m_dataEntries == null)
            {
                m_dataEntries = Dao.ExtractData<ShieldType, ShieldDataEntry>(GetTable());
            }

            return m_dataEntries;
        }
    }
}
