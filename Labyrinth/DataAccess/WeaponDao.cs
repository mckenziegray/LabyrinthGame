using System.Collections.Generic;
using System.Data;

namespace Labyrinth
{
    /// <summary>
    /// Stores data for an entry in the Weapon table
    /// </summary>
    public class WeaponDataEntry
    {
        public WeaponType WeaponType { get; set; }
        public int Damage { get; set; }
        public int Value { get; set; }
    }

    /// <summary>
    /// Provides access to the Item table in the database
    /// </summary>
    public static class WeaponDao
    {
        private static DataTable m_table;
        private static Dictionary<WeaponType, WeaponDataEntry> m_dataEntries;

        /// <summary>
        /// Retrieves the Weapon table from the database
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing the data in the table</returns>
        public static DataTable GetTable()
        {
            if (m_table == null)
            {
                m_table = Dao.GetTable(nameof(Weapon));
            }

            return m_table;
        }

        /// <summary>
        /// Returns data from the Weapon table as a collection of <see cref="WeaponDataEntry"/>
        /// </summary>
        /// <returns>A collection of <see cref="WeaponDataEntry"/> containing Weapon table data</returns>
        public static Dictionary<WeaponType, WeaponDataEntry> GetData()
        {
            if (m_dataEntries == null)
            {
                m_dataEntries = Dao.ExtractData<WeaponType, WeaponDataEntry>(GetTable());
            }

            return m_dataEntries;
        }
    }
}
