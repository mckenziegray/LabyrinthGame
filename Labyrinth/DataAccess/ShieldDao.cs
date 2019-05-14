using System.Data;

namespace Labyrinth
{
    /// <summary>
    /// Provides access to the Shield table in the database
    /// </summary>
    static class ShieldDao
    {
        private static DataTable m_table;

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
    }
}
