using System.Data;

namespace Labyrinth
{
    /// <summary>
    /// Provides access to the Item table in the database
    /// </summary>
    static class ItemDao
    {
        private static DataTable m_table;

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
    }
}
