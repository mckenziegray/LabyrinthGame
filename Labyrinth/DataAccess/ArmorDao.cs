using System.Data;

namespace Labyrinth
{
    /// <summary>
    /// Provides access to the Armor table in the database
    /// </summary>
    static class ArmorDao
    {
        private static DataTable m_table;

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
    }
}
